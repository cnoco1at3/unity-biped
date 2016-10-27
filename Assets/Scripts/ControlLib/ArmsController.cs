using UnityEngine;
using System.Collections;

public class ArmsController : CharaController {
    private static int ctrl_count = 0;
    private int ctrl_id;
    public ArmsController(CharaConfiguration chara, Configuration config, GameObject[] objs, bool debug = false)
        : base(chara, config, objs, debug) {
        ctrl_id = ctrl_count++;
    }

    public override void GenerateJointPositionTrajectory() {
        base.GenerateJointPositionTrajectory();
    }

    public override void GenerateJointRotation() {
        base.GenerateJointRotation();
    }
}
