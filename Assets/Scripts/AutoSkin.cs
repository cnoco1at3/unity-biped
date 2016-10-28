using UnityEngine;
using System.Collections;

public class AutoSkin : MonoBehaviour {
	[SerializeField] Transform newRoot;

	void Start() {
		BindMesh ();
	}

	void BindMesh() {
		Transform[] o_bones = GetComponentInChildren<SkinnedMeshRenderer>().bones;
		Transform[] bones = newRoot.GetComponentsInChildren<Transform>();
		Transform[] newBones = new Transform[o_bones.Length];
		for (int i = 0; i < o_bones.Length; i++)
		{
			Transform bone = o_bones[i];
			foreach(Transform newBone in bones)
			{
				if(bone.name == newBone.name)
				{
					newBone.position = bone.position;
					newBones[i] = newBone;
				}
			}
		}
		GetComponentInChildren<SkinnedMeshRenderer>().bones = newBones;
	}

}