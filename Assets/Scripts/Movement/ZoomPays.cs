﻿using UnityEngine;
using System.Collections;

public class ZoomPays : MonoBehaviour {

	public Transform trEarth;

	private MeshCollider collider;
	private Vector3 boundsSize;
	private float size;
	private float distanceMax;
	private float marginScreen = 5f;
	private bool buttonCalled = false;
	private float oldOrthographicSize;
	private Transform oldTrEarth;
	/**
	 * Vitesse de déplacement de la terre lors du reset
	 */
	private float speedReset = 0.5f;

	private bool zoomIn;

	// Use this for initialization
	void Start () {
		oldTrEarth = trEarth;
		oldOrthographicSize = Camera.main.orthographicSize;
		collider = GetComponent<MeshCollider> ();
		boundsSize = collider.bounds.size;
		size = Mathf.Max (boundsSize.x, Mathf.Max (boundsSize.y, boundsSize.z));
	}
	
	// Update is called once per frame
	void Update () {
		if (buttonCalled) {
			Quaternion old = trEarth.rotation;
			float angleToTurn = Vector3.Angle (Camera.main.transform.position - trEarth.position, transform.localPosition - trEarth.position);

			// Angle de rotation
			Vector3 paysPlan = transform.localPosition;
			paysPlan.y = 0;
			Vector3 earthPlan = trEarth.position;
			earthPlan.y = 0;
			Vector3 cameraPlan = Camera.main.transform.position;
			cameraPlan.y = 0;

			float distancePaysToEarth = Vector3.Distance (paysPlan, earthPlan);
			float distanceToEarth = Vector3.Distance(cameraPlan, earthPlan);
			float distanceToPays = Vector3.Distance(cameraPlan, paysPlan);

			angleToTurn = Mathf.Acos ((Mathf.Pow (distancePaysToEarth, 2) + Mathf.Pow (distanceToEarth, 2) - Mathf.Pow (distanceToPays, 2)) / (2 * distancePaysToEarth * distanceToEarth));

			float turnSpeed = Mathf.Min (angleToTurn, speedReset);

			Vector3 currentPointOfVieuw = Camera.main.transform.position - trEarth.position;
			Vector3 aimedPointOfView = transform.localPosition - trEarth.position;
			aimedPointOfView.x = currentPointOfVieuw.x;

			trEarth.rotation = Quaternion.Lerp (trEarth.rotation, new Quaternion(0.0f, 1.0f, 0.05f, 0.1f), Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));

			if (Quaternion.Angle (trEarth.rotation, new Quaternion (0.0f, 1.0f, 0.05f, 0.1f)) < speedReset) {
				distanceMax = Mathf.Max (GetComponent<MeshRenderer> ().bounds.extents.y, GetComponent<MeshRenderer> ().bounds.extents.x / Camera.main.aspect) + marginScreen + Screen.height / Parametres.hauteurMenuPrincipal;
				if (Camera.main.orthographicSize > distanceMax + 1) {
					Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize - speedReset * 2f, distanceMax);
				}
				else if (Camera.main.orthographicSize < distanceMax - 1) {
					Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize + speedReset * 2f, distanceMax);
				}
				else
					Camera.main.orthographicSize = distanceMax;
			}

			if (oldOrthographicSize == Camera.main.orthographicSize && Quaternion.Angle(trEarth.rotation, new Quaternion(0.0f, 1.0f, 0.05f, 0.1f)) < speedReset) {
				buttonCalled = false;
			}
		}



		oldOrthographicSize = Camera.main.orthographicSize;
		oldTrEarth = trEarth;
	}

	public void activeButton() {
		buttonCalled = true;
	}
}
