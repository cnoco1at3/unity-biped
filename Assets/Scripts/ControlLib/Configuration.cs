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
    public float kDH = 0.65f; // desired height
    // PD control
    public float kP = 400f, // P for PD controller
        kD; // D for PD controller
    // velocity tuning
    public float kV = 0.05f,
        kVAlpha = 0.05f,
        kLiftH = 0.32f; // foot lift height

    public List<Vector3> gizmos;
    public List<Color> gizcolor;

    public Configuration () {
        kDV = new Vector3(0.0f, 0.0f, 10.0f);
        kD = 2 * Mathf.Sqrt(kP);
        gizmos = new List<Vector3>();
        gizcolor = new List<Color>();
    }
}
