using UnityEngine;
using System.Collections;

public class RigManager : MonoBehaviour {
	Vector3[] defaultBonesPos;
	Transform[] curBones;
	[SerializeField] GameObject curCharacter;
	[SerializeField] Camera worldCam;
	[SerializeField] Camera rigCam1;
	[SerializeField] Camera rigCam2;
	[SerializeField] Camera rigCam3;
	[SerializeField] Transform centerViewJoints;
	[SerializeField] Transform rightViewJoints;
	[SerializeField] Transform leftViewJoints;
	bool isRigging;

	void Start()
	{
		DontDestroyOnLoad (this);
		Screen.orientation = ScreenOrientation.Portrait;
		foreach (Camera cam in Camera.allCameras)
			cam.enabled = false;
		worldCam.enabled = true;
		StartRigMode (curCharacter);
	}

	void PrepareForRig() {
		curBones = curCharacter.GetComponentInChildren<ControlEngine>().root.GetComponentsInChildren<Transform>();
		curCharacter.GetComponentInChildren<ControlEngine>().enabled = false;
		defaultBonesPos = new Vector3[curBones.Length];
		for (int i = 0; i < curBones.Length; i++) {
			defaultBonesPos [i] = curBones [i].position;
		}

		Transform[] jointEditorGroups = {centerViewJoints, rightViewJoints, leftViewJoints};
		//bind joints to joint editors
		foreach (Transform jointEditorGroup in jointEditorGroups) {
			foreach (Transform jointEditorT in jointEditorGroup) {
				JointEditor jointEditor = jointEditorT.GetComponent<JointEditor> ();
				string jointName = jointEditor.name.Remove (jointEditor.name.Length - 9);
				jointEditor.joint = FindJoint (jointName);
			}
		}
	}

	Transform FindJoint(string name)
	{
		foreach (Transform joint in curBones) {
			if (joint.name == name)
				return joint;
		}
		Debug.LogError ("No joint with name \"" + name + "\" exists.");
		return null;
	}

	void BindMesh() {
		SkinnedMeshRenderer rend = curCharacter.GetComponentInChildren<SkinnedMeshRenderer> ();

		Mesh mesh = (Mesh)Instantiate (curCharacter.GetComponentInChildren<SkinnedMeshRenderer> ().sharedMesh);
		mesh.RecalculateNormals(); //<-- SUPER IMPORTANT

		Matrix4x4[] bindPoses = new Matrix4x4[curBones.Length];

		for (int i = 0; i < curBones.Length; i++) {
			bindPoses [i] = curBones [i].worldToLocalMatrix * curCharacter.transform.localToWorldMatrix;
		}

		mesh.bindposes = bindPoses;

		BoneWeight[] boneWeights = new BoneWeight[mesh.vertexCount];
		for (int i = 0; i < mesh.vertices.Length; i++) {
			int closestIndex = 0;
			for (int j = 0; j < curBones.Length; j++) {
				Vector3 vert = curCharacter.transform.TransformPoint(mesh.vertices [i]);
				if (Vector3.Distance (vert, curBones [j].position) <
					Vector3.Distance (vert, curBones [closestIndex].position))
					closestIndex = j;
			}
			boneWeights [i].boneIndex0 = closestIndex;
			boneWeights [i].weight0 = 1;
		}
		mesh.boneWeights = boneWeights;

		// Assign bones and bind poses
		rend.bones = curBones;
		rend.sharedMesh = mesh;
	}

	public void StartRigMode (GameObject character)
	{
		SetRigVisibility(curCharacter);
		if (!isRigging) {
			isRigging = true;
			SetCamera (rigCam1);
			SetBodyPhysics (false);
			PrepareForRig ();
		} else
			Debug.LogError ("Tried to enter rig mode before finishing rigging.");
	}

	void SetCamera (Camera cam)
	{
		foreach (Camera curCam in Camera.allCameras)
			curCam.enabled = false;
		cam.enabled = true;
		Transform[] jointEditorGroups = {centerViewJoints, rightViewJoints, leftViewJoints};
		//set joint editor visibility depending on camera
		foreach (Transform jointEditorGroup in jointEditorGroups) {
			foreach (Transform jointEditorT in jointEditorGroup) {
				JointEditor jointEditor = jointEditorT.GetComponent<JointEditor> ();
				jointEditor.SwitchCam(cam);
			}
		}
	}

	public void EndRigMode ()
	{
		if (isRigging) {
			BindMesh ();
			curCharacter.GetComponentInChildren<ControlEngine>().enabled = true;
			SetBodyPhysics (true);
			SetCamera (worldCam);
			// move back to default position
			for (int i=0; i < curBones.Length; i++)
				curBones[i].position = defaultBonesPos[i];
			isRigging = false;
		} else
			Debug.LogError ("Tried to end rig mode before starting rigging.");
	}

	public void CancelRigMode ()
	{
		if (isRigging) {
			SetBodyPhysics (true);
			SetCamera (worldCam);
			// move back to default position
			for (int i=0; i < curBones.Length; i++)
				curBones[i].position = defaultBonesPos[i];
			isRigging = false;
		} else
			Debug.LogError ("Tried to cancel rig mode before starting rigging.");
	}

	public void ResetJoints ()
	{
		if (isRigging) {
			// move back to default position
			for (int i = 0; i < curBones.Length; i++)
				curBones[i].position = defaultBonesPos[i];
		} else
			Debug.LogError ("Tried to reset rig before starting rigging.");
	}

	public void SetBodyPhysics (bool isOn) {
		foreach (Rigidbody rb in curCharacter.GetComponentsInChildren<Rigidbody>()) {
			rb.isKinematic = !isOn;
		}
	}

	public void SetRigVisibility(bool isOn) {
		MeshRenderer[] jointMeshes = 
			curCharacter.transform.FindChild("MatchMan").GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer rend in jointMeshes)
			rend.enabled = isOn;
	}
}
