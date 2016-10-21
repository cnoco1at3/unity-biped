using UnityEngine;
using System.Collections;

public class AutoSkin : MonoBehaviour {
	[SerializeField] Transform root;

	void Start() {
		BindMesh ();
	}

	void BindMesh() {
		SkinnedMeshRenderer rend = GetComponentInChildren<SkinnedMeshRenderer> ();

		Mesh mesh = (Mesh)Instantiate (GetComponentInChildren<SkinnedMeshRenderer> ().sharedMesh);
		mesh.RecalculateNormals();

		Transform[] bones = root.GetComponentsInChildren<Transform>();
		Matrix4x4[] bindPoses = new Matrix4x4[bones.Length];

		for (int i = 0; i < bones.Length; i++) {
			bindPoses [i] = bones [i].worldToLocalMatrix * transform.localToWorldMatrix;
		}

		mesh.bindposes = bindPoses;

		BoneWeight[] boneWeights = new BoneWeight[mesh.vertexCount];
		for (int i = 0; i < mesh.vertices.Length; i++) {
			int closestIndex = 0;
			for (int j = 0; j < bones.Length; j++) {
				Vector3 vert = transform.TransformPoint(mesh.vertices [i]);
				if (Vector3.Distance (vert, bones [j].position) <
					Vector3.Distance (vert, bones [closestIndex].position))
					closestIndex = j;
			}
			boneWeights [i].boneIndex0 = closestIndex;
			boneWeights [i].weight0 = 1;
		}
		mesh.boneWeights = boneWeights;

		// Assign bones and bind poses
		rend.bones = bones;
		rend.sharedMesh = mesh;
	}

}