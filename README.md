# MyFirstVR

### The scenario:
Today we're building a 'slicer' app. If you've ever played Fruit Ninja, you've got some idea where this is going. It will teach you some basic skills that will give you a quick start into building VR applications in Unity.

### Stuff to Download

1) Download Unity 2017.1.1f

- (Win/Mac) https://store.unity.com/download?ref=personal

- (Linux users should use 2017.1.3b1 or higher from http://beta.unity3d.com/download/2ca68d182788/public_download.html)


2) Once you've downloaded Unity, make sure to run the installer and include the desktop systems you want to build for and IOS or Android if you want to do mobile VR development. This will take a long time.  

3) From the Unity Asset Store, download these free plugins:

- SteamVR 1.2.2+ https://www.assetstore.unity3d.com/en/#!/content/32647 ( or git https://github.com/ValveSoftware/steamvr_unity_plugin)

- VRTK 3.2.1+ https://www.assetstore.unity3d.com/en/#!/content/64131 (or git https://github.com/thestonefox/VRTK/releases/tag/3.2.1)


4) Optionally, download any supporting SDKs for hardware you own and want to develop on:

- for Mobile: get GoogleVR 1.7 https://github.com/googlevr/gvr-unity-sdk/releases/download/1.70.0/GoogleVRForUnity_1.70.0.unitypackage and make sure your Android SDK or XCode are up to date

- for Oculus: get Oculus Utilities 1.18.1 https://developer.oculus.com/downloads/package/oculus-utilities-for-unity-5/ and Oculus Avatar SDK https://developer.oculus.com/downloads/package/oculus-avatar-sdk/

- for Vive: install Steam and SteamVR desktop app https://support.steampowered.com/kb_article.php?ref=2001-UXCM-4439#install-steam

5) Optionally, download any additional editing tools you might want to have on hand:  

Blender (useful for mesh editing and 3d file conversions)
https://www.blender.org/download/

Audacity (useful for audio editing)
http://www.audacityteam.org/download/

Krita (useful for image editing)
https://krita.org/en/download/krita-desktop/


#### If You are Building With Vive
[Steam]

[SteamVR]

[SteamVR Unity Plugin]

#### If You Are Building With Oculus
[Oculus Setup]

[Oculus Utilities for Unity](https://developer.oculus.com/downloads/package/oculus-utilities-for-unity-5/)

[Oculus Avatar SDK (for controllers)]

#### If You Are Building With A Mobile Device




## Session I: [Setting Up and Using Unity 3D for VR](./docs/SetupAndBasics.md)
* The User Interface
* Some Non-standard Settings to Enable
* Importing Packages
* Special Folders
* Move, Rotate, Scale
* Transforms and Components
* Building Executables

## Session II: [Controlling Objects With Scripts](./docs/Scripting.md)
* Adding a Script to a GameObject
* Start() and Update()
* Other Important Monobehavior Functions
* Scripts in Hierarchies
* Triggering actions from keyboard input

## Session III: [Collisions and Physics](./docs/CollisionsAndPhysics.md)
* getting to know Colliders and Rigid
* using the OnCollisionEnter function
* managing what can collide with Tags
* gravity, kinematics, and friction

## Session IV: [Attaching User Controls](./docs/Controllers.md)
* overview of controller sdks
* grabbing a simple object
* grabbing a sword
* setting up two-handed grab

## Session V: [Enhancing With Textures, Shaders and Sound](./docs/TexturesShadersAndSound.md)
* add a background scene
* change textures in a photo editor
* use a shader instead of a static texture
* add sound effects


-------------------


## Try Out Some Stuff


## Optimize Your Editor for development

* switch to text-only files
* turn on color to signal you are in play mode
* select your favorite text editor (or you might want to use Mono)
* set paths to androidSDK, Java,
	- SDK should look like: `<your-path>/Android/Sdk` in Linux, `` in MacOS, `` in Windows
	- JDK should look like: `/usr/lib/jvm/default` in Linux, `` in MacOS, `` in Windows
	- NDK should look like: `` in Linux, `` in MacOS, `` in Windows



## Setting Up for Your Head Mounted Display (HMD)

HMDs come in a variety of packages and more options are coming out every month. A great place to start is with VRTK - its available on github and as a free asset in the Unity store https://github.com/thestonefox/VRTK/releases

If you have a HMD on hand, it will abstract away a number of low-level features that are peculiar to different hardware and let you think about and interact with them in a high level way. If you don't have a headset, the latest versions include a `VRSimulatorCameraRig` prefab which will let you develop features on any machine that can run Unity.





### Sources:
[Flickr equirectangular Group](https://www.flickr.com/groups/equirectangular/pool/)

HDRI Images (many CC-BY)
[Adaptive Samples](http://adaptivesamples.com/category/hdr-panos/)

https://hdrihaven.com/hdris/free_hdris/misty_pines_2k.hdr

## Characters

### Rigged Sintel File
http://www.aiai.ed.ac.uk/~ai/unity/resources/sintel/
http://www.aiai.ed.ac.uk/~ai/unity/resources/sintel/SintelScene.unitypackage



### Inverse Kinematics
https://docs.unity3d.com/Manual/InverseKinematics.html

Go to the asset store and add `raw mocap data`. Now you'll have access to


https://www.blendswap.com/blends/view/54246
```
Sintel made by Angela Guenette (Character modeling),

rigged by Nathan Vegdahl (Rigging-Animation)

converted for XNALara by http://xnaaral.deviantart.com/

http://xnaaral.deviantart.com/art/Sintel-Pre-Version-208299244
http://creativecommons.org/licenses/by/3.0/
```


Cutting script
https://github.com/BLINDED-AM-ME/UnityAssets.git

## BladeController

```CSharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BladeController : MonoBehaviour {

	public Material replacementMaterial;
	public AudioClip cutSound;

	private bool bladeEnabled = true;
	private GameObject[] pieces;

	void OnCollisionEnter (Collision col) {

		Debug.Log (string.Format("OnCollisionEnter: {0} hits {1}", gameObject.name, col.gameObject.name));

		if(bladeEnabled && col.gameObject.tag == "Sliceable") {
			string targetName = col.gameObject.name;
			AudioSource.PlayClipAtPoint(cutSound, gameObject.transform.position);

			bladeEnabled = false;
			StartCoroutine ("EnableSplitting", 0.5f);

			pieces = BLINDED_AM_ME.MeshCut.Cut(col.gameObject, transform.position, transform.right, replacementMaterial);

			for (int i = 0; i < pieces.Length; i++) {Debug.Log (string.Format("{0} of {1} pieces: {2}", i, pieces.Length, pieces[i].GetComponent<Rigidbody>()));

				GameObject piece = pieces [i];

				// Remove existing colliders if they exist
				Destroy (piece.GetComponent<BoxCollider> ());
				Destroy (piece.GetComponent<CapsuleCollider> ());
				Destroy (piece.GetComponent<SphereCollider> ());
				Destroy (piece.GetComponent<MeshCollider> ());

				// Add a MeshCollider to match the exact shape
				piece.AddComponent<MeshCollider> ().convex = true;

				// Ensure the new piece has a Rigidbody
				if (!piece.GetComponent<Rigidbody> ()) {
					piece.AddComponent<Rigidbody>();
				}

				// Name the pieces using a numbered suffix notation
				piece.name = string.Format ("{0}_{1}",  targetName, i);

				// Tag the piece as 'Sliceable'
				piece.tag = "Sliceable";
			}
		}
	}

	void OnCollisionExit(Collision col){
		Debug.Log (string.Format ("Collision exit"));
	}

	IEnumerator EnableSplitting(float afterNumSeconds){
		yield return new WaitForSeconds (afterNumSeconds);
		bladeEnabled = true;
	}
}

```

## Find Some Sounds

http://soundbible.com/1520-Decapitated.html
Title: Decapitated
About: Sound of someone being decapitate with a large blade. sound requested by ben carter.
Uploaded: 07.22.10 | License: Attribution 3.0 | Recorded by Mike Koenig
http://soundbible.com/336-Pin-Or-Ting-Metal-Plate.html
Title: Pin Or Ting Metal Plate
About: Sharp ping or metal clank
Uploaded: 05.19.09 | License: Attribution 3.0 | Recorded by Mike Koenig

```
```

## Find Some Assets
[Trees by  Krzysztof Czerwinski](https://trzyde.blogspot.co.uk/p/freebies_5.html)


## Edit VR in VR

EditorVR.unitypackage
https://github-production-release-asset-2e65be.s3.amazonaws.com/76560415/2de6ab5c-49fc-11e7-9252-bbe058547c09?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIWNJYAX4CSVEH53A%2F20170806%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20170806T213959Z&X-Amz-Expires=300&X-Amz-Signature=fa7b70ee840fb52243ad07c49863118223d542c9ceb611241ebd6ebfcf518090&X-Amz-SignedHeaders=host&actor_id=334410&response-content-disposition=attachment%3B%20filename%3DEditorVR.unitypackage&response-content-type=application%2Foctet-stream

ovr_unity_utilities_1.16.0-beta.zip
https://securecdn.oculus.com/binaries/download/?id=1577981995606501&access_token=OC%7C1196467420370658%7C

Go to Edit > Project Settings > Player and enable `Virtual Reality Supported` (if you are using Oculus along with other platforms, Oculus must be first in the list)
