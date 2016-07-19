using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*!
 * Configuration contains all the parameters and the system's current 
 * condition. Multiple configurations are allowed.
 */

/* configuration of a joint */
struct JointConfig {
    public Joint joint;

    // target and initial joint rotation in local space
    public Quaternion init_rot;
    public Quaternion tar_rot;
}

public class Configuration {

    /* parameters */

    public Vector3 k_dv; // desired velocity
    public Vector3 k_voff; // velocity offset
    public float k_dh = 0.63f; // desired height
    // PD control
    public const float k_p = 300f, // P for PD controller
        k_d = 2.0f; // D for PD controller
    // velocity tuning
    public const float k_v = 0.05f,
        k_v_alpha = 0.05f,
        k_lift_h = 0.12f; // foot lift height

    /* joints */
    public Hashtable jnt_configs;
    public GameObject root; // root joint
    public GameObject[] leg_l, leg_r; // legs
    public GameObject[] arm_l, arm_r; // arms
    public GameObject[] body; // body

    /* attributes */
    // joints list length
    public int Length { get; private set; }
    // center of mass position in local space
    public Vector3 COM { get; private set; }

    public Configuration (GameObject i_root,
        GameObject[] i_leg_l,
        GameObject[] i_leg_r,
        GameObject[] i_arm_l,
        GameObject[] i_arm_r,
        GameObject[] i_body) {

        // initialize parameters

        k_voff = new Vector3(0.0f, 0.0f, -1.3f);
        k_dv = k_voff;
        Length = 0;

        // initialize joints lists
        jnt_configs = new Hashtable();
        root = i_root;

        leg_l = i_leg_l;
        AddJointConfig(leg_l);

        leg_r = i_leg_r;
        AddJointConfig(leg_r);

        arm_l = i_arm_l;
        AddJointConfig(arm_l);

        arm_r = i_arm_r;
        AddJointConfig(arm_r);

        body = i_body;
        AddJointConfig(body);

        Length = jnt_configs.Count;

        // calculate center of mass
        COM = root.transform.localToWorldMatrix *
            root.GetComponent<Rigidbody>().centerOfMass;

        foreach (DictionaryEntry jnt_entry in jnt_configs) {
            JointConfig jnt_config = (JointConfig)jnt_entry.Value;
            COM = (Vector3)(jnt_config.joint.transform.localToWorldMatrix *
                jnt_config.joint.GetComponent<Rigidbody>().centerOfMass) +
                COM;
        }

        COM /= Length;
        COM = root.transform.worldToLocalMatrix * COM;
    }

    /* add joints into the joint table */
    void AddJointConfig (GameObject[] list) {
        foreach (GameObject jnt in list) {
            // initialize joint configuration
            JointConfig jnt_config = new JointConfig();
            jnt_config.joint =
                jnt.GetComponent<ConfigurableJoint>();
            jnt_config.init_rot =
                jnt_config.joint.transform.localRotation;
            jnt_config.tar_rot = jnt_config.init_rot;

            // add to joint table 
            jnt_configs.Add(jnt, jnt_config);
        }
    }
}
