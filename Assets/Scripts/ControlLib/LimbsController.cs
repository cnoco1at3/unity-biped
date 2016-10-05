using UnityEngine;

public class LimbsController : CharaController {

    // Configuration
    private CharaConfiguration _chara;
    private Configuration _config;

    // Limbs info
    private GameObject[] _limbs;
    private ConfigurableJoint[] _joints;
    private Rigidbody[] _rigs;

    // IK parameters
    private Vector3 _ik_target;
    private Vector3[] _ik_position;

    // Debug info
    private bool _debug = false;
    
    // Constructor
    public LimbsController (CharaConfiguration chara,
        Configuration config,
        GameObject[] limbs, bool debug = false) {

        _chara = chara;
        _config = config;
        _limbs = limbs;
        _debug = debug;

        _ik_position = new Vector3[_limbs.Length];
    }

    public override void GenerateJointPositionTrajectory () {

        // Assign IK target for swing or stance foot
        if (GetCurrentMode() == AnimMode.kSwing) {
            _ik_target = _limbs[0].transform.position + IPMError();
            _ik_target.y = Configuration.kLiftH * PhaseManager.InterpolateHeight(Time.time);
        }
        else {
            _ik_target = _limbs[0].transform.position;
            _ik_target.y = 0;
        }

        // 1st pass IK solving
        for (int i = 0; i < _limbs.Length; ++i)
            _ik_position[i] = _limbs[i].transform.position;
        FABRIKSolver.SolveIKWithVectorConstraint(ref _ik_position, _ik_target, _chara.root.transform.forward);

        // 2nd pass IK solving
        _ik_target = _limbs[0].transform.position;
        _ik_target.y = _config.kDH;
        _ik_target += _config.kDV * Time.fixedDeltaTime;
        FABRIKSolver.SolveIKWithVectorConstraint(ref _ik_position, _ik_target, _chara.root.transform.forward, true);
    }

    public override void GenerateJointRotation () {
        for(int i = 0; i < _limbs.Length; ++i) {

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
        d = Vector3.Magnitude(d) * new Vector3(d.x, 0, d.z).normalized - Configuration.kVAlpha * _config.kDV;
        d.y = 0;

        return d;
    }
}
