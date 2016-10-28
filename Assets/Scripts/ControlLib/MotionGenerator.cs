using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionGenerator {

    private CharaConfiguration _chara;
    private Configuration _config; // configuration
    private ConfigurableJoint _root_joint;

    private bool _debug; // show debug info

    /* constructor */
    public MotionGenerator(CharaConfiguration chara, Configuration config, bool debug = false) {
        _chara = chara;
        _config = config;
        _debug = debug;
        _root_joint = _chara.root.GetComponent<ConfigurableJoint>();
    }

    public void GenerateTargetPose() {
        GenerateTargetTrajectory();
        GenerateTargetRotation();
        TuneRootDirection();
    }

    public void ApplyTargetPose() {
        foreach (CharaController chara_ctrl in _chara.controllers) {
            chara_ctrl.ApplyJointRotation();
        }
    }

    /* generate target rotation of joints */
    private void GenerateTargetRotation() {
        foreach (CharaController chara_ctrl in _chara.controllers) {
            chara_ctrl.GenerateJointRotation();
        }
    }

    private void GenerateTargetTrajectory() {
        foreach (CharaController chara_ctrl in _chara.controllers) {
            chara_ctrl.GenerateJointPositionTrajectory();
        }
    }

    private void TuneRootDirection() {
        /*
        const float kHardLimit = 3.0f;
        Quaternion to = Quaternion.LookRotation(_config.kDV.normalized, Vector3.up);
        Vector3 from = _root_joint.targetRotation.eulerAngles;
        Vector3 error = to.eulerAngles - from;
        if (error.magnitude > kHardLimit) {
            float regularizer = kHardLimit / error.magnitude;
            to.x = to.x * regularizer;
            to.y = to.y * regularizer;
            to.z = to.z * regularizer;
        }
        _root_joint.targetRotation = to;
        */
    }
}