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


	// Use this for initialization
	void Start () {
		startingDirection = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		ray = camera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			if (startingDirection == Vector3.zero && Input.GetMouseButton(1)) {
				startingDirection = hit.point - earthCenter;
			}
			else if (Input.GetMouseButton(1)) {
				currentDirection = hit.point;
				Vector3 rotationAxis = Vector3.Cross (startingDirection, currentDirection);
				float rotationAngle = Vector3.Angle (startingDirection, currentDirection);
				tr.RotateAround(earthCenter, rotationAxis, rotationAngle);
				startingDirection = currentDirection;
			}
			else {
				startingDirection = Vector3.zero;
			}
		}
	}

	public void resetRotation() {
		tr.rotation = Quaternion.identity;
		tr.position = Vector3.zero;
	}
}
