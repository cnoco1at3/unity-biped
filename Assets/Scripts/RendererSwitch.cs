using UnityEngine;
using System.Collections;

public class RendererSwitch : MonoBehaviour {
	public bool rendererOn;
	// Use this for initialization
	void Start () {
		foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
			rend.enabled = rendererOn;
	}

}
