## Session V: Enhancing With Textures, Shaders and Sound
* adding materials and textures to objects
* add a background scene
* change textures in a photo editor
* use a shader instead of a static texture
* add sound effects

### Adding Materials and Textures to objects

Materials pretty much determine how your scene will appear. You can think of Materials as a bucket of visual effects that determine how an object should be rendered for the cameras. They use a *shader* to render *maps*. By adding or changing the maps, you change how the object appears.

- in the Project view, create a new Material and name it something like MyFirstMaterial
- click on it to view it in the Inspector
- in the Inspector, observe that its Shader is Standard and its rendering mode is opaque
- click on the color selector in *albedo* and change it to something more interesting than white
- apply the texture to an object by dragging it from the Project window to the object in the Hierarchy view
- once you get used to it, assign a material to the floor and your sword

Textures are basically images like `jpg` or `png` but with special metadata attached to them that tells Unity how to treat them in a scene.

- copy an image from your computer into the Assets directory
- in the Inspector, examine the various options under Texture type (but leave it as default)
- click on the Material you created in the Project view
- in the Inspector, click on the circle next to the *albedo* and select your image from the window

You can think of materials as a way to manage the appearance of one or more objects.

- try assigning the material to more than one object, then select the Material from the Project view and change its color - observe how all of the objects with that material change color

You can find more details specifications in the [Unity Materials and Textures documentation](https://docs.unity3d.com/Manual/Shaders.html)

### Customize the Floor

Now that you've got the basic idea, we'll add a more realistic texture to the ground. Professionally made textures will include not just a simple image but several image types that handle different types of image processing. The classical way of doing this included a 'normal', 'specular', and 'diffuse' layer. Unity's current method gives you 'albedo', 'metallic', 'normal map', 'height map', and 'occlusion'.

Create a new material by going to the menu: `Assets > Create > Material` (or right click in the Project window) and name it `FloorMaterial`. In the Inspector, you should see that it's using Shader: Standard (we'll cover these more shortly). Now you've got a basic material that we can attach more information to.

Find `WoodPlanks028_2k` (there are even higher quality versions up to 6k) in the pre_assets directory drag it into your `Assets/Textures` directory. This contains images in the format they are provided by [Poliigon](https://www.poliigon.com/) - with a suffix describing different layers. By dragging from the Project window to the Inspector, you can assign different images to different maps of the material. If you click the circle next to the word, it will pull up a menu you can use to search everything in your assets directory.

- assign Albedo to the `COL2` image
- set the Smoothness to 0.40 and change the source to `Albedo Alpha`
- assign Normal to the `NRM` image and click the `Fix Now` button to make sure the image gets processed as a 'normal'
- assign Heightmap to the `DISP` image (not `DISP16`)
- assign Occlusion to the `AO` image
- to adjust how big the wooden planks appear, change the Tiles values for X and Y (5 and 5 seems to work well)


#### Customize the Pillar

We can customize the pillar in a similar way, but this artist has provided a more complex texture. Find `pillar_greekMat.mat`, move it to your `Assets/Materials` directory and examine it in the Inspector. You'll see that it has Albedo, Specular, Normal Map, and a Height Map assigned. If you look at each of those images in the inspector individually, you'll see that they have a series of regions. Those regions correspond to different parts of the mesh and are mapped there during rendering. If you open it up in [Krita](https://krita.org) or another image editing program, you could add graffiti or change its texture. For now, drag that material to the pillar to attach it.

#### Add a Background

Now let's add some background scenery. A common way to provide distant  scenery is to use a 'skybox'. As StreetView, photosphere apps, and 360/180 cameras become more popular, there are a lot of resources available for backgrounds. We're going to use a static image captured in HDR: `misty_pines_2k`.

* Move the image to your `./Assets` directory.
* Select it and in the Inspector set the Texture Shape to Cube and the Mapping to Latitude-Longitude layout (you should see the preview look like its projected onto the inside of a half-sphere)
* Create a new material (Menu: Assets > Create > Material) and give it a name describing the image
* Click on that material and set its shader to Skybox > Cubemap
* Click the Cubemap (HDR) select button and choose your material in the menu (or simply drag and drop the image from your project tab) . You can verify that it projected correctly by dragging the mouse around in the preview - if you need to adjust how it aligns with your scene, use the rotation slider in the Inspector.
* To use as a skybox, open the `Window > Lighting > Settings` menu and under the scene tab, drag your cubemap material onto the skybox material slot. You should see it appear in the background of your Scene View

If you need to adjust how close or how high the photosphere appears, you can tweak those settings using one of the first two the options described in [this project](https://github.com/omgwtfgames/unity-cardboard-photosphere) which provides a great summary of the three main ways to set up backgrounds in Unity. While 'inverted normals' might sound a little complicated as you're getting started, it basically means setting up the sphere to draw the texture on the *inside* rather than the *outside*, so if you make it big enough to fit your whole scene, you can drop a standard photosphere (i.e. an equirectangular image) like you might see in Google's StreetView or could take with a smartphone app.


### Shaders
Shaders are specialized scripts that determine how textures will be applied across a mesh.

One of the best introductions to shaders is http://www.alanzucconi.com/2015/06/10/a-gentle-introduction-to-shaders-in-unity3d/

If we go back to our fadeCube, that can help make sense of how shader interact with scripts and give you some basic constructs that will help when you're ready for fancier stuff.

#### Making a basic Shader

In the pre_asets directory, go into Shaders and Copy the SwordShadersProgresssion into your assets folder along with `lava.png` and `Displacements.png` from the textures folder. Each of these shaders adds a little more functionality to the previous one. In order to apply a shader to an object, it first needs a material to work with, so right click in your assets and create a new material. By dragging the shader onto the material, and then dragging the material onto the object, we can see what our shader is doing.

The first shader, 01SolidColorShader is about as basic as a shader can be. For every pixel where the object is located, it always returns the same color.

```
Shader "Custom/SolidColor" {
	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex vert             
			#pragma fragment frag

			struct vertInput {
				float4 pos : POSITION;
			};  

			struct vertOutput {
				float4 pos : SV_POSITION;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
				return o;
			}

			half4 frag(vertOutput output) : COLOR {
				return half4(1.0, 0.0, 0.0, 1.0);
			}
			ENDCG
		}
	}
}
```

With the next shader, 02TextureShader, we can apply a texture to the material using what is called UV maps. (The U and V are the names of the axis. They are simply the X and Y coordinates of the texture we are using.). You'll notice at the top of this shader, we have a Properties section.  This allows us to give the shader inputs from unity. `_MainTex ("Texture", 2D) = "white" {}` means that the variable in our code will be \_MainTex, the name shown in the unity inspector is "Texture", which takes in a 2D image, and if no image is supplied, it will use a white image by default. Note that we have to redeclare \_MainTex as a sampler2D before we can use it. We can drag `lava.png` into the Texture slot of our material to apply the texture to it. So now, we are mapping the object's 3d point to a UV coordinate which we then use to sample the texture that we give it.


While we normally think of textures as Images, to the computer, it's simply a grid of rgba values, which are represented as numbers from 0-1. So instead of passing it a picture, we can pass it a grid of values that we won't use as colors. This Displacement picture that we are using has random noise in both the red and green channels. We can use values in these channels to warp our texture. So instead of sampling the texuture with our normal UV values, we add a displacement value that we get from our Displacement picture. Since the values are always between 0 and 1, we multiply it by 2 and subtract 1 so we get values from -1 to 1, so our displacement will move in all directions. When we apply this shader to our material and drag `Displacements.png` to the Warp Texture slot, we can move the Magnitude slider to see how the texure changes as we increase the effect of the warp.

While the warp is cool, it' static once you choose a Magnitude value. The last shader, 04FlowingShader, utilizes the built in `_Time` variable to add animation to our shader. So instead of reading from our Displacement texture the same UV value each time, we add `_Time.x*_Speed` to it so we get a flowing type effect. \_Time has an x, y, and w, but they are all just the time multiplied by a constant. In order they are t/20, t, t\*2, t\*3. There are other constants like `_SinTime` which can give you oscilating values. Information about these and other constants can be viewed here:  https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html


#### Using a Shader to Show the Sword is Powered Up

In the pre_asets directory, in the materials folder you'll find `PowerUpMaterial.mat`, and in the textures folder you'll find `lava.png` and `Displacements.png`, and in the shader's folder named SwordShaderProgression. Bring them into your Assets directory and create another copy of your sword model in the hierarchy and name it `MagmaSword`. Check to make sure the PowerUpMaterial is using the `Custom/04FlowingShader` and is assigned `lava` as the texture and `Displacement` as the warp texture. If you've got everything connected you,  should see an animated shader effect in the Scene window. If you click on the PowerUpMaterial, you can also play around with the Magnitude and Speed sliders to see how it affects the texture. It might be easier to see on a plain cube, so you can create a new 3D cube in your scene and drag the material onto it.

To connect this with our two-handed sword powerup effect:

- select the MagmaSword object in the Hierarchy and uncheck the box next to it in the Inspector (this will disable the object and make it disappear from view)
- open up the `BladeController.cs` script and find your `EnablePowerUp` and `DisablePowerUp` functions
- at the top of the script, create SerializeField variables for `simpleModel` and `powerModel` game objects
- select the Sword parent object in the Hierarchy and drag the SimpleSword and MagmaSword children to their slots in the `BladeController` compenent in the Inspector
- back in the script, under enable, add the following lines: `simpleModel.SetActive (false);`  `powerModel.SetActive (true);`
- under disable, add the following lines: `simpleModel.SetActive (false);`  `powerModel.SetActive (true);`

### Sounds

Though sometimes sounds are a last thought, they can really add to a sense of immersion in a scene. Unity gives you a lot of power for creating effects by playing the audio at a particular place in the scene. It's volume will depend on how far you are from it in the scene.

Much like images you can load sounds as assets simply by moving supported files such as `mp3` or `wav` to the Assets directory. Once there, you can attach sounds to objects by adding an `Audio Source` component (or you can reference them directly with a script as an `AudioClip` you invoke at a certain time). Sounds that are played using `PlayClipAtPoint` will play from a position in the scene and can be a very effective technique for drawing a users attention to a particular place in the scene.

Open up your BladeController script and add the following variable declaration:

```
[SerializeField]
AudioClip cutSound;
```

Then drag the cut sound audio clip to your script in the Inspector.

In the `OnCollisionEnter` function, play the sound.

```
AudioSource.PlayClipAtPoint (cutSound, gameObject.transform.position);
```

- add a sound to differentiate between hitting a `Sliceable` and a `Strong` object
- add a sound that always plays while the magmaSword is enabled (hint: use an Audio Component, not just a clip)
- add some ambient sound like a bird singing to the scene by creating an empty object and attaching the sound
- can you write a script that will play the bird sound at random intervals from a random place in the scene?


### Particles, Lines, and Trails

As a bonus, if you're looking for some additional effects to add to your sword, Unity has a powerful engine for adding particles and trailing lines to objects. To add a Trail to your sword swing, right-click the sword parent in the Hierarchy and select `Effects > Trail` (or Particle, or Line). Then you'll get a child object with the relevant renderer attached. You can change its origin just like everything else in the scene by moving its Transform.

There's lots of fun options you explore within the effects menus. These often come with a performance cost but used carefully can provide powerful enhancements to your experience.

### One Last Challenge

If you made it all the way through - congratulations! If you want one more challenge, see if you can write a script that will throw new sliceable objects towards the pillar. This process of generating objects is often referred to as a 'spawner'. You can use spatial audio to signal to the user that an object is about to fly towards them.

As a hint, you'll probably want some function that chooses which prefab to use and then makes sure that each prefab has a Rigidbody and has been tagged as `Sliceable`

```
GameObject sliceable = (GameObject)Instantiate (ChoosePrefab (), transform.position, Quaternion.identity);
sliceable.tag = "Sliceable";

Vector3 heading = target.transform.position - transform.position;

Rigidbody rb = sliceable.GetComponent<Rigidbody> ();

if (!rb) rb = sliceable.AddComponent<Rigidbody> ();

rb.velocity = transform.TransformDirection (heading/heading.magnitude * power);
rb.useGravity = true;
rb.drag = 1f;
```
