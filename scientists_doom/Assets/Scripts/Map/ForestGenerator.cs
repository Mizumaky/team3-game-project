using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour {
	public List<GameObject> treePrefabList;

	public float firstRingRadius;
	public float distanceBetweenRows;
	public float maxTreeScale, minTreeScale;
	public ushort maxRingsCount;
	public ushort treeCountFirstRing;
	public short treeCountNextRingIncrement;
	[Range(0, 180f)]
	public float ringAngleOffsetLimit;
	public float treeScaleModPerRow = 0.03f;
	public float treeScaleMod = 0;

	public GameObject forest;

	private void Start() {
		if(forest == null) {
			GenerateForest();
		}
	}

	// Generates forest from tree rings (starting from the outter ring)
	public void GenerateForest() {
		treeScaleMod = 0;
		if(forest != null) {
			DestroyImmediate(forest.gameObject);
		}

		forest = new GameObject();
		forest.name = "Forest";
		forest.transform.parent = transform;

		for (int i = 0; i < maxRingsCount; i++) {
			GenerateTreeRing(treeCountFirstRing + (i * treeCountNextRingIncrement), firstRingRadius + (i * distanceBetweenRows));
		}
	}

	// Spawns trees in a ring
	private void GenerateTreeRing(int treeCount, float ringRadius) {
		float accurateAngle = 360f / treeCount;
		
		float ringAngleOffset;
		float offsetAngle;
		for (int i = 0; i < treeCount; i++) {
			// randomize ring rotation
			ringAngleOffset = Random.Range(-ringAngleOffsetLimit, ringAngleOffsetLimit);
			offsetAngle = (accurateAngle * i + ringAngleOffset) % 360;

			// create a ray to find tree y pos
			float rayPosX = transform.position.x + ringRadius * Mathf.Sin(offsetAngle * Mathf.Deg2Rad);
			float rayPosZ = transform.position.z + ringRadius * Mathf.Cos(offsetAngle * Mathf.Deg2Rad);
			float rayLength = 100f;
			Vector3 rayOriginPosition = new Vector3(rayPosX, 10f, rayPosZ);
			Ray rayDown = new Ray(rayOriginPosition, Vector3.down);

			// spawn tree on the intersection of the ray and terrain
			RaycastHit hit;
			if(Physics.Raycast(rayDown, out hit, rayLength)) {
				if(hit.collider.gameObject.layer == 9) {
					SpawnTree(hit.point);
				}
			}
		}
		treeScaleMod += treeScaleModPerRow;
		Debug.Log(treeScaleMod);
	}

	private void SpawnTree(Vector3 pos) {
		float scaleFactor = Random.Range(minTreeScale, maxTreeScale) + treeScaleMod; //scale tree by n and scale from ring radius
		//pos.y *= n; //adjust position relative to scale   

		int treeIndex = Random.Range(0, 3); //random tree prefab selection

		GameObject tree = Instantiate(treePrefabList[treeIndex], pos, treePrefabList[treeIndex].transform.rotation, forest.transform);	
		tree.transform.localScale = tree.transform.localScale * scaleFactor;
	}
}
