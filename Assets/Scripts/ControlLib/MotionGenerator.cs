using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionGenerator {

    private CharaConfiguration _chara; 
    private Configuration _config; // configuration

    private bool _debug; // show debug info

    /* constructor */
    public MotionGenerator (CharaConfiguration chara, Configuration config, bool debug = false) {
        _chara = chara;
        _config = config;
        _debug = debug;
    }

    public void GenerateTargetPose () {
        GenerateTargetTrajectory();
        GenerateTargetRotation();
    }

    public void ApplyTargetPose () {
        foreach(CharaController chara_ctrl in _chara.controllers) {
            chara_ctrl.ApplyJointRotation();
        }
    }

    /* generate target rotation of joints */
    private void GenerateTargetRotation () {
        foreach(CharaController chara_ctrl in _chara.controllers) {
            chara_ctrl.GenerateJointRotation();
        }
    }

    private void GenerateTargetTrajectory () {
        foreach(CharaController chara_ctrl in _chara.controllers) {
            chara_ctrl.GenerateJointPositionTrajectory();
        }
    }
}