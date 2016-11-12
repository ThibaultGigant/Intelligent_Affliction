using UnityEngine;
using System.Collections;

public class SunRotation : MonoBehaviour {

	public Transform tr;

	/**
	 * speedRotation : vitesse de rotation du soleil
	 */
	private float speedRotation = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float rotationAngle = speedRotation * Parametres.getTimeSpeed (); // / (1.0f * Parametres.timeOfADay);
		Quaternion rotationAxis = Parametres.earthAxis;// * tr.rotation;
		transform.RotateAround(Parametres.earthCenter, rotationAxis.ToEulerAngles(), rotationAngle);
	}
}
