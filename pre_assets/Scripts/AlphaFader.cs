using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFader : MonoBehaviour {

	[SerializeField]
	float duration = 3f;

	private float elapsed = 3;
	private float fadeDirection = -1; // positive for fadeOut, negative for fade in
	private float fadePercent;
	private Material material;

	void Start () {
		// get the main material for the object
		material = gameObject.GetComponent<Renderer> ().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		// calculate what percent should be faded
		fadePercent = Mathf.Lerp (0, 1f, elapsed/duration);

		// pass the new alpha value to the shader
		material.SetFloat("_alpha", fadePercent);

		// count down for 
		elapsed += (Time.deltaTime * fadeDirection);

	}

	public void StartFadeOut(){
		fadeDirection = 1f;
	}

	public void StartFadeIn(){
		fadeDirection = -1f;
	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination){

		if (fadePercent < 0 && fadePercent < 1) {
			Graphics.Blit (source, destination, material);
		} else {
			Graphics.Blit (source, destination);
		}
	}
}
