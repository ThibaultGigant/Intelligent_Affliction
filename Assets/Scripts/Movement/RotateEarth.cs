using UnityEngine;
using System.Collections;

public class RotateEarth : MonoBehaviour {
	/**
	 * Position du centre de la Terre, pour le calcul de l'angle entre les deux directions : 
	 * centre-point_précédent et centre-point_courant
	 */
	private Transform earthCenter;
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
	private float distanceMax;
	/**
	 * Sensibilité du zoom de la caméra
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
	 * Garde en mémoire la taille de la fenêtre, en cas de redimensionnement
	 */
	private float lastScreenSize;

	// Use this for initialization
	void Start () {
		// Initialisation de la vue de la caméra
		earthCenter = transform;
		distanceMax = Mathf.Max(GetComponent<Collider> ().bounds.extents.y + Screen.height / Parametres.hauteurMenuPrincipal, GetComponent<Collider> ().bounds.extents.x / Camera.main.aspect) + marginScreen;
		Camera.main.orthographicSize = distanceMax;

		// Garde en mémoire les valeurs de références
		startingDirection = Vector3.zero;
		lastScreenSize = Mathf.Min(Screen.width, Screen.height);
	}
	
	/**
	 * Fait tourner la terre en suivant la souris lorsque le clique droit est enfoncé
	 * Fait un zoom lorsque la molette de la souris est actionnée
	 * Fait en sorte que le zoom reste le même après un redimensionnement de la fenêtre
	 */
	void Update () {
		// Mise à jour de la distance maximale, en cas de redimensionnement de la fenêtre
		distanceMax = Mathf.Max(GetComponent<Collider> ().bounds.extents.y + Screen.height / Parametres.hauteurMenuPrincipal, GetComponent<Collider> ().bounds.extents.x / Camera.main.aspect) + marginScreen;

		// Mouvements de la caméra
		rotate ();
		zoom();

		// Correction
		checkScreenResized ();
	}

	/**
	 * Rotation de la terre
	 * Suit le curseur, lorsque le clique droit de la souris est enfoncé
	 */
	private void rotate() {
		if (resetFlag) {
			resetRotation ();
			return;
		}

		RaycastHit hit = MouseManager.getHitEarth ();
		// Si le clique droit et appuyé et que la Terre est visée
		if (MouseManager.rightClickPushed && MouseManager.doesHit(gameObject)) {
			// Initialisation
			if (startingDirection == Vector3.zero) {
				startingDirection = hit.point - earthCenter.position;
			}
			// La Terre suit la souris
			else {
				currentDirection = hit.point;

				// Axe de rotation
				Vector3 rotationAxis = Vector3.Cross (startingDirection, currentDirection);
				//rotationAxis.x = 0;
				//rotationAxis.z = 0;

				float rotationAngle = Vector3.Angle (startingDirection, currentDirection);
				earthCenter.RotateAround(earthCenter.position, rotationAxis, rotationAngle);
				startingDirection = currentDirection;
			}
		}
		// Réinitialisation pour la prochaine rotation
		else if (startingDirection != Vector3.zero) {
			startingDirection = Vector3.zero;
		}
	}

	/**
	 * Demande le replacement de la caméra à l'angle initial, sur le pays initial
	 * Fonction appelée lorsque le bouton associé est activé
	 */
	public void resetRotationButton() {
		resetFlag = true;
	}

	/**
	 * Replace la caméra au point initial
	 * La terre tourne à une certaine vitesse, ce n'est pas instantané
	 */
	private void resetRotation() {
		float angleToTurn = Quaternion.Angle (earthCenter.rotation, Quaternion.identity);

		Quaternion old = earthCenter.rotation;

		earthCenter.rotation = Quaternion.Lerp (earthCenter.rotation, Quaternion.identity, Mathf.Clamp01 (angleToTurn > 0 ? speedReset / angleToTurn : 0f));

		if (Camera.main.orthographicSize < distanceMax)
			Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize + speedReset / 2f, distanceMax);
		else
			Camera.main.orthographicSize = distanceMax;

		if (earthCenter.rotation == old && Camera.main.orthographicSize == distanceMax)
			resetFlag = false;
	}

	/**
	 * Effectue le zoom demandé sur la planète
	 */
	private void zoom() {
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (wheel > 0)
			zoomIn (-1.0f * wheel);
		else if (wheel < 0 )
			zoomOut (-1.0f * wheel);
	}

	/**
	 * Zoome vers la planète
	 * @param zoom Valeur de zoom demandée
	 */
	private void zoomIn(float zoom) {
		if (Camera.main.orthographicSize <= distanceMin)
			return;

		Camera.main.orthographicSize += zoom * sensitivity;
	}

	/**
	 * Dézoome de la planète
	 * @param zoom Valeur de zoom demandée
	 */
	private void zoomOut(float zoom) {
		if (Camera.main.orthographicSize >= distanceMax)
			return;

		Camera.main.orthographicSize += zoom * sensitivity;
	}

	/**
	 * Effectue un zoom ou dézoom sur la Terre pour garder la même taille relativement à la fenêtre
	 */
	private void checkScreenResized() {
		float currentScreenSize = Mathf.Min (Screen.width, Screen.height);
		if (lastScreenSize != currentScreenSize) {
			Camera.main.orthographicSize /= currentScreenSize / lastScreenSize;
			lastScreenSize = currentScreenSize;
		}
	}
}