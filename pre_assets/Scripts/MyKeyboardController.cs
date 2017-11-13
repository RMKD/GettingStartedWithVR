using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyKeyboardController : MonoBehaviour {

	[SerializeField]
	GameObject fadeCube;

	[SerializeField]
	GameObject camera;

	[SerializeField]
	GameObject awesomeSword;

	// [SerializeField]
	// GameObject spawners;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("KeyDown A");
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			Debug.Log ("KeyDown S");
			if (awesomeSword && camera) {
				Vector3 pos = camera.transform.position;
				awesomeSword.transform.position = new Vector3 (pos.x + 0.25f, pos.y - 0.5f, pos.z + 1);
				awesomeSword.transform.parent = camera.transform;
			}
		}

		if (Input.GetKeyUp (KeyCode.S)) {
			Debug.Log ("KeyUp S");
		}

		if (Input.GetKey (KeyCode.S)) {
			Debug.Log ("KeyHold S");
			if (awesomeSword) awesomeSword.transform.Rotate (new Vector3 (0, 2f, 0));
		}

		if (Input.GetKeyDown (KeyCode.F)) {
			Debug.Log ("KeyDown F");
			fadeCube.GetComponent < AlphaFader> ().StartFadeOut ();
		}
		if (Input.GetKeyUp (KeyCode.F)) {
			Debug.Log ("KeyUp F");
			fadeCube.GetComponent < AlphaFader> ().StartFadeIn ();
		}

		// if (Input.GetKeyDown (KeyCode.Space)) {
		// 	spawners.SetActive (!spawners.activeSelf);
		// 	Debug.Log ("spawners active? " + spawners.activeSelf);
		// }

	}
}
