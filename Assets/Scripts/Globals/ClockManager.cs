using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;
using System.Threading;

public class ClockManager : MonoBehaviour
{
	/**
	 * Text devant contenir la date à afficher
	 */
	public Text textDate;

	/**
	 * Booléen informant si durant la frame courrante, une nouvelle journée démarre
	 * true si c'est le cas, false sinon
	 */
	public static bool newDay = true;

	/**
	 * Booléen informant si durant la frame courrante, un nouveau mois démarre
	 * true si c'est le cas, false sinon
	 */
	public static bool newMonth = true;

	/**
	 * Horloge. Compte le temps écoulé durant la journée
	 * Dès quelle dépasse Parametres.timeOfADay,
	 * une nouvelle journée démarre et on soustrait cette valeur à countTime
	 */
	private int countTime = Parametres.timeOfADay - 1;

	// Use this for initialization
	void Start ()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo ("fr-FR");
		displayDate ();
	}
	
	/**
	 * Fonction appelée à chaque frame.
	 * ---
	 * Se charge de compter le temps, et de mettre à jour la date.
	 */
	void Update ()
	{
		// newDay doit rester à true durant seulement une frame pour chaque journée
		if (newDay)
			newDay = false;
		if (newMonth)
			newMonth = false;
		
		countTime += Parametres.getTimeSpeed ();
		if (countTime >= Parametres.timeOfADay) {
			Parametres.date = Parametres.date.AddDays(1);
			displayDate ();
			countTime -= Parametres.timeOfADay;
			newDay = true;
			if (Parametres.date.Day == 1)
				newMonth = true;
		}
	}

	/**
	 * Mise à jour de l'affichage de la date
	 */
	public void displayDate() {
		textDate.text = Parametres.date.ToString("d MMM yyyy");
	}
}

