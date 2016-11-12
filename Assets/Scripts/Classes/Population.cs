using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Population {

	/**
	 * Variables publiques
	 * ---
	 * • totalPopulation			: Population totale du pays
	 * • superficie					: Superficie du pays
	 * • nomPays					: Nom du pays
	 * 
	 * Variables privées
	 * ---
	 * • happinessIndex			: "Contentement" de la population
	 * • repartitionRecherche		: Pourcentage de la population assignée à la Recherche
	 * • repartitionMedecine		: Pourcentage de la population assignée à la Médecine
	 * • repartitionAgriculture	: Pourcentage de la population assignée à l'agriculture
	 * • repartitionTransports		: Pourcentage de la population assignée aux transports
	 * • repartitionLoisirs			: Pourcentage de la population assignée aux loisirs
	 */
	public int totalPopulation;             // Population totale du pays
	public float superficie;
	public string nomPays;
	public Pays pays;

	private int happinessIndex;             // "Contentement" de la population
	private int repartitionRecherche;       // Pourcentage de la population assignée à la Recherche
	private int repartitionMedecine;        // Pourcentage de la population assignée à la Médecine
	private int repartitionAgriculture;     // Pourcentage de la population assignée à l'agriculture
	private int repartitionTransports;      // Pourcentage de la population assignée aux transports
	private int repartitionLoisirs;         // Pourcentage de la population assignée aux loisirs

	private string[] assignementActivites = { "Recherche", "Medecine", "Agriculture", "Transports", "Loisirs" };
	private Dictionary<string,float> repartitionActivitesPopulation;

	/**
	 * repartitionAge
	 * ---
	 * Répartition de la population en fonction de leur age
	 * • bebe				: 0 - 4 ans
	 * • enfant			: 5 - 18 ans
	 * • fleur de l'age	: 19 - 26 ans
	 * • adulte			: 27 - 49 ans
	 * • age				: 50 - xxx
	 */
	private string[] repartitionAge = { "bebe", "enfant", "fleur de l'age", "adulte", "age"};
	private int[] trancheAge = {0, 5, 19, 27, 50};
	private Dictionary<string,int> repartitionAgePopulation;
	private Date oldDate;

	public Population(Pays p, float s, string n) {
		pays = p;
		superficie = s;
		nomPays = n;

		totalPopulation = pays.getInitialNumberPopulation ();
		SetupRepartition ();
		oldDate = Parametres.date.copy ();
	}

	private void SetupRepartition() {
		if (totalPopulation <= 0) {
			Debug.LogError (nomPays + ", Repartition : Le nombre de population doit être positif");
			totalPopulation = 0;
		}

		repartitionActivitesPopulation = new Dictionary<string, float> ();

		int nbReparti = totalPopulation / assignementActivites.Length;
		for ( int i = 0 ; i < assignementActivites.Length - 1 ; i++ )
			repartitionActivitesPopulation.Add(assignementActivites [i], nbReparti);

		// Pour eviter les imprécisions du à la discrétisation des pourcentages
		repartitionActivitesPopulation.Add (assignementActivites[assignementActivites.Length - 1], totalPopulation - (assignementActivites.Length - 1) * nbReparti);

		repartitionAgePopulation = new Dictionary<string, int> ();

		nbReparti = totalPopulation / repartitionAge.Length;
		for (int i = 0; i < repartitionAge.Length - 1; i++)
			repartitionAgePopulation.Add (repartitionAge [i], nbReparti);

		// Pour eviter les imprécisions du à la discrétisation des pourcentages
		repartitionAgePopulation.Add (repartitionAge[repartitionAge.Length - 1], totalPopulation - (repartitionAge.Length - 1) * nbReparti);
	}

	/**
	 * ecouleTemps
	 * ---
	 * Fait vieillir la population
	 * Réorganise l'assignement des habitants
	 * Prend en compte la vitesse de simulation
	 */
	public void ecouleTemps () {
		// Appelé une fois par jour, pour l'instant
		if (!Parametres.date.Equals(oldDate)) {
			//repartitionAgeToString ();
			int nbNaissances = naissances ();
			int nbDeces = deces ();
			int nbDecesVieillesse = vieillissement ();
			oldDate = Parametres.date.copy ();
		}

	}

	private int naissances() {
		int nbNaissances = 0;

		//0.03 * (repartitionAgePopulation ["fleur de l'age"] + repartitionAgePopulation ["adulte"]);

		// Nombre de naissance "fix" en fonction du nombre de jeune adulte et d'adulte.
		// 3% de leur nombre par année
		nbNaissances = Mathf.Max(0, Mathf.FloorToInt(0.03f / 12f * (repartitionAgePopulation ["fleur de l'age"] + repartitionAgePopulation ["adulte"])));
		totalPopulation += nbNaissances;
		return nbNaissances;
	}

	private int deces() {
		int nbDeces = 0;

		// Nombre de décès "fix" en fonction de la population totale
		// 0.9% de leur nombre par année
		nbDeces = Mathf.Max(0, Mathf.FloorToInt(0.009f / 12f * totalPopulation));
		totalPopulation -= nbDeces;
		return nbDeces;
	}

	private int vieillissement() {
		int nbDeces = 0;

		int quotient;
		int deplacement;

		for (int i = 0; i < repartitionAge.Length - 1; i++) {
			quotient = trancheAge [i + 1] - trancheAge [i];
			deplacement = repartitionAgePopulation [repartitionAge [i]] / quotient;
			repartitionAgePopulation[repartitionAge[i+1]] += deplacement;
			repartitionAgePopulation [repartitionAge [i]] -= deplacement;
		}

		// Deces des personnes agées, fix, 10%
		nbDeces = Mathf.Max(0, Mathf.FloorToInt(0.01f / 12f * totalPopulation));
		repartitionAgePopulation [repartitionAge [repartitionAge.Length - 1]] -= nbDeces;

		totalPopulation -= nbDeces;

		return nbDeces;
	}

	public void repartitionAgeToString() {
		for (int i = 0; i < repartitionAge.Length ; i++) {
			Debug.Log(repartitionAge [i] + " " + repartitionAgePopulation [repartitionAge [i]]);
		}
	}
}
