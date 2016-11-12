using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockManager : MonoBehaviour
{
	public Text textDate;

	private int countTime = 0;

	// Use this for initialization
	void Start ()
	{
		displayDate ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		countTime += Parametres.getTimeSpeed ();
		if (countTime >= Parametres.timeOfADay) {
			Parametres.date.Tomorow ();
			displayDate ();
			countTime -= Parametres.timeOfADay;
		}
	}

	private void displayDate() {
		textDate.text = Parametres.date.getJour() + "/" + Parametres.date.getMois() + "/" + Parametres.date.getAnnee();
	}
}

