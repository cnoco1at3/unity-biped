using UnityEngine;
using System.Collections;

public class PhaseManager {

    public const float k_cycle = 0.6f;
    private static float[] param = { 0.0f, 0.5f, 0.5f, 1.0f };

    public static int GetCurrentPhase(float time) {
        float rt = Mathf.Repeat(time, k_cycle);
        return rt > k_cycle / 2 ? 1 : 0;
    }

    public static float Interpolate(float time) {
        float rt = Mathf.Repeat(time, k_cycle / 2);
        float t = 1 - Mathf.Abs(rt - k_cycle / 4) / (k_cycle / 4);
        return t;
    }

    public static float InterpolateHeight(float time) {
        float t = Interpolate(time);

        // bezier interpolation
        return Mathf.Pow(1 - t, 3) * param[0] + 3 * Mathf.Pow(1 - t, 2) * t * param[1] + 3 * (1 - t) * t * t * param[2] + Mathf.Pow(t, 3) * param[3];
    }
}
