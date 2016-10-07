using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*!
 * Configuration contains all the parameters and the system's current 
 * condition. Multiple configurations are allowed.
 */

public class Configuration {

    /* global parameters */

    public Vector3 kDV; // desired velocity
    public float kDH = 0.62f; // desired height
    // PD control
    public float kP = 200f, // P for PD controller
        kD; // D for PD controller
    // velocity tuning
    public float kV = 0.05f,
        kVAlpha = 0.05f,
        kLiftH = 0.20f; // foot lift height

    public List<Vector3> gizmos;
    public List<Color> gizcolor;

    public Configuration () {
        kDV = Vector3.zero;
        kD = 2 * Mathf.Sqrt(kP);
        gizmos = new List<Vector3>();
        gizcolor = new List<Color>();
    }
}
