// Control Direction Grab Action|SecondaryControllerGrabActions|60040
namespace VRTK.SecondaryControllerGrabActions
{
	using UnityEngine;
	using System.Collections;

	[AddComponentMenu("VRTK/Scripts/Interactions/Secondary Controller Grab Actions/PowerUpSword")]
	public class PowerUpSword : VRTK_ControlDirectionGrabAction{
		[SerializeField]
		bool isPoweredUp = false;

		[SerializeField]
		BladeController bladeController;

		public override void Initialise(VRTK_InteractableObject currentGrabbdObject, VRTK_InteractGrab currentPrimaryGrabbingObject, VRTK_InteractGrab currentSecondaryGrabbingObject, Transform primaryGrabPoint, Transform secondaryGrabPoint){
			base.Initialise(currentGrabbdObject, currentPrimaryGrabbingObject, currentSecondaryGrabbingObject, primaryGrabPoint, secondaryGrabPoint);
			isPoweredUp = true;
			Debug.Log ("Powerup!!!");
			GetComponent<BladeController> ().EnablePowerUp();
		}

		public override void ResetAction(){
			isPoweredUp = false;
			Debug.Log ("ResetAction: Back to normal power");
			GetComponent<BladeController> ().DisablePowerUp();
			base.ResetAction();
		}

		public override void OnDropAction(){
			base.OnDropAction();
			isPoweredUp = false;
			Debug.Log ("DropAction: Back to normal power");
			GetComponent<BladeController> ().DisablePowerUp();
		}

	}
}