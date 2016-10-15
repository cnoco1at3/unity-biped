using UnityEngine;
using System.Collections;

// this example creates a quad mesh from scratch, creates bones
// and assigns them, and animates the bones motion to make the
// quad animate based on a simple animation curve.
public class AutoSkinDemo : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<Animation>();
        gameObject.AddComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer rend = GetComponent<SkinnedMeshRenderer>();
        Animation anim = GetComponent<Animation>();

        // Build basic mesh
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(-1, 5, 0), new Vector3(1, 5, 0) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
        mesh.RecalculateNormals();
        rend.material = new Material(Shader.Find("Diffuse"));

        // Create Bone Transforms and Bind poses
        // One bone at the bottom and one at the top

		Transform[] bones = new Transform[2];
		Matrix4x4[] bindPoses = new Matrix4x4[2];
		bones[0] = new GameObject("Lower").transform;
		bones[0].parent = rend.transform;
		// Set the position relative to the parent
		bones[0].localRotation = Quaternion.identity;
		bones[0].localPosition = Vector3.zero;
		bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;

		bones[1] = new GameObject("Upper").transform;
		bones[1].parent = rend.transform;
		// Set the position relative to the parent
		bones[1].localRotation = Quaternion.identity;
		bones[1].localPosition = new Vector3(0, 5, 0);
		bindPoses[1] = bones[1].worldToLocalMatrix * transform.localToWorldMatrix;

		mesh.bindposes = bindPoses;

		BoneWeight[] boneWeights = new BoneWeight[mesh.vertexCount];
		for (int i = 0; i < mesh.vertices.Length; i++) {
			int closestIndex = 0;
			for (int j = 0; j < bones.Length; j++) {
				if (Vector3.Distance(mesh.vertices[i], bones[j].position) < 
					Vector3.Distance(mesh.vertices[i], bones[closestIndex].position))
				closestIndex = j;
			}
			boneWeights[i].boneIndex0 = closestIndex;
			boneWeights[i].weight0 = 1;
		}
		mesh.boneWeights = boneWeights;
		//		foreach (BoneWeight boneWeight in mesh.boneWeights)
		//			print (boneWeight.weight0 + " " + boneWeight.boneIndex0);
		//		foreach (Vector3 vert in mesh.vertices)
		//			print (vert);


		// Assign bones and bind poses
		rend.bones = bones;
		rend.sharedMesh = mesh;
	}
}