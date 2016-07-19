using UnityEngine;
using System.Collections;

public class SphereForce : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        Vector3 dir = other.transform.position - transform.position;
        dir = 1000.0f * dir.normalized;
        other.GetComponent<Rigidbody>().AddForce(dir);
    }
}
