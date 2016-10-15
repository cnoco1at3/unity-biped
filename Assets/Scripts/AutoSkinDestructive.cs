// 4.x example
using UnityEngine;
using System.Collections;

public class AutoSkinDestructive : MonoBehaviour {
	Transform[] bones;
	Mesh mesh;

	void Start() {
		SkinnedMeshRenderer rend = GetComponentInChildren<SkinnedMeshRenderer>();

		mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		bones = rend.rootBone.GetComponentsInChildren<Transform>();
		mesh.boneWeights = new BoneWeight[mesh.vertices.Length];

		for (int i = 0; i < mesh.vertices.Length; i++) {
			AssignBone (i);
		}

		Matrix4x4[] bindPoses = new Matrix4x4[bones.Length];

		for (int i = 0; i < bindPoses.Length; i++) {
			bindPoses [i] = bones [i].worldToLocalMatrix * transform.localToWorldMatrix;
		}

		mesh.bindposes = bindPoses;

		// Assign bones and bind poses
		rend.bones = bones;
		rend.sharedMesh = mesh;
	}

	void AssignBone (int vertIndex)
	{
		print (bones[1]);
		int closestIndex = 0;
		for (int i = 1; i < bones.Length; i++) {
			if (Vector3.Distance(mesh.vertices[vertIndex], bones[i].position) < 
				Vector3.Distance(mesh.vertices[vertIndex], bones[closestIndex].position))
				closestIndex = i;
		}
		mesh.boneWeights[vertIndex].boneIndex0 = 1;
		mesh.boneWeights[vertIndex].boneIndex0 = 1;
	}
}