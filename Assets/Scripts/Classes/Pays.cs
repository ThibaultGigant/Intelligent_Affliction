using System;
using UnityEngine;

public class Pays : MonoBehaviour
{
	public int initialNumberPopulation;
	public int superficie;

	private string nomPays;

	private Population population;

	public void Start ()
	{
		nomPays = gameObject.name;
		population = new Population(this, superficie, nomPays);
	}

	public int getInitialNumberPopulation() { return initialNumberPopulation; }

	public void Update() {
		population.ecouleTemps ();
	}
}

