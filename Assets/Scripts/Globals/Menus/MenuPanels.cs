using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuPanels : MonoBehaviour
{

	/**
	 * Boutton du Happiness
	 */
	public Button HappinessButton;

	/**
	 * Panel des catégories
	 */
	public GameObject CategoriesPanel;

	/**
	 * Sprite plus
	 */
	public Sprite plusButton;

	/**
	 * Sprite moins
	 */
	public Sprite moinsButton;

	/**
	 * Clique sur le boutton Happinnes
	 */
	public void HappinessOnClick() {
		Image image = HappinessButton.GetComponent<Image> ();

		if (image.sprite == plusButton) {
			CategoriesPanel.SetActive (true);
			image.sprite = moinsButton;
		}
		else {
			CategoriesPanel.SetActive (false);
			image.sprite = plusButton;
		}

		CategoriesPanel.GetComponent<CategoriesObserver> ().toggleVisible();

	}
}

