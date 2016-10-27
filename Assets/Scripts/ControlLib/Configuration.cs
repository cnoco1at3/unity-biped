using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*!
 * Configuration contains all the parameters and the system's current 
 * condition. Multiple configurations are allowed.
 */

public class Configuration {

    /* global parameters */

    public float scale_factor = 1.0f;

    public Vector3 kDV; // desired velocity
    public float kDH = 0.65f; // desired height
    // PD control
    public float kP = 200f, // P for PD controller
        kD; // D for PD controller
    // velocity tuning
    public float kV = 1f,
        kVAlpha = 0.05f,
        kLiftH = 0.33f; // foot lift height

    public List<Vector3> gizmos;
    public List<Color> gizcolor;

    public Configuration (float scale_factor) {
        this.scale_factor = scale_factor;
        kDV = new Vector3(0.0f, 0.0f, 2.0f) * scale_factor;
        kDH *= scale_factor;
        kLiftH *= scale_factor;
        // kP *= Mathf. (scale_factor);
        kD = 2 * Mathf.Sqrt(kP);
        gizmos = new List<Vector3>();
        gizcolor = new List<Color>();
    }
}
