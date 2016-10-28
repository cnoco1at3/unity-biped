using UnityEngine;
using System.Collections;

public class TestAppGoTo : MonoBehaviour {

    private ControlEngine _control_engine;

    // Use this for initialization
    void Start() {
        _control_engine = FindObjectOfType<ControlEngine>().GetComponent<ControlEngine>();
    }

    // Update is called once per frame
    void Update() {
        _control_engine.SetDesiredDirection(transform.position);
    }

}
