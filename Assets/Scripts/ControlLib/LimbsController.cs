using UnityEngine;

public class LimbsController : CharaController {

    // IK parameters
    private Vector3 _ik_target;

    // Constructor
    public LimbsController (CharaConfiguration chara, Configuration config, GameObject[] objs, bool debug = false) 
        : base(chara, config, objs, debug) {

    }

    public override void GenerateJointPositionTrajectory () {

        // Assign IK target for swing or stance foot
        if (GetCurrentMode() == AnimMode.kSwing) {
            _ik_target = _objs[0].transform.position + IPMError();
            _ik_target.y = _config.kLiftH * PhaseManager.InterpolateHeight(Time.time);
        }
        else {
            _ik_target = _objs[0].transform.position;
            _ik_target.y = 0;
        }

        // 1st pass IK solving
        for (int i = 0; i < _objs.Length; ++i)
            _target_pos[i] = _objs[i].transform.position;
        FABRIKSolver.SolveIKWithVectorConstraint(ref _target_pos, _ik_target, _chara.root.transform.forward);

        // 2nd pass IK solving
        _ik_target = _objs[0].transform.position;
        _ik_target.y = _config.kDH;
        _ik_target += _config.kDV * Time.fixedDeltaTime;
        FABRIKSolver.SolveIKWithVectorConstraint(ref _target_pos, _ik_target, _chara.root.transform.forward, true);
    }

    public override void GenerateJointRotation () {

        for(int i = 0; i < _objs.Length; ++i) {

            // Foot joint
            if(i == _objs.Length - 1) {
                _target_rot[i] = Quaternion.Inverse(_joints[i].connectedBody.transform.rotation);
                continue;
            }

            // Calculate local axis
            Vector3 x = (_target_pos[i + 1] - _target_pos[i]).normalized;
            Vector3 y = Vector3.Cross(x, _chara.root.transform.forward).normalized;
            if (Vector3.Dot(y, _chara.root.transform.up) > 0)
                y = -y;
            Vector3 z = Vector3.Cross(y, x);

            if (GetCurrentMode() != AnimMode.kSwing && i == 0) {
                _target_rot[i] =
                    Quaternion.Inverse(_joints[i].connectedBody.transform.rotation) *
                    Quaternion.FromToRotation(-Vector3.up, _chara.root.transform.right) *
                    Quaternion.LookRotation(z, y);
            }

            // other joints
            else {
                _target_rot[i] = Quaternion.Inverse(_joints[i].connectedBody.transform.rotation) *
                    Quaternion.LookRotation(z, y);
            }
        }
    }

    public override AnimMode GetCurrentMode () {
        return AnimMode.kStance;
    }

    private Vector3 IPMError () {
        Vector3 d = _chara.root.velocity;
        Vector3 com = (Vector3)(_chara.root.transform.localToWorldMatrix * _chara.GetCenterOfMass()) + _chara.root.transform.position;

        float g = Mathf.Abs(Physics.gravity.y);
        if (g == 0) g = 0.1f;

        d *= Mathf.Sqrt(com.y / g + Vector3.SqrMagnitude(d) / (4 * g * g));
        d = Vector3.Magnitude(d) * new Vector3(d.x, 0, d.z).normalized - _config.kVAlpha * _config.kDV;
        d.y = 0;

        return d;
    }
}
