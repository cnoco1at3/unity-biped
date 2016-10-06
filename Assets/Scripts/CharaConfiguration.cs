using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaConfiguration{

    public Rigidbody root { get; private set; }
    public List<CharaController> controllers;

    private Configuration _config;
    private List<GameObject[]> _body_list;
    private List<GameObject[]> _limbs_list;

    private bool _debug = false;

    public CharaConfiguration (Configuration config, List<GameObject[]> body_list, List<GameObject[]> limbs_list, bool debug = false) {
        _debug = debug;
        _config = config;
        _body_list = body_list;
        _limbs_list = limbs_list;

        controllers = new List<CharaController>();

        foreach (GameObject[] limbs in _limbs_list) {
            LimbsController limbs_ctrl = new LimbsController(this, _config, limbs);
            controllers.Add(limbs_ctrl);
        }

        foreach(GameObject[] body in body_list) {
            CharaController body_ctrl = new CharaController();
            controllers.Add(body_ctrl);
        }
    }

    public Vector3 GetCenterOfMass () {
        return root.transform.position;
    }
}
