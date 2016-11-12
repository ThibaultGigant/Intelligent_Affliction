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
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		countTime += Parametres.getTimeSpeed ();
		if (countTime >= 100) {
			Parametres.date.Tomorow ();
			displayDate ();
			countTime -= 100;
		}
	}

	private void displayDate() {
		textDate.text = Parametres.date.getJour() + "/" + Parametres.date.getMois() + "/" + Parametres.date.getAnnee();
	}
}

