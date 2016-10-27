﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlEngine : MonoBehaviour {
    public GameObject root;
    public GameObject[] leg_l, leg_r;
    public GameObject[] arm_l, arm_r;
    public GameObject[] body;
    public bool debug;

    private Configuration _config;
    private CharaConfiguration _chara;
    private MotionGenerator _motion_generator;

    public void SetDesiredVelocity(Vector3 dV) {
        _config.kDV = dV;
    }

    void Start() {
        // Configuration
        _config = new Configuration(gameObject.transform.localScale.x);
        // Character setup
        List<GameObject[]> body_list = new List<GameObject[]>();
        body_list.Add(arm_l);
        body_list.Add(arm_r);
        body_list.Add(body);
        List<GameObject[]> limbs_list = new List<GameObject[]>();
        limbs_list.Add(leg_l);
        limbs_list.Add(leg_r);
        _chara = new CharaConfiguration(_config, root.GetComponent<Rigidbody>(), body_list, limbs_list, debug);
        _motion_generator = new MotionGenerator(_chara, _config, debug);

        /*
        if (debug)
            root.GetComponent<Rigidbody>().isKinematic = true;
            */
    }

    void Update() {
        /* wasd control */
        //if (debug)
        //KeyboardInteraction();
        int layer_mask = 0xF << 8;
        layer_mask = ~layer_mask;
        Vector3 root_oplane_position = root.transform.position;
        root_oplane_position.y = 0;
        Ray ray = new Ray(root_oplane_position, -Vector3.up);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 10.0f, layer_mask);
        _config.ground_offset = hit.point.y;
    }

    void FixedUpdate() {
        if (debug) {
            _config.gizmos.Clear();
            _config.gizcolor.Clear();
        }
        _motion_generator.GenerateTargetPose();
        _motion_generator.ApplyTargetPose();

    }

    void KeyboardInteraction() {
        if (Input.GetKey(KeyCode.W)) {
            _config.kDV = new Vector3(0.0f, 0.0f, 0.7f);
        } else if (Input.GetKey(KeyCode.A)) {
            _config.kDV = new Vector3(-0.7f, 0.0f, 0.0f);
        } else if (Input.GetKey(KeyCode.S)) {
            _config.kDV = new Vector3(0.0f, 0.0f, -0.3f);
        } else if (Input.GetKey(KeyCode.D)) {
            _config.kDV = new Vector3(0.7f, 0.0f, 0.0f);
        } else {
            _config.kDV = Vector3.zero;
        }
    }

    void OnDrawGizmos () {
        if (debug) {
            for (int i = 0; i < _config.gizcolor.Count; ++i) {
                Gizmos.color = _config.gizcolor[i];
                Gizmos.DrawWireCube(_config.gizmos[i], _config.scale_factor * (new Vector3(0.1f, 0.1f, 0.1f)));
            }
        }
    }
}
