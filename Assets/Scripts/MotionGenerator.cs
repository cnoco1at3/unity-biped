using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionGenerator {

    // helper gizmos' position list
    public List<Vector3> Helper { get; private set; }
    public List<Vector3> Target { get; private set; }

    private IKSolver ik_solver; // ik solver
    private Configuration config; // configuration
    private Rigidbody rig_root; // root joint's rigidbody

    /* constructor */
    public MotionGenerator (Configuration c) {
        config = c;
        ik_solver = new IKSolver();

        rig_root = config.root.GetComponent<Rigidbody>();

        Helper = new List<Vector3>();
        Target = new List<Vector3>();
    }

    /* generate target rotation of joints */
    public void GeneratePose () {

        Helper.Clear();
        Target.Clear();

        // get the current phase
        int phase = PhaseManager.GetCurrentPhase(Time.time);

        /* generate leg pose
        GenLegPose(config.leg_l, phase == 0);
        GenLegPose(config.leg_r, phase != 0);
        */

        GenLegPose(config.leg_l, false);
        GenLegPose(config.leg_r, false);
    }

    /*!
     * generate leg pose, swing == true means this leg is now the swing leg, 
     * otherwise this leg is the stance leg.
     */
    void GenLegPose (GameObject[] leg, bool swing) {
        Vector3 target;
        Vector3[] pos = new Vector3[leg.Length];

        /* first pass solve ik */
        // swing foot 
        if (swing) {
            target = leg[0].transform.position + CalIPM();
            target.y = Configuration.kLiftH *
                PhaseManager.InterpolateHeight(Time.time);
        }

        // stance foot 
        else {
            target = leg[leg.Length - 1].transform.position;
            target.y = 0;
        }

        // solve ik
        for (int i = 0; i < leg.Length; ++i)
            pos[i] = leg[i].transform.position;
        pos = ik_solver.LimitedSolveIK(pos, target, config.root.transform.forward);
        Target.Add(target);

        /* second pass solve ik */
        target = leg[0].transform.position;
        target.y = config.kDH;
        target += config.kDV * Time.fixedDeltaTime;
        pos = ik_solver.LimitedSolveIK(pos, target,
            config.root.transform.forward, true);

        Target.Add(target);

        /* add helper gizmos */
        for (int i = 0; i < 3; ++i)
            Helper.Add(pos[i]);

        /* convert target position into target joint rotation */
        for (int j = 0; j < pos.Length; ++j) {
            JointConfig jnt_config = (JointConfig)config.jnt_configs[leg[j]];

            // foot joint
            if (j == pos.Length - 1) {
                jnt_config.tar_rot = Quaternion.Inverse(jnt_config.joint.connectedBody.transform.rotation) *
                    Quaternion.LookRotation(Vector3.up, jnt_config.joint.connectedBody.transform.up);
                config.jnt_configs[leg[j]] = jnt_config;
                continue;
            }

            // calculate local axis
            Vector3 x = (pos[j + 1] - pos[j]).normalized;

            Vector3 y = Vector3.Cross(x, config.root.transform.forward).normalized;
            if (Vector3.Dot(y, config.root.transform.up) > 0)
                y = -y;

            Vector3 z = Vector3.Cross(y, x);

            // stance leg's root
            if (!swing && j == 0) {
                jnt_config.tar_rot =
                    Quaternion.Inverse(jnt_config.joint.connectedBody.transform.rotation) *
                    Quaternion.FromToRotation(-Vector3.up, config.root.transform.right) *
                    Quaternion.LookRotation(z, y);
            }

            // other joints
            else {
                jnt_config.tar_rot = Quaternion.Inverse(jnt_config.joint.connectedBody.transform.rotation) *
                    Quaternion.LookRotation(z, y);
            }

            config.jnt_configs[leg[j]] = jnt_config;
        }

    }

    /* IPM model calculation */
    Vector3 CalIPM () {
        Vector3 d = rig_root.velocity;
        Vector3 com = (Vector3)(config.root.transform.localToWorldMatrix * config.COM) + config.root.transform.position;

        float g = Mathf.Abs(Physics.gravity.y);
        if (g == 0) g = 0.1f;

        d *= Mathf.Sqrt(com.y / g + Vector3.SqrMagnitude(d) / (4 * g * g));
        d = Vector3.Magnitude(d) * new Vector3(d.x, 0, d.z).normalized - Configuration.kVAlpha * config.kDV;
        d.y = 0;

        return d;
    }
}
