using UnityEngine;

public enum AnimMode {
    kSwing,
    kStance,
    kStatic
};

public class CharaController{

    private CharaConfiguration _chara;
    private Configuration _config;

    // Generate a target trajectory position in world coordinate
    public virtual void GenerateJointPositionTrajectory () {

    }

    // Generate the target joint rotation in local coordinate
    public virtual void GenerateJointRotation () {

    }

    public virtual AnimMode GetCurrentMode () {
        return AnimMode.kStatic;
    }
}
