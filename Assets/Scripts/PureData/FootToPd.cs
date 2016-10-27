using UnityEngine;
using System.Collections;

using LibPDBinding;

public class FootToPd : MonoBehaviour {
	Rigidbody rb;
	private float lastVelocity;
	private float thisVelocity;
	private bool bounceMoment;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		lastVelocity = 0;
		bounceMoment = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 v3Velocity = rb.velocity; 
		thisVelocity = v3Velocity.y;

		float footY = gameObject.transform.position.y;


		if (footY < 0.05 && bounceMoment == false) {
			LibPD.SendFloat ("footBounce", 1);
			bounceMoment = true;
		}

		if (footY > 0.05) {
			bounceMoment = false;
		}




		LibPD.SendFloat("footPosY", footY);
		print ("sending footPosY: " + footY);


	}
}



