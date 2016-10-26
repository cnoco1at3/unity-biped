using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaConfiguration{

    public Rigidbody root { get; private set; }
    public List<CharaController> controllers;

    private Configuration _config;
    private List<GameObject[]> _body_list;
    private List<GameObject[]> _limbs_list;

    private bool _debug;

    public CharaConfiguration (Configuration config, Rigidbody body_root, 
        List<GameObject[]> body_list, 
        List<GameObject[]> limbs_list, 
        bool debug = false) {

        _config = config;
        root = body_root;
        _body_list = body_list;
        _limbs_list = limbs_list;
        _debug = debug;

        controllers = new List<CharaController>();

        foreach (GameObject[] limbs in _limbs_list) {
            LimbsController limbs_ctrl = new LimbsController(this, _config, limbs, _debug);
            controllers.Add(limbs_ctrl);
        }

        foreach(GameObject[] body in body_list) {
            CharaController body_ctrl = new CharaController(this, _config, body, _debug);
            controllers.Add(body_ctrl);
        }
    }

    public Vector3 GetCenterOfMass () {
        Vector3 sum = root.transform.localToWorldMatrix.MultiplyPoint(root.centerOfMass);
        int count = 1;
        foreach (CharaController controller in controllers) {
            foreach(Rigidbody rig in controller._rigs) {
                sum += rig.transform.localToWorldMatrix.MultiplyPoint(rig.centerOfMass);
                count++;
            }
        }
        return (1.0f / count) * sum;
    }
}
