using UnityEngine;
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
    private Vector3 _desired_position = Vector3.zero;

    public void SetDesiredPosition(Vector3 dP) {
        _desired_position = dP;
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
        AdjustGroundOffset();
        DesiredPositionController();
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

    void OnDrawGizmos() {
        if (debug && _config != null && _config.gizcolor != null) {
            for (int i = 0; i < _config.gizcolor.Count; ++i) {
                Gizmos.color = _config.gizcolor[i];
                Gizmos.DrawWireCube(_config.gizmos[i], _config.scale_factor * (new Vector3(0.1f, 0.1f, 0.1f)));
            }
        }
    }

    private void DesiredPositionController() {
        Vector3 error = _chara.root.transform.position - _desired_position;
        error.y = 0;
        _config.kDV = error * 0.5f;
    }

    private void AdjustGroundOffset() {
        const int layer_mask = ~(0xF << 8);
        bool cast_flag = false;
        Vector3 root_oplane_position = root.transform.position;
        RaycastHit hit = new RaycastHit();
        cast_flag = Physics.Raycast(root_oplane_position, -Vector3.up, out hit, 10.0f, layer_mask);
        if(cast_flag)
            _config.ground_offset = hit.point.y;
    }
}
