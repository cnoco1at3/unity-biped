using UnityEngine;

public enum AnimMode {
    kSwing,
    kStance,
    kStatic
};

public class CharaController{
    protected CharaConfiguration _chara;
    protected Configuration _config;

    protected GameObject[] _objs;
    protected ConfigurableJoint[] _joints;
    protected Rigidbody[] _rigs;
    protected Quaternion[] _init_rot;
    protected Quaternion[] _target_rot;
    protected Vector3[] _target_pos;

    protected bool _debug;

    public CharaController (CharaConfiguration chara, Configuration config, GameObject[] objs, bool debug = false) {
        _objs = objs;
        _joints = new ConfigurableJoint[_objs.Length];
        _rigs = new Rigidbody[_objs.Length];
        _init_rot = new Quaternion[_objs.Length];
        _target_rot = new Quaternion[_objs.Length];
        _target_pos = new Vector3[_objs.Length];

        for (int i = 0; i < _objs.Length; ++i) {
            _rigs[i] = _objs[i].GetComponent<Rigidbody>();
            _init_rot[i] = _objs[i].transform.rotation;
            _target_rot[i] = _init_rot[i];

            _joints[i] = _objs[i].GetComponent<ConfigurableJoint>();
        }

        if (_debug)
            Debug.Log("CharaController Constructor");

        InitializeJoints();
    }

    // Generate a target trajectory position in world coordinate
    public virtual void GenerateJointPositionTrajectory () {
    }

    // Generate the target joint rotation in local coordinate
    public virtual void GenerateJointRotation () {
    }

    public virtual void ApplyJointRotation () {
        for (int i = 0; i < _objs.Length; ++i) {
            Quaternion target_joint_rot = MathHelper.LocalToJoint(
                _joints[i],
                _init_rot[i],
                _target_rot[i]
                );
            _joints[i].targetRotation = target_joint_rot;
        }
    }

    public virtual AnimMode GetCurrentMode () {
        return AnimMode.kStatic;
    }

    protected virtual void InitializeJoints () {
        for (int i = 0; i < _objs.Length; ++i) {
            JointDrive drive = new JointDrive();
            drive.positionSpring = _config.kP * _rigs[i].mass * _objs[i].GetComponentsInChildren<Rigidbody>().Length;
            drive.positionDamper = 2 * Mathf.Sqrt(drive.positionSpring);
            drive.maximumForce = Mathf.Infinity;

            _joints[i].angularXDrive = drive;
            _joints[i].angularYZDrive = drive;
        }
        foreach (GameObject obj in _objs) {
            obj.AddComponent<RigInteractor>();
        }
    }
}
