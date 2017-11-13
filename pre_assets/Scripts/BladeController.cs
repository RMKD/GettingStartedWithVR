using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class BladeController : MonoBehaviour {

	[SerializeField]
	Material replacementMaterial;
	[SerializeField]
	AudioClip cutSound;
	[SerializeField]
	AudioClip cannotCutSound;

	[SerializeField]
	GameObject simpleModel;
	[SerializeField]
	GameObject powerModel;

	public bool destroyObjectsAfterSlicing;

	private bool bladeEnabled = true;
	private bool isPoweredUp = false;
	private GameObject[] pieces; 
	private Rigidbody rb;
	private string parentTag;

	void Start(){
		rb = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter (Collision col) {

		Debug.Log (string.Format("OnCollisionEnter: {0} hits {1}", gameObject.name, col.gameObject.name));

		if (col.gameObject.tag == "Sliceable" || (isPoweredUp && col.gameObject.tag == "Strong")) {
			if (!bladeEnabled)
				return;

			parentTag = col.gameObject.tag;

			string targetName = col.gameObject.name;
			AudioSource.PlayClipAtPoint (cutSound, gameObject.transform.position);

			// disable the blade to prevent cascading collisions
			bladeEnabled = false;

			// use the imported MeshCut function to split the object (note: this requries objects have valid UV, so may not work for all objects)
			pieces = BLINDED_AM_ME.MeshCut.Cut (col.gameObject, transform.position, transform.right, replacementMaterial);

			for (int i = 0; i < pieces.Length; i++) {
				//Debug.Log (string.Format ("{0} of {1} pieces: {2}", i, pieces.Length, pieces [i].GetComponent<Rigidbody> ()));

				GameObject piece = pieces [i];

				//Make sure pieces are not sliceable while the collision is taking place
				piece.tag = "Untagged";

				// Remove existing colliders if they exist
				Destroy (piece.GetComponent<BoxCollider> ());
				Destroy (piece.GetComponent<CapsuleCollider> ());
				Destroy (piece.GetComponent<SphereCollider> ());
				Destroy (piece.GetComponent<MeshCollider> ());

				// Ensure the new piece has a Rigidbody
				if (!piece.GetComponent<Rigidbody> ()) {
					piece.AddComponent<Rigidbody> ();
				}

				// If the sliced piece is relatively simple, add a convex MeshCollider to match the exact shape of the new piece
				if (piece.GetComponent<MeshFilter>().mesh.vertexCount < 64) {
					piece.AddComponent<MeshCollider> ().convex = true;
				} else {
					// Otherwise, just wrap the weird shape in a BoxCollider
					piece.AddComponent<BoxCollider> ();
				}

				if(destroyObjectsAfterSlicing) StartCoroutine(DestroyObjectAfter (piece, 3f));
			}

		} else if (col.gameObject.tag == "Strong") {
			// for objects tagged 'Strong' play a sound idicating that they cannot be cut
			AudioSource.PlayClipAtPoint (cannotCutSound, gameObject.transform.position);
		} else {
			// ignore all other objects
			Debug.Log (col.gameObject.name + " no effect " + col.gameObject.tag);
		}

	}

	void OnCollisionExit(Collision col){
		Debug.Log (string.Format ("Collision exit"));
		for (int i = 0; i < pieces.Length; i++) {
			GameObject piece = pieces [i];
			// make sure the object hasn't already been destroyed
			if (piece) {
				piece.tag = parentTag;
			}
		}
		bladeEnabled = true;
	}

	public void EnablePowerUp(){
		Debug.Log("BladeController>EnablePowerUp");
		simpleModel.SetActive (false);
		powerModel.SetActive (true);
		isPoweredUp = true;
	}

	public void DisablePowerUp(){
		Debug.Log("BladeController>DisablePowerUp");
		powerModel.SetActive (false);
		simpleModel.SetActive (true);
		isPoweredUp = false;
	}

	IEnumerator DestroyObjectAfter(GameObject obj, float numSeconds){
		yield return new WaitForSeconds (numSeconds);
		if(obj) Destroy(obj);
	}
		

	void SlowPhysics(GameObject piece){
		Rigidbody r = piece.GetComponent<Rigidbody> ();
		StartCoroutine (RestorePhysics (r));
		r.AddForce (Vector3.up);
		r.mass = 0.001f;
		r.drag = 20f;
	}

	IEnumerator RestorePhysics(Rigidbody r){
		float originalMass = r.mass;
		float originalDrag = r.drag;
		yield return new WaitForSeconds (1f);
		r.mass = originalMass;
		r.drag = originalDrag;
	}
}

