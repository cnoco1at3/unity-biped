using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
    public GameObject root;
    public GameObject[] leg_l, leg_r;
    public GameObject[] arm_l, arm_r;
    public GameObject[] body;

    private Configuration config;
    private MotionGenerator mogen;
    private Vector3 giz_dir;

    void Start () {
        config = new Configuration(root, leg_l, leg_r, arm_l, arm_r, body);
        mogen = new MotionGenerator(config);

        // initialize motor constraint
        foreach (DictionaryEntry jnt_entry in config.jnt_configs) {
            JointConfig jnt_config = (JointConfig)jnt_entry.Value;
            ConfigurableJoint jnt = (ConfigurableJoint)jnt_config.joint;
            JointDrive jntdrv = new JointDrive();

            jntdrv.positionSpring = Configuration.k_p *
                jnt.gameObject.GetComponent<Rigidbody>().mass *
                (jnt.GetComponentsInChildren<Rigidbody>().Length);
            jntdrv.positionDamper = Configuration.k_d *
                Mathf.Sqrt(jntdrv.positionSpring);
            jntdrv.maximumForce = Mathf.Infinity;

            jnt.angularXDrive = jntdrv;
            jnt.angularYZDrive = jntdrv;
        }
    }

    /* wasd control */
    void Update () {
        if (Input.GetKey(KeyCode.W)) {
            config.k_dv = config.k_voff + new Vector3(0.0f, 0.0f, 0.7f);
            giz_dir = new Vector3(0f, 0f, 1f);
        }
        else if (Input.GetKey(KeyCode.A)) {
            config.k_dv = config.k_voff + new Vector3(-0.7f, 0.0f, 0.0f);
            giz_dir = new Vector3(-1f, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.S)) {
            config.k_dv = config.k_voff + new Vector3(0.0f, 0.0f, -0.3f);
            giz_dir = new Vector3(0f, 0f, -1f);
        }
        else if (Input.GetKey(KeyCode.D)) {
            config.k_dv = config.k_voff + new Vector3(0.7f, 0.0f, 0.0f);
            giz_dir = new Vector3(1f, 0f, 0f);
        }
        else {
            config.k_dv = config.k_voff;
            giz_dir = new Vector3(0f, 0f, 0f);
        }
    }

    void FixedUpdate () {
        mogen.GeneratePose();

        // set motor constraint
        foreach (DictionaryEntry jnt_entry in config.jnt_configs) {
            JointConfig jnt_config = (JointConfig)jnt_entry.Value;
            Quaternion target = LocalToJoint(
                (ConfigurableJoint)jnt_config.joint,
                jnt_config.init_rot,
                jnt_config.tar_rot);
            ((ConfigurableJoint)jnt_config.joint).targetRotation = target;
        }
    }

    // covert local space rotation to joint space
    Quaternion LocalToJoint (ConfigurableJoint jnt,
        Quaternion init,
        Quaternion tar) {
        Vector3 right = jnt.axis;
        Vector3 forward = Vector3.Cross(jnt.axis, jnt.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Quaternion w2j = Quaternion.LookRotation(forward, up);

        Quaternion res = Quaternion.Inverse(w2j);
        res *= Quaternion.Inverse(tar) * init;
        res *= w2j;
        return res;
    }

    // convert world space rotation to joint space
    Quaternion WorldToJoint (ConfigurableJoint jnt,
        Quaternion init,
        Quaternion tar) {
        Vector3 right = jnt.axis;
        Vector3 forward = Vector3.Cross(jnt.axis, jnt.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Quaternion w2j = Quaternion.LookRotation(forward, up);

        Quaternion res = Quaternion.Inverse(w2j);
        res *= init * Quaternion.Inverse(tar);
        res *= w2j;
        return res;
    }

    void OnDrawGizmos () {

        Gizmos.color = Color.yellow;
        foreach (Vector3 pos in mogen.Helper) {
            Gizmos.DrawWireCube(pos, new Vector3(0.1f, 0.1f, 0.1f));
        }

        Gizmos.color = Color.blue;
        foreach (Vector3 pos in mogen.Target) {
            Gizmos.DrawWireCube(pos, new Vector3(0.1f, 0.1f, 0.1f));
        }

        Gizmos.color = Color.green;
        Gizmos.DrawCube(giz_dir, new Vector3(0.3f, 0.3f, 0.3f));
    }
}
