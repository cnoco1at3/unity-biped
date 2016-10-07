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
    public Vector3 kVOff; // velocity offset
    public float kDH = 0.62f; // desired height
    // PD control
    public float kP = 40000f, // P for PD controller
        kD = 400f; // D for PD controller
    // velocity tuning
    public float kV = 0.05f,
        kVAlpha = 0.05f,
        kLiftH = 0.12f; // foot lift height

    public Configuration () {
        kVOff = new Vector3(0.0f, 0.0f, -1f);
        kDV = kVOff;
    }
}
