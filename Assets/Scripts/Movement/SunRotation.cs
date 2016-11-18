using UnityEngine;
using System.Collections;

public class SunRotation : MonoBehaviour {

	/**
	 * speedRotation : vitesse de rotation du soleil
	 */
	private float speedRotation = 0.1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float rotationAngle = speedRotation * Parametres.getTimeSpeed ();
		transform.RotateAround(Parametres.earthCenter, Parametres.earth.transform.up, rotationAngle);
	}
}
