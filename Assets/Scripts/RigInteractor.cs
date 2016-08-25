using UnityEngine;
using System.Collections;

public class RigInteractor : MonoBehaviour {

    // hand's layer index 
    private const int kHandLayer = 11;
    // force strength
    private const float kFStrength = 100f;

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.layer == kHandLayer) {
            float dis = (transform.position - other.transform.position).magnitude;
            Vector3 force = Mathf.Clamp(kFStrength / dis, 0, 300) * (transform.position - other.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(force);

            Debug.Log(GetComponent<Rigidbody>().ToString());
            Debug.Log(force);
        }
    }
}
