using UnityEngine;
using System.Collections;

public class SunRotation : MonoBehaviour {

	/**
	 * Composant Transform du Soleil
	 */
	private Transform tr;

	/**
	 * speedRotation : vitesse de rotation du soleil
	 */
	private float speedRotation = 0.1f;

	// Use this for initialization
	void Start () {
		tr = GetComponentInParent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		float rotationAngle = speedRotation * Parametres.getTimeSpeed (); // / (1.0f * Parametres.timeOfADay);
		// Quaternion rotationAxis = Parametres.earthAxis;// * tr.rotation; // Rotation de la terre au lieu de rotation du soleil. C'est pareil, mais en pas pareil... Ça me paraît plus naturel
		transform.RotateAround(Parametres.earthCenter, Vector3.up /*rotationAxis.eulerAngles*/, rotationAngle);
	}
}
