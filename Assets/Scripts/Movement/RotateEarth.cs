using UnityEngine;
using System.Collections;

public class RotateEarth : MonoBehaviour {
	/**
	 * Camera qui voit la scene
	 */
	public Camera camera;
	/**
	 * Position du centre de la Terre, pour le calcul de l'angle entre les deux directions : 
	 * centre-point_précédent et centre-point_courant
	 */
	public Vector3 earthCenter = new Vector3(0f, -15.77f, -5.79f);
	/**
	 * Transform de l'objet auquel on applique la rotation
	 */
	public Transform tr;

	/**
	 * rayon qui va vers la souris
	 */
	Ray ray;
	/**
	 * hit du Raycast
	 */
	RaycastHit hit;
	/**
	 * Direction du rayon allant du centre de la terre au point visé précédemment
	 */
	Vector3 startingDirection;
	/**
	 * Direction courante du rayon allant du centre de la terre au point visé actuellement
	 */
	Vector3 currentDirection;
	/**
	 * Distance minimale de la caméra par rapport à la terre
	 */
	private float distanceMin = 10f;
	/**
	 * Distance maximale de la caméra par rapport à la terre
	 */
	private float distanceMax = 65f;
	/**
	 * Sensibilité du zoom de la  caméra
	 */
	private float sensitivity = 10f;
	/**
	 * Vitesse de déplacement de la terre lors du reset
	 */
	private float speedReset = 4f;
	/**
	 * Booléen permettant de bloquer toute action durant l'animation du reset
	 */
	private bool resetFlag = false;

	// Use this for initialization
	void Start () {
		startingDirection = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		rotate ();
		zoom();
		tr.position = Vector3.zero;
	}

	/**
	 * Rotation de la terre
	 * ---
	 * Suit le curseur, lorsque le clique droit de la souris est enfoncé
	 */
	private void rotate() {
		if (resetFlag) {
			resetRotation ();
			return;
		}

		ray = camera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			if (startingDirection == Vector3.zero && Input.GetMouseButton(1)) {
				startingDirection = hit.point - earthCenter;
			}
			else if (Input.GetMouseButton(1)) {
				currentDirection = hit.point;

				// Axe de rotation
				Vector3 rotationAxis = Vector3.Cross (startingDirection, currentDirection);
				//rotationAxis.x = 0;
				//rotationAxis.z = 0;

				float rotationAngle = Vector3.Angle (startingDirection, currentDirection);
				tr.RotateAround(earthCenter, rotationAxis, rotationAngle);
				startingDirection = currentDirection;
			}
			else {
				startingDirection = Vector3.zero;
			}
		}
	}

	public void resetRotationButton() {
		resetFlag = true;
		//tr.rotation = Quaternion.identity;
		//tr.position = Vector3.zero;
		//camera.orthographicSize = 65f;
	}

	private void resetRotation() {
		float angleToTurn = Quaternion.Angle (tr.rotation, Quaternion.identity);
		float turnSpeed = Mathf.Min (angleToTurn, speedReset);

		Quaternion old = tr.rotation;

		tr.rotation = Quaternion.Lerp (tr.rotation, Quaternion.identity, Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));

		if (camera.orthographicSize < 64f)
			camera.orthographicSize = Mathf.Min (camera.orthographicSize + speedReset / 2f, 65f);
		else
			camera.orthographicSize = 65f;

		if (tr.rotation == old && camera.orthographicSize == 65f)
			resetFlag = false;
	}

	private void zoom() {
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (wheel < 0)
			zoomIn (wheel);
		else if (wheel > 0 )
			zoomOut (wheel);
	}

	private void zoomIn(float zoom) {
		if (camera.orthographicSize <= distanceMin)
			return;

		camera.orthographicSize += zoom * sensitivity;
	}

	private void zoomOut(float zoom) {
		if (camera.orthographicSize >= distanceMax)
			return;

		camera.orthographicSize += zoom * sensitivity;
	}
}
