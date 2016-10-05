using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlEngine : MonoBehaviour {
    public GameObject root;
    public GameObject[] leg_l, leg_r;
    public GameObject[] arm_l, arm_r;
    public GameObject[] body;

    private Configuration _config;
    private MotionGenerator _motion_generator;
    private Vector3 _giz_dir;

    void Start () {
        _config = new Configuration(root, leg_l, leg_r, arm_l, arm_r, body);
        _motion_generator = new MotionGenerator(_config);

        _config.root.gameObject.AddComponent<RigInteractor>();

        // initialize motor constraint
        foreach (DictionaryEntry jnt_entry in _config.jnt_configs) {
            JointConfig jnt_config = (JointConfig)jnt_entry.Value;
            ConfigurableJoint jnt = (ConfigurableJoint)jnt_config.joint;
            JointDrive jntdrv = new JointDrive();

            jnt.gameObject.AddComponent<RigInteractor>();

            jntdrv.positionSpring = Configuration.kP *
                jnt.gameObject.GetComponent<Rigidbody>().mass *
                (jnt.GetComponentsInChildren<Rigidbody>().Length);
            jntdrv.positionDamper = Configuration.kD *
                Mathf.Sqrt(jntdrv.positionSpring);
            jntdrv.maximumForce = Mathf.Infinity;

            jnt.angularXDrive = jntdrv;
            jnt.angularYZDrive = jntdrv;
        }
    }

    /* wasd control */
    void Update () {
        if (Input.GetKey(KeyCode.W)) {
            _config.kDV = _config.kVOff + new Vector3(0.0f, 0.0f, 0.7f);
            _giz_dir = new Vector3(0f, 0f, 1f);
        }
        else if (Input.GetKey(KeyCode.A)) {
            _config.kDV = _config.kVOff + new Vector3(-0.7f, 0.0f, 0.0f);
            _giz_dir = new Vector3(-1f, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.S)) {
            _config.kDV = _config.kVOff + new Vector3(0.0f, 0.0f, -0.3f);
            _giz_dir = new Vector3(0f, 0f, -1f);
        }
        else if (Input.GetKey(KeyCode.D)) {
            _config.kDV = _config.kVOff + new Vector3(0.7f, 0.0f, 0.0f);
            _giz_dir = new Vector3(1f, 0f, 0f);
        }
        else {
            _config.kDV = _config.kVOff;
            _giz_dir = new Vector3(0f, 0f, 0f);
        }
    }

    void FixedUpdate () {
        _motion_generator.GeneratePose();

        // set motor constraint
        foreach (DictionaryEntry jnt_entry in _config.jnt_configs) {
            JointConfig jnt_config = (JointConfig)jnt_entry.Value;
            Quaternion target = MathHelper.LocalToJoint(
                (ConfigurableJoint)jnt_config.joint,
                jnt_config.init_rot,
                jnt_config.tar_rot);
            ((ConfigurableJoint)jnt_config.joint).targetRotation = target;
        }
    }

    void OnDrawGizmos () {

        Gizmos.color = Color.yellow;
        // this is throwing up nullreference exceptions
        foreach (Vector3 pos in _motion_generator.helper_gizmos) {
            Gizmos.DrawWireCube(pos, new Vector3(0.1f, 0.1f, 0.1f));
        }

        Gizmos.color = Color.blue;
        foreach (Vector3 pos in _motion_generator.target_gizmos) {
            Gizmos.DrawWireCube(pos, new Vector3(0.1f, 0.1f, 0.1f));
        }

        /*
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_giz_dir, new Vector3(0.3f, 0.3f, 0.3f));
        */
    }
}
