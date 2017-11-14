## Session II: Controlling Objects With Scripts
* Adding a Script to a GameObject
* Start() and Update()
* Other Important Monobehavior Functions
* Scripts in Hierarchies
* Triggering actions from keyboard input
* Trigger actions from controller input

Almost everything you can do with the graphical editor you can do with scripts in C#. It's not always obvious how to do it, but a combination of reading Unity Documentation and searching in community forums will usually turn up some examples.

### Adding a Script to a GameObject

* Create a Cube in the Hierarchy (from the menu: `GameObject>3D Object>Cube` or right-click in the window)
* If the object is not visible in a scene window, double-click on the object in the Hierarchy and it should be brought into focus
* In the Inspector, click `Add Component` and type `MyTestScript` then click `New Script` and click `Create and Add` (you should now see that script attached to your Cube in the Inspector and appear in your Project window)
* In the Inspector or Project window, double-click the `MyTestScript` file - this should open a text editor (by default it is `Monodevelop` but you can change it in the menu `Edit>Preferences>External Tools`, though you may want to make sure you can get C# code completion in your favorite editor before changing it since it's one of the fastest ways to learn)


### Basic Functions

In C#, functions take the following form:
```
<optional-scope> <return-type> <FunctionName>(<optional-args>){
  // some awesome stuff
  return <return type>; // if the return-type is not void
}
```

In practice, that looks like:
```
public float MultByOnePointSeven(int n){
  return n * 1.7f;
}
```

By default, scripts extend the `Monobehaviour` class which gives you a number of useful functions (`<optional-scope> class <classname> {}` is how classes are declared in C# and `:` is used after the classname to inherit from a parent class). We can build an understanding of those functions by observing their behavior in the Console.

```
// Use this for initialization
void Start(){
  Debug.Log("MyTestScript>Start");
}

// Update is called once per frame
void Update(){
  Debug.Log("MyTestScript>Update");
}
```

Now, switch back to the editor and hit play. If you Console tab is visible, you should see one message from `Start` and hundreds from `Update`. When you are getting a lot of repeat messages, you can collapse them into a single message by toggling `Collapse` on the Console tab.

To get a sense of how `Update` can be used, lets introduce a counter variable. Declare a simple counter variable:

`float myVariable = 0;`

Modify function to increment `myVariable` and the Log statement to display the counter variable (it is possible to use `+` to concatenate string values in C# but better form to use `string.Format`)

```
void Update(){
  myVariable += 1;
  Debug.Log(string.Format("MyTestScript>Update myVariable: {0}", myVariable));
}
```




### Additional Monobehaviour Functions

The complete list of functions available to Monobehaviours is available in the [documentation](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) but here's a few more you may see:

#### Awake
Awake is for things that should happen when a script is first loaded but do not need to be run every time it is enabled. Try adding an Awake function to your test script with its own Debug statement. In the editor, click run, then in the Inspector window, click the checkbox to disable and re-enable your object. Observe when the different functions execute.


#### OnFixedUpdate

Like `Update` but it runs every so many milliseconds rather than every frame. This can be important if you are trying to do realistic physics simulations.


#### OnCollisionEnter / OnCollisionExit

These functions are both used for collision detection. (we'll use this a little later).


#### OnRenderImage

This function is used for post-processing (we'll use this a little later).


### Attributes

One of the features of C# as a language is that you can you '[attributes](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/index)' to provide metadata about your functions using square bracket notation. Unity leverages this to provide paths for users to customize the editor in several different ways. Here are a couple that come in handy and will give you a sense how to interpret this kind of code when you see it. The full list of function attributes is listed in the [Unity Documentation: Attributes](https://docs.unity3d.com/ScriptReference/AddComponentMenu.html)

#### SerializeField

Expose a variable to be changed in the editor.

```
[SerializableField]
float myVariable = 0;

```
Before this syntax, the easiest way to expose variables in the editor was to make them `public`. A lot of us got in a bad habit of doing that, but you should probably make them public **only** if you intend for other scripts to access them. In general, this looks like `myGameObject.GetComponent<SomeScriptName>().FunctionToRun()`

#### RequireComponent

Sometimes when you write a script, you want it to control other Components attached to the GameObject. This attribute will tell the compiler to throw an error if the Component isn't there as expected. This keeps you from writing `if (ThingIsAttached) {} else {}` type statements.

```
[RequireComponent(typeof(Rigidbody))]
public class DoSomethingAwesome(){
  // awesome stuff with rigid body
}
```

#### MenuItem

When you've got an action with multiple steps that you're doing over and over, write a script and stick it in a menu!

```
[UnityEditor.MenuItem ("MenuSection/Dropdown Title")]
```


### Trigger Actions from Keyboard input

The standard method of setting up user controls is to use the InputManager found via `Edit > Project Settings > Input`, however if you want to set up some controls just for testing, writing yourself some keyboard shortcuts is a great way to do it.

#### Simple Logging

To trigger actions from the keyboard, create an Empty GameObject and name it `KeyboardManager`. Then create a new script called MyKeyboardController and attach it to the KeyboardManager. In the `Update` function, add a simple check and a debug statement.

```
if (Input.GetKeyDown (KeyCode.A)) {
  Debug.Log ("KeyDown A");
}
```

You should see log statements in your Console whenever you press `a` on the keyboard

- try `Input.GetKeyUp` and `Input.GetKey` - how to their behaviors differ?

#### Setting up a Scripted Camera Fade

Let's try something more useful - how about something that fades to black when you hit spacebar? A nice fade is especially useful for VR applications since hard cuts between views can be jarring to users.

In the assets folder, look for a prefab (that's the blue cube symbol) and drag it into the scene. Position it so that your camera is in the center of the cube. In your keyboard script, create a GameObject variable with a SerializeField decorator called `fadeCube`, then return to the editor.

```
// in MyKeyboardController.cs
[SerializeField]
GameObject fadeCube;
```

If you don't immediately see a GameObject slot appear in the inspector window with your script, hit Play (Ctrl+P) and it should force the script to reload. When you see it, drag your fadeCube prefab onto it. Now you've got a *variable* in your script that points to a *object* in the scene.

In the Hierarchy view, click on the FadeCube and examine it in the Inspector. You should see a script Component attached to it called `AlphaFader.cs`. Double-click on that to load it in a text editor. You should see the familiar `Start()` and `Update()` functions along with `OnRenderImage()` and some *public* functions. Since they are public, they can be called from other contexts.

To trigger a fade from `MyKeyboardController`, we'll use the reference that we made to `fadeCube` and call `GetComponent` to look up its script, `AlphaFader`.  Once we have a reference to the script, we can call any of its public functions. If your editor has code suggestions, you should see `StartFadeOut()` and `StartFadeIn()` appear as options when you type `.` after the script object.

```
fadeCube.GetComponent<AlphaFader>().StartFadeOut ();
```

The `<` and `>` surrounding `AlphaFader` indicate the *type* of component we are asking the fadeCube GameComponent for. If you didn't specify the type, Unity wouldn't know what functions it has available.

- add `StartFadeOut`to a key of your choice and test it
- assign a different key to invoke `StartFadeIn`
- fade in and out to your heart's content

#### Arranging Things in Your Scene
Before we move on, let's add one more keyboard shortcut. This time, we'll take the sword we painstakingly imported before and make it easy for us to pick it up.

- make sure you have the sword prefab added to your scene (viewable in the Hierarchy)
- add a `awesomeSword` variable to your `MyKeyboardController` script and connect the sword instance in the Hierarchy to the script variable in the inspector
- create a keyboard shortcut that will move the sword so that it is right in front of the camera

If you need a hint, the inside of your function will probably look something like this:`sword.transform.position = new Vector3 (3.2f, 1.5f, 1.0f);`

Once you've got the sword magically flying to your 'hands' at will:

- rewrite your function to move to the right place wherever the camera moves (hint: you can create another GameObject variable that points to the camera and assign its parent as a transform `sword.transform.parent = mainCamera.transform;` ... but how can we figure out the Camera's position?)
- add another keyboard shortcut to rotate the sword downward over several frames to 'swing' it
