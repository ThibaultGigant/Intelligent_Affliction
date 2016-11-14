using UnityEngine;
using System.Collections;

public class RotateEarth : MonoBehaviour {
	/**
	 * Position du centre de la Terre, pour le calcul de l'angle entre les deux directions : 
	 * centre-point_précédent et centre-point_courant
	 */
	private Transform earthCenter;
	/**
	 * Transform de l'objet auquel on applique la rotation
	 */
	private Transform tr;

	/**
	 * rayon qui va vers la souris
	 */
	private Ray ray;
	/**
	 * hit du Raycast
	 */
	private RaycastHit hit;
	/**
	 * Direction du rayon allant du centre de la terre au point visé précédemment
	 */
	private Vector3 startingDirection;
	/**
	 * Direction courante du rayon allant du centre de la terre au point visé actuellement
	 */
	private Vector3 currentDirection;
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
	/**
	 * Appelée à la création de la classe, elle initialise les attributs aux valeurs par défaut
	 */
	void Start () {
		// Initialisation de la vue de la caméra
		earthCenter = transform;
		tr = transform;
		distanceMax = Mathf.Max(GetComponent<Collider> ().bounds.extents.y + Screen.height / Parametres.hauteurMenuPrincipal, GetComponent<Collider> ().bounds.extents.x / Camera.main.aspect) + marginScreen;
		Camera.main.orthographicSize = distanceMax;

		// Garde en mémoire les valeurs de références
		startingDirection = Vector3.zero;
		lastScreenSize = Mathf.Min(Screen.width, Screen.height);
	}
	
	/**
	 * Update is called once per frame
	 * ---
	 * Fait tourner la terre en suivant la souris lorsque le clique droit est enfoncé<br />
	 * Fait un zoom lorsque la molette de la souris est actionnée<br />
	 * Fait en sorte que le zoom reste le même après un redimensionnement de la fenêtre<br />
	 */
	void Update () {
		// Mise à jour de la distance maximale, en cas de redimensionnement de la fenêtre
		distanceMax = Mathf.Max(GetComponent<Collider> ().bounds.extents.y + Screen.height / Parametres.hauteurMenuPrincipal, GetComponent<Collider> ().bounds.extents.x / Camera.main.aspect) + marginScreen;

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
	 * Suit le curseur, lorsque le clic droit de la souris est enfoncé
	 */
	private void rotate() {
		if (resetFlag) {
			resetRotation ();
			return;
		}

		// Rayon lancé de la caméra pour voir s'il y a clic sur un pays
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			// Si c'est la première fois qu'on clique, on stocke le point de collision
			if (startingDirection == Vector3.zero && Input.GetMouseButton(1)) {
				startingDirection = hit.point - earthCenter.position;
			}
			else if (Input.GetMouseButton(1)) { // Sinon on bouge
				currentDirection = hit.point;

				// Axe de rotation
				Vector3 rotationAxis = Vector3.Cross (startingDirection, currentDirection);

				// Application de la rotation
				float rotationAngle = Vector3.Angle (startingDirection, currentDirection);
				tr.RotateAround(earthCenter.position, rotationAxis, rotationAngle);
				startingDirection = currentDirection;
			}
			else { // Remise à zéro de la valeur quand on relache
				startingDirection = Vector3.zero;
			}
		}
	}

	/**
	 * resetRotationButton
	 * ---
	 * Fonction appelée lorsque le boutton associé est activé
	 * 
	 * Pour replacer la caméra au point initial
	 */
	public void resetRotationButton() {
		resetFlag = true;
	}

	/**
	 * resetRotation
	 * ---
	 * Replace la caméra au point initial
	 * 
	 * La terre tourne à une certaine vitesse, ce n'est pas instantané
	 */
	private void resetRotation() {
		float angleToTurn = Quaternion.Angle (tr.rotation, Quaternion.identity);

		Quaternion old = tr.rotation;

		tr.rotation = Quaternion.Lerp (tr.rotation, Quaternion.identity, Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));

		if (Camera.main.orthographicSize < distanceMax)
			Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize + speedReset / 2f, distanceMax);
		else
			Camera.main.orthographicSize = distanceMax;

		if (tr.rotation == old && Camera.main.orthographicSize == distanceMax)
			resetFlag = false;
	}

	/**
	 * Applique un zoom sur la terre selon le scroll
	 */
	private void zoom() {
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (wheel > 0)
			zoomIn (-1.0f * wheel);
		else if (wheel < 0 )
			zoomOut (-1.0f * wheel);
	}

	/**
	 * Zoome vers la terre en fonction du scroll
	 */
	private void zoomIn(float zoom) {
		if (Camera.main.orthographicSize <= distanceMin)
			return;

		Camera.main.orthographicSize += zoom * sensitivity;
	}

	/**
	 * Dézoome de la terre en fonction du scroll
	 */
	private void zoomOut(float zoom) {
		if (Camera.main.orthographicSize >= distanceMax)
			return;

		Camera.main.orthographicSize += zoom * sensitivity;
	}

	private void checkScreenResized() {
		float currentScreenSize = Mathf.Min (Screen.width, Screen.height);
		if (lastScreenSize != currentScreenSize) {
			Camera.main.orthographicSize /= currentScreenSize / lastScreenSize;
			lastScreenSize = currentScreenSize;
		}
	}
}
