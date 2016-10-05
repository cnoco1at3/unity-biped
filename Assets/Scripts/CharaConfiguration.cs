using UnityEngine;
using System.Collections;

public class CharaConfiguration{
    
    public Rigidbody root;

    private CharaController[] _controllers;

    public CharaConfiguration () {

    }

    public Vector3 GetCenterOfMass () {
        return root.transform.position;
    }
}
