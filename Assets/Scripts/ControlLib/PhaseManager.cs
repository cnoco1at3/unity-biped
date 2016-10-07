using UnityEngine;
using System.Collections;

public class PhaseManager {

    public const float kCycle = 0.6f;
    private static float[] thresholds = { 0.1f, 0.4f, 0.6f, 0.9f };
    private static float[] param = { 0.0f, 0.5f, 0.5f, 1.0f };

    public static AnimMode GetCurrentPhase (float time, int id) {
        float rt = Mathf.Repeat(time, kCycle) / kCycle;
        if (rt > thresholds[2 * id] && rt < thresholds[2 * id + 1])
            return AnimMode.kSwing;
        else
            return AnimMode.kStance;
    }

    public static float Interpolate (float time, int id) {
        float rt = Mathf.Repeat(time, kCycle) / kCycle;
        float t = (rt - thresholds[2 * id]) / (thresholds[2 * id + 1] - thresholds[2 * id]);
        return 1 - t; // 2 * Mathf.Abs(0.5f - t);
    }

    public static float InterpolateHeight (float time, int id) {
        float t = Interpolate(time, id);

        // bezier interpolation
        return Mathf.Pow(1 - t, 3) * param[0] + 3 * Mathf.Pow(1 - t, 2) * t * param[1] + 3 * (1 - t) * t * t * param[2] + Mathf.Pow(t, 3) * param[3];
    }
}
