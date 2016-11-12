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
	public Transform earthCenter;
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
	/**
	 * Marge tolérée pour le zoom de la caméra
	 */
	private float marginScreen = 10f;
	/**
	 * Garde en mémoire la taille de la fenpetre, en cas de redimensionnement
	 */
	private float lastScreenSize;

	// Use this for initialization
	void Start () {
		// Initialisation de la vue de la caméra
		distanceMax = Mathf.Max(GetComponent<Collider> ().bounds.extents.y, GetComponent<Collider> ().bounds.extents.x / camera.aspect) + marginScreen + Screen.height / Parametres.hauteurMenuPrincipal;
		camera.orthographicSize = distanceMax;

		// Garde en mémoire les valeurs de références
		startingDirection = Vector3.zero;
		lastScreenSize = Mathf.Min(Screen.width, Screen.height);
	}
	
	/**
	 * Update is called once per frame
	 * ---
	 * Fait tourner la terre en suivant la souris lorsque le clique droit est enfoncé
	 * Fait un zoom lorsque la molette de la souris est actionnée
	 * Fait en sorte que le zoom reste le même après un redimensionnement de la fenêtre
	 */
	void Update () {
		// Mise à jour de la distance maximale, en cas de redimensionnement de la fenêtre
		distanceMax = Mathf.Max(GetComponent<Collider> ().bounds.extents.y, GetComponent<Collider> ().bounds.extents.x / camera.aspect) + marginScreen + Screen.height / Parametres.hauteurMenuPrincipal;

		// Mouvements de la caméra
		rotate ();
		zoom();

		// Correction
		tr.position = Vector3.zero;
		checkScreenResized ();
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
				startingDirection = hit.point - earthCenter.position;
			}
			else if (Input.GetMouseButton(1)) {
				currentDirection = hit.point;

				// Axe de rotation
				Vector3 rotationAxis = Vector3.Cross (startingDirection, currentDirection);
				//rotationAxis.x = 0;
				//rotationAxis.z = 0;

				float rotationAngle = Vector3.Angle (startingDirection, currentDirection);
				tr.RotateAround(earthCenter.position, rotationAxis, rotationAngle);
				startingDirection = currentDirection;
			}
			else {
				startingDirection = Vector3.zero;
			}
		}
	}

	/**
	 * resetRotationButton
	 * ---
	 * Fonction appelée lorsque le boutton associé est activé
	 * Pour replacer la caméra au point initial
	 */
	public void resetRotationButton() {
		resetFlag = true;
		//tr.rotation = Quaternion.identity;
		//tr.position = Vector3.zero;
		//camera.orthographicSize = 65f;
	}

	/**
	 * resetRotation
	 * ---
	 * Replace la caméra au point initial
	 * La terre tourne à une certaine vitesse, ce n'est pas instantané
	 */
	private void resetRotation() {
		float angleToTurn = Quaternion.Angle (tr.rotation, Quaternion.identity);
		float turnSpeed = Mathf.Min (angleToTurn, speedReset);

		Quaternion old = tr.rotation;

		tr.rotation = Quaternion.Lerp (tr.rotation, Quaternion.identity, Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));

		//if (camera.orthographicSize < 64f)
		if (camera.orthographicSize < distanceMax)
			camera.orthographicSize = Mathf.Min (camera.orthographicSize + speedReset / 2f, distanceMax);
		else
			camera.orthographicSize = distanceMax;

		if (tr.rotation == old && camera.orthographicSize == distanceMax)
			resetFlag = false;
	}

	private void zoom() {
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (wheel > 0)
			zoomIn (-1.0f * wheel);
		else if (wheel < 0 )
			zoomOut (-1.0f * wheel);
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

	private void checkScreenResized() {
		float currentScreenSize = Mathf.Min (Screen.width, Screen.height);
		if (lastScreenSize != currentScreenSize) {
			camera.orthographicSize /= currentScreenSize / lastScreenSize;
			lastScreenSize = currentScreenSize;
		}
	}
}
