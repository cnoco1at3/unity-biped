using UnityEngine;
using System.Collections;

public static class Extension {

    // Map angle from (0, 2 * PI) to (-PI, PI)
	public static Vector3 Shift2Pi (this Vector3 deg) {
        for(int i = 0; i < 3; ++i) {
            deg[i] = deg[i] > 180.0f ? deg[i] - 360.0f : deg[i];
        }
        return deg;
    }
}
