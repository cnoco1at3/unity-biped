using UnityEngine;
using System.Collections;

public class TestAppGoTo : MonoBehaviour {

    public GameObject aim_point;
    public GameObject chara;
    private ControlEngine _control_engine;
    private GameObject _chara_root;
    private bool _init_flag = false;
    private Vector3 _prev_root_position;

    private const float kP = 0.04f;
    private const float kD = 0.04f;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (_init_flag) {
            UpdateDesiredVelocity();
        } else {
            Initialize();
        }
    }

    void UpdateDesiredVelocity() {
        Vector3 position_error = aim_point.transform.position - _chara_root.transform.position;
        Vector3 velocity_error = (_prev_root_position - _chara_root.transform.position) / Time.deltaTime;
        // _control_engine.SetDesiredVelocity(kP * position_error );
    }

    void Initialize() {
        _control_engine = chara.GetComponent<ControlEngine>();
        _chara_root = _control_engine.root;
        _prev_root_position = _chara_root.transform.position;
        _init_flag = _control_engine != null ? true : false;
    }
}
