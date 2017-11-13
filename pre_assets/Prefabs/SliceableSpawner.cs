using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceableSpawner : MonoBehaviour {

	[SerializeField]
	GameObject [] prefabsToSlice;

	[SerializeField]
	GameObject target; // object to aim at

	[SerializeField]
	AudioClip launchSound;

	public float spawnRate = 0.01f;
	public float power = 10f;
	public float objectTimeToLive = 10f;

	void Start(){
		// if a prefab list hasn't been provided, spawn cubes
		if (prefabsToSlice.Length < 1) {
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.localScale = new Vector3 (.3f, .3f, .3f);
			prefabsToSlice = new GameObject[] {cube};
		}

		// tell the BladeController to destroy objects after slicing
		GameObject.FindObjectOfType<BladeController> ().destroyObjectsAfterSlicing = true;
	}

	void FixedUpdate () {
		if (Random.value < spawnRate) {
			SpawnObject ();
		};
	}

	GameObject ChoosePrefab(){
		return prefabsToSlice [Random.Range (0, prefabsToSlice.Length)];
	}

	void SpawnObject(){

		AudioSource.PlayClipAtPoint (launchSound, transform.position);

		GameObject sliceable = (GameObject)Instantiate (ChoosePrefab (), transform.position, Quaternion.identity);
		sliceable.tag = "Sliceable";

		Vector3 heading = target.transform.position - transform.position;

		Rigidbody rb = sliceable.GetComponent<Rigidbody> ();

		if (!rb) rb = sliceable.AddComponent<Rigidbody> ();

		rb.velocity = transform.TransformDirection (heading/heading.magnitude * power);
		rb.useGravity = true;
		rb.drag = 1f;

		StartCoroutine (DestroyObjectAfter(sliceable, objectTimeToLive));
	}

	IEnumerator DestroyObjectAfter(GameObject go, float numSeconds){
		yield return new WaitForSeconds (numSeconds);
		if(go) Destroy(go);
	}
}
