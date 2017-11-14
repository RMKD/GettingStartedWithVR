## Session III: Collisions and Physics
* getting to know Colliders and Rigidbodies
* using the OnCollisionEnter function
* managing what can collide with Tags
* gravity, kinematics, and friction

### Getting To Know Colliders and Rigidbodies

To understand how collisions and physics work in Unity we're going to have to get to know some more components: Colliders and Rigidbodies. When you're trying to get something working, it can sometimes be confusing which you're working with so it's helpful to build a good mental model of the differences between them.

**Colliders**: calculate the intersection between the colliders of other objects and importantly are frequently *not* the same shape as the mesh used for drawing.

**Rigidbodies**: apply force to an object based on inputs from the environment and can be configured to account for gravity or specific constraints.


Most of the objects you've worked with so far come with a collider attached. It's typically a regular geometric shape that can be represented by a few variables that is relatively cheap to compute. If you need to detect collisions with more precision you can change to a `Mesh` that matches your visual model exactly.



### Using the OnCollisionEnter Function

The most important thing you'll need for interacting with objects is Collision detection. Unity provides standard functions for doing this within Monobehaviour (that most scripts extend).


```
OnCollisionEnter(Collision col){
  Debug.Log (string.Format("OnCollisionEnter: {0} hits {1}", gameObject.name, col.gameObject.name));
}

OnCollisionExit(Collision col){
  Debug.Log (string.Format("OnCollisionExit: {0} lost contact with {1}", gameObject.name, col.gameObject.name));
}
```

### Managing What Can Collide With Tags

In the `Edit > Project Settings > Tags and Layers` menu, click the plus sign to add some custom tags. For our scene we'll use "Sliceable" and "Strong". There several default tags that exist but we can add our own.

When you select an object in the Hierarchy View and in the Inspector View you'll see the Tag dropdown under the object name. Select the Pillar object (or add it from a Prefab if its not in the scene already) and change its tag to `Strong`.

For now, create a new Cube and change its tag to `Sliceable`



### Make an Object Sliceable

To slice an object, there are two GameObjects we need to think about: the blade and the object being sliced. We could manage this collision from either place, but since we might slice multiple objects, we'll try to do most of the work on the blade - and we'll do that by writing a script.

- add a Cube to the scene
- resize it to 0.3, 0.3, 0.3 (this is pretty good for a human-scale object)
- tag the cube as 'Sliceable'

On the blade object, create a new script and name it `BladeController`. Then perform the following updates:

- set up functions for  `OnCollisionEnter` and `OnCollisionExit`
- add a top-level variable to reference the sliced pieces `private GameObject[] pieces;`
- add a check to see if the collided object is sliceable: `if (col.gameObject.tag == "Sliceable")`
- import the BLINDED_AM_ME package into your assets folder and set up the code to cut the object:

```
pieces = BLINDED_AM_ME.MeshCut.Cut (col.gameObject, transform.position, transform.right, replacementMaterial);

for (int i = 0; i < pieces.Length; i++) {
  Debug.Log (string.Format ("{0} of {1} pieces: {2}", i, pieces.Length, pieces [i].GetComponent<Rigidbody> ()));

  // add more stuff here shortly
}
```

Give that a try and make sure you've got the basics working.

If you've got new meshes being created, that's pretty good, but there's a few problems with what we've got so far. For instance, try pausing and looking at the objects you've sliced. The collider is still the same size and shape as the original. That leads to some weird behaviors - especially if you intend to slice objects more than once. To get cuts that match how we swing we need to make sure the collider matches the object exactly. To do that, we'll swap out the existing collider and add a fresh a MeshCollider.

```
// Remove existing colliders if they exist
Destroy (piece.GetComponent<BoxCollider> ());
Destroy (piece.GetComponent<CapsuleCollider> ());
Destroy (piece.GetComponent<SphereCollider> ());
Destroy (piece.GetComponent<MeshCollider> ());

// Add a MeshCollider to match the exact shape of the new piece
piece.AddComponent<MeshCollider> ().convex = true;
```

We also need to make sure that each new piece has a rigid body

```
// Ensure the new piece has a Rigidbody
if (!piece.GetComponent<Rigidbody> ()) {
  piece.AddComponent<Rigidbody> ();
}
```

If you find the blade is coming into too many objects too quickly, we can handle that by setting a bool to disable the blade when it enters a collision and enable it when it exits the collision.

```
bladeEnabled = false;
```

The last item to update is to make sure that every piece is tagged as Sliceable - place that in the `OnCollisionExit` to prevent the newly created pieces from generating new collisions and then reenable the blade.

```
for (int i = 0; i < pieces.Length; i++) {
  pieces [i].tag = "Sliceable";
}
bladeEnabled = true;
```

### Extra Things to Try

Experiment with adding physics effects. Can you re-create bullet time? Can you add some visual effects that make the sword change color when certain things happen?
