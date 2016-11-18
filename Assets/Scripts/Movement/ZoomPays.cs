using UnityEngine;
using System.Collections;

public class ZoomPays : MonoBehaviour {

	/**
	 * Composant Transform de la Terre
	 */
	private Transform trEarth;
	/**
	 * Taille que la caméra doit respercter afin de pouvoir afficher tout le pays
	 */
	private float distanceMax;
	/**
	 * Marge à prendre en compte lors du zoom de la caméra
	 */
	private float marginScreen = 5f;
	/**
	 * Booléen indiquant si l'action est demandée, ou en cours d'execution
	 */
	private bool buttonCalled = false;
	/**
	 * Vitesse de déplacement de la terre lors du reset
	 */
	private float speedReset = 2f;

	// Use this for initialization
	void Start () {
		trEarth = Parametres.earth.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (buttonCalled) {
			float angleToTurn = Vector3.Angle (Camera.main.transform.position - trEarth.position, transform.localPosition - trEarth.position);

			// Angle de rotation
			Vector3 paysPlan = transform.localPosition;
			paysPlan.y = 0;
			Vector3 earthPlan = trEarth.position;
			earthPlan.y = 0;
			Vector3 cameraPlan = Camera.main.transform.position;
			cameraPlan.y = 0;

			/*
			float distancePaysToEarth = Vector3.Distance (paysPlan, earthPlan);
			float distanceToEarth = Vector3.Distance(cameraPlan, earthPlan);
			float distanceToPays = Vector3.Distance(cameraPlan, paysPlan);
			angleToTurn = Mathf.Acos ((Mathf.Pow (distancePaysToEarth, 2) + Mathf.Pow (distanceToEarth, 2) - Mathf.Pow (distanceToPays, 2)) / (2 * distancePaysToEarth * distanceToEarth));
			*/

			Vector3 currentPointOfVieuw = Camera.main.transform.position - trEarth.position;
			Vector3 aimedPointOfView = transform.localPosition - trEarth.position;
			aimedPointOfView.x = currentPointOfVieuw.x;

			trEarth.rotation = Quaternion.Lerp (trEarth.rotation, new Quaternion(0.0f, 1.0f, 0.05f, 0.1f), Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));
			Debug.Log (Quaternion.Lerp (trEarth.rotation, new Quaternion(0.0f, 1.0f, 0.05f, 0.1f),1f));// Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f)));


			if (Quaternion.Angle (trEarth.rotation, new Quaternion (0.0f, 1.0f, 0.05f, 0.1f)) < 15f * speedReset) {
			}


			distanceMax = Mathf.Max (GetComponent<MeshRenderer> ().bounds.extents.y + Screen.height / Parametres.hauteurMenuPrincipal, GetComponent<MeshRenderer> ().bounds.extents.x / Camera.main.aspect) + marginScreen;

			if (Camera.main.orthographicSize > distanceMax + 1) {
				Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize - speedReset / Mathf.Max(Quaternion.Angle (trEarth.rotation, new Quaternion (0.0f, 1.0f, 0.05f, 0.1f)) / 20f, 1f), distanceMax);
			}
			else if (Camera.main.orthographicSize < distanceMax - 1) {
				Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize + speedReset / Mathf.Max(Quaternion.Angle (trEarth.rotation, new Quaternion (0.0f, 1.0f, 0.05f, 0.1f)) / 20f, 1f), distanceMax);
			}
			else
				Camera.main.orthographicSize = distanceMax;

			if (Camera.main.orthographicSize == distanceMax && Quaternion.Angle(trEarth.rotation, new Quaternion(0.0f, 1.0f, 0.05f, 0.1f)) < 0.001f) {
				buttonCalled = false;
			}
		}
	}

	/**
	 * Active l'animation de zoom sur le pays sélectionné
	 */
	public void activeButton() {
		buttonCalled = true;
	}
}
