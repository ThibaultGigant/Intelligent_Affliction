using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {
	/**
	 *  Indique si le clique droit est enfoncé
	 */
	[HideInInspector] public static bool rightClickPushed = false;
	/**
	 * Indique si un simple clique gauche à été détecté
	 */
	[HideInInspector] public static bool simpleLeftClick = false;
	/**
	 * Indique si un double clique gauche à été détecté
	 */
	[HideInInspector] public static bool doubleLeftClick = false;



	/**
	 * Rayon permettant de déterminer l'endroit où pointe la souris
	 */
	private static Ray ray;
	/**
	 * hit du Raycast
	 */
	private static RaycastHit hit;
	/**
	 * hit du Raycast, passant à travers les pays
	 */
	private static RaycastHit hitForEarth;
	/**
	 * hit du Raycast, ne touchant que les ports
	 */
	private static RaycastHit hitMaritimeMode;
	/**
	 * Indique si un premier clique gauche à été effectué
	 */
	private bool oneClick = false;
	/**
	 * Garde en mémoire le moment où le premier clique à été effectué
	 */
	private float timeOfFirstClick;
	/**
	 * Délai autorisé entre deux cliques
	 */
	private float delayForDoubleClick = 0.2f;
	/**
	 * Indique si le Raycast touche un objet
	 */
	private static bool hitSomething = false;
	/**
	 * Indique si le Raycast touche la Terre
	 */
	private static bool hitEarth = false;
	/**
	 * Indique si le Raycast touche un port dans le mode maritime
	 */
	private static bool hitMaritime = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ResetActionBooleans ();
		checkMouseActions ();

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hitForEarth, Mathf.Infinity, ~LayerMask.NameToLayer ("Pays"))) {
			hitEarth = true;
		}
		if (Physics.Raycast (ray, out hit)) {
			hitSomething = true;	
		}
		if (Parametres.chooseMaritime && Physics.Raycast (ray, out hitMaritimeMode, Mathf.Infinity, ~LayerMask.NameToLayer ("Maritime"))) {
			hitMaritime = true;
		}
	}
	
	private void checkMouseActions() {
		/** Si c'est un clique droit
		 * ---
		 * • Rotation de la terre
		 * • Menu contextuel ( Vérifier que la souris n'ai pas (ou peu) bougé pendant le clique )
		 */
		bool tmp = false;
		if (Input.GetMouseButton(1))
		{
			rightClickPushed = true;
		}
		/**
 		 * Sinon, si c'est un clique gauche
		 */
		else if (Input.GetMouseButtonDown(0))
		{
			// Simple clique
			if (!oneClick)
			{
				simpleLeftClick = true;
				oneClick = true;
				timeOfFirstClick = Time.time;
				tmp = true;
			}
			// Double clique
			else if (Time.time - timeOfFirstClick < delayForDoubleClick)
			{
				doubleLeftClick = true;
				oneClick = false;
			}
		}

		if (oneClick && Time.time - timeOfFirstClick > delayForDoubleClick &&  !tmp) {
			oneClick = false;
		}
	}

	private void ResetActionBooleans () {
		simpleLeftClick = false;
		doubleLeftClick = false;
		rightClickPushed = false;
		hitSomething = false;
		hitMaritime = false;
		hitEarth = false;
	}

	public static RaycastHit getHit() {
		return hit;
	}

	public static RaycastHit getHitEarth() {
		return hitForEarth;
	}

	public static Ray getRay() {
		return ray;
	}

	public static bool doesHit(GameObject obj) {
		// Si obj est la Terre
		if (obj.name == Parametres.earth.name) {
			return (hitEarth);
		}
		// Sinon
		return (hitSomething && hit.collider.name == obj.name);
	}

	public static bool doesHitMaritime(GameObject obj) {
		if (!Parametres.chooseMaritime)
			return false;
		return (hitMaritime && 
			hitMaritimeMode.collider.name 
			== obj.name);
	}
}
