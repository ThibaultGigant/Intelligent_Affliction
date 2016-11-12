using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public RawImage menuPrincipal;

	private float largeurMenuPrincipal = Screen.width * 1.0f;

	// Use this for initialization
	void Start () {
		resizeMenuPrincipal ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (largeurMenuPrincipal != Screen.width * 1.0f) {
			largeurMenuPrincipal = Screen.width * 1.0f;
			resizeMenuPrincipal ();
		}
	}

	private void resizeMenuPrincipal() {
		menuPrincipal.rectTransform.sizeDelta = new Vector2(largeurMenuPrincipal, Parametres.hauteurMenuPrincipal);
	}
}
