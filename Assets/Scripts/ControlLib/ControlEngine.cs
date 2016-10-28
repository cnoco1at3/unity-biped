using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlEngine : MonoBehaviour {
    public GameObject root;
    public GameObject[] leg_l, leg_r;
    public GameObject[] arm_l, arm_r;
    public GameObject[] body;
    public bool debug;
    public bool run;

    private Configuration _config;
    private CharaConfiguration _chara;
    private MotionGenerator _motion_generator;
    private Vector3 _desired_direction;
    private float _desired_speed_factor = 3f;
    private bool _flick = false;
    private float _flick_time = 0.0f;

    public void SetDesiredPosition(Vector3 dd) {
        _desired_direction = dd;
        if (!run) {
            Vector3 error = dd - _chara.root.transform.position;
            error.y = 0;
            Vector3 look = Quaternion.LookRotation(error, Vector3.up).eulerAngles;
            Vector3 origin = _chara.root.transform.rotation.eulerAngles;
            origin.y = look.y;
            _chara.root.transform.rotation = Quaternion.Euler(origin);
        }
    }

    public void SetSpeedFactor(float sf) {
        _desired_speed_factor = sf;
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

        _desired_direction = Vector3.zero;

        /*
        if (debug)
            root.GetComponent<Rigidbody>().isKinematic = true;
            */
    }

    void Update() {
        /* wasd control */
        if (debug)
            KeyboardInteraction();
        if (run) {
            AdjustGroundOffset();
            DesiredPositionController();
        }

        if (Input.GetKey(KeyCode.Mouse0)) {
            _flick = true;
            _flick_time = Time.time;
        }
    }

    void FixedUpdate() {
        if (run) {
            if (debug) {
                _config.gizmos.Clear();
                _config.gizcolor.Clear();
            }
            _motion_generator.GenerateTargetPose();
            _motion_generator.ApplyTargetPose();

            if (_flick) {
                Vector3 force_dir = _desired_direction;
                force_dir.y = 0f;
                force_dir.Normalize();
                _chara.root.AddForce(force_dir * 1e2f * (0.5f + Time.time - _flick_time));
                if (_flick_time - Time.time > 0.5f)
                    _flick = false;
            }
        }

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
        Vector3 error = _chara.root.transform.position - _desired_direction;
        error.y = 0;
        _config.kDV = error * _desired_speed_factor;
        //_chara.root.velocity = _config.kDV;
    }

    private void AdjustGroundOffset() {
        const int layer_mask = ~(0xF << 8);
        bool cast_flag = false;
        Vector3 root_oplane_position = root.transform.position;
        RaycastHit hit = new RaycastHit();
        cast_flag = Physics.Raycast(root_oplane_position, -Vector3.up, out hit, 10.0f, layer_mask);
        if (cast_flag)
            _config.ground_offset = hit.point.y;
    }
}