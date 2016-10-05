using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionGenerator {

    // helper gizmos' position list
    public List<Vector3> helper_gizmos { get; private set; }
    public List<Vector3> target_gizmos { get; private set; }

    private Configuration _config; // configuration
    private Rigidbody _rig_root; // root joint's rigidbody

    private bool _debug; // show debug info

    /* constructor */
    public MotionGenerator (Configuration config, bool debug = false) {
        _config = config;
        _debug = debug;
        _rig_root = _config.root.GetComponent<Rigidbody>();
        helper_gizmos = new List<Vector3>();
        target_gizmos = new List<Vector3>();
    }

    /* generate target rotation of joints */
    public void GeneratePose () {

        helper_gizmos.Clear();
        target_gizmos.Clear();

        // get the current phase
        int phase = PhaseManager.GetCurrentPhase(Time.time);

        /* generate leg pose
        GenLegPose(config.leg_l, phase == 0);
        GenLegPose(config.leg_r, phase != 0);
        */
        GenerateLegPose(_config.leg_l, false);
        GenerateLegPose(_config.leg_r, false);
    }

    /*!
     * generate leg pose, swing == true means this leg is now the swing leg, 
     * otherwise this leg is the stance leg.
     */
    private void GenerateLegPose (GameObject[] leg, bool swing) {
        Vector3 target;
        Vector3[] pos = new Vector3[leg.Length];

        /* first pass solve ik */
        // swing foot 
        if (swing) {
            target = leg[0].transform.position + IPMError();
            target.y = Configuration.kLiftH *
                PhaseManager.InterpolateHeight(Time.time);
        }

        // stance foot 
        else {
            target = leg[0].transform.position;
            target.y = 0;
        }

        // solve ik
        for (int i = 0; i < leg.Length; ++i)
            pos[i] = leg[i].transform.position;
        FABRIKSolver.SolveIKWithVectorConstraint(ref pos, target, _config.root.transform.forward);
        target_gizmos.Add(target);

        /* second pass solve ik */
        target = leg[leg.Length - 1].transform.position;
        target.y = _config.kDH;
        target += _config.kDV * Time.fixedDeltaTime;
        FABRIKSolver.SolveIKWithVectorConstraint(ref pos, target, _config.root.transform.forward, true);

        target_gizmos.Add(target);

        /* add helper gizmos */
        for (int i = 0; i < 3; ++i)
            helper_gizmos.Add(pos[i]);

        /* convert target position into target joint rotation */
        for (int j = 0; j < pos.Length; ++j) {
            JointConfig jnt_config = (JointConfig)_config.jnt_configs[leg[j]];

            // foot joint
            if (j == pos.Length - 1) {
                jnt_config.tar_rot = Quaternion.Inverse(jnt_config.joint.connectedBody.transform.rotation);
                    // Quaternion.LookRotation(Vector3.up, jnt_config.joint.connectedBody.transform.up);
                _config.jnt_configs[leg[j]] = jnt_config;
                continue;
            }

            // calculate local axis
            Vector3 x = (pos[j + 1] - pos[j]).normalized;

            Vector3 y = Vector3.Cross(x, _config.root.transform.forward).normalized;
            if (Vector3.Dot(y, _config.root.transform.up) > 0)
                y = -y;

            Vector3 z = Vector3.Cross(y, x);

            // stance leg's root
            if (!swing && j == 0) {
                jnt_config.tar_rot =
                    Quaternion.Inverse(jnt_config.joint.connectedBody.transform.rotation) *
                    Quaternion.FromToRotation(-Vector3.up, _config.root.transform.right) *
                    Quaternion.LookRotation(z, y);
            }

            // other joints
            else {
                jnt_config.tar_rot = Quaternion.Inverse(jnt_config.joint.connectedBody.transform.rotation) *
                    Quaternion.LookRotation(z, y);
            }

            _config.jnt_configs[leg[j]] = jnt_config;
        }

    }

    private void GenerateTrajectory (footmode mode) {

    }

    /* IPM model calculation */
    private Vector3 IPMError () {
        Vector3 d = _rig_root.velocity;
        Vector3 com = (Vector3)(_config.root.transform.localToWorldMatrix * _config.COM) + _config.root.transform.position;

        float g = Mathf.Abs(Physics.gravity.y);
        if (g == 0) g = 0.1f;

        d *= Mathf.Sqrt(com.y / g + Vector3.SqrMagnitude(d) / (4 * g * g));
        d = Vector3.Magnitude(d) * new Vector3(d.x, 0, d.z).normalized - Configuration.kVAlpha * _config.kDV;
        d.y = 0;

        return d;
    }
}