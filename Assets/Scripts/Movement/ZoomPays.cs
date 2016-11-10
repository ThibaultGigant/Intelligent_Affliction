using UnityEngine;
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
	 * Sensibilité du zoom de la  caméra
	 */
	private float sensitivity = 10f;
	/**
	 * Vitesse de déplacement de la terre lors du reset
	 */
	private float speedReset = 4f;

	// Use this for initialization
	void Start () {
		oldOrthographicSize = Camera.main.orthographicSize;
		collider = GetComponent<MeshCollider> ();
		boundsSize = collider.bounds.size;
		size = Mathf.Max (boundsSize.x, Mathf.Max (boundsSize.y, boundsSize.z));
	}
	
	// Update is called once per frame
	void Update () {
		if (buttonCalled) {
			Debug.Log ("Timon");
			distanceMax = Mathf.Max (GetComponent<MeshRenderer> ().bounds.extents.y, GetComponent<MeshRenderer> ().bounds.extents.x / Camera.main.aspect) + marginScreen;

			//Quaternion.FromToRotation (Camera.main.transform.position, transform.position) - trEarth.position;

			//float angleToTurn = Quaternion.Angle (trEarth.rotation, Quaternion.Euler( transform.position - trEarth.position));
			float angleToTurn = Vector3.Angle (Camera.main.transform.position - trEarth.position, transform.position - trEarth.position);
			Debug.Log (angleToTurn);
			float turnSpeed = Mathf.Min (angleToTurn, speedReset);

			Quaternion old = trEarth.rotation;

			//trEarth.rotation = Quaternion.Lerp (trEarth.rotation, Quaternion.Euler( transform.position - Camera.main.transform.position), Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));
			trEarth.rotation = Quaternion.AngleAxis(Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f), Vector3.Cross (Camera.main.transform.position - trEarth.position, transform.position - trEarth.position));

			if (Camera.main.orthographicSize < distanceMax)
				Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize + speedReset / 2f, distanceMax);
			else
				Camera.main.orthographicSize = distanceMax;
			
			if (oldOrthographicSize == Camera.main.orthographicSize && oldTrEarth.rotation.Equals(trEarth.rotation)) {
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
