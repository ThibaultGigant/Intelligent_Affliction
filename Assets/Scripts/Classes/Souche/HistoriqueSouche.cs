using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Classe représentant la "mémoire" de la souche, qui va lui permettre de faire des choix
 */
public class HistoriqueSouche
{
	/**
	 * Souche à laquelle correspond cet historique
	 */
	private Souche souche;
	/**
	 * Liste des dates d'évolution de la transmission
	 */
	private List<DateTime> datesEvolutionTransmission;
	/**
	 * Liste des dates d'évolution de la Resistance
	 */
	private List<DateTime> datesEvolutionResistance;
	/**
	 * Liste des dates d'évolution de la vitesse d'évolution
	 */
	private List<DateTime> datesEvolutionEvolutionSpeed;
	/**
	 * Liste des dates d'évolution de la transmission
	 */
	private List<DateTime> datesEvolutionSymptomes;

	/**
	 * Liste des types de transmission améliorée dans l'ordre
	 */
	private List<String> transmissionEvolutionList;
	/**
	 * Liste des types de résistance améliorée dans l'ordre
	 */
	private List<String> resistanceEvolutionList;
	/**
	 * Liste des types de symptômes amélioré dans l'ordre
	 */
	private List<String> symptomsEvolutionList;
	/**
	 * Gradient d'infection des dernières itérations
	 */
	private LimitedList< KeyValuePair<DateTime, uint> > infectionGradient;


	/**
	 * Constructeur
	 */
	public HistoriqueSouche (Souche souche)
	{
		this.souche = souche;

		this.datesEvolutionTransmission = new List<DateTime> ();
		this.datesEvolutionResistance = new List<DateTime> ();
		this.datesEvolutionEvolutionSpeed = new List<DateTime> ();
		this.datesEvolutionSymptomes = new List<DateTime> ();

		this.transmissionEvolutionList = new List<String> ();
		this.resistanceEvolutionList = new List<String> ();
		this.symptomsEvolutionList = new List<String> ();

		this.infectionGradient = new LimitedList< KeyValuePair<DateTime, uint> > (1000);
	}

	/**
	 * Date de la dernière évolution de la transmission
	 */
	public DateTime lastEvolutionTransmission()
	{
		if (datesEvolutionTransmission.Count == 0)
			return DateTime.Now;
		return datesEvolutionTransmission [datesEvolutionTransmission.Count - 1];
	}

	/**
	 * Date de la dernière évolution de la résistance
	 */
	public DateTime lastEvolutionResistance()
	{
		if (datesEvolutionResistance.Count == 0)
			return DateTime.Now;
		return datesEvolutionResistance [datesEvolutionResistance.Count - 1];
	}

	/**
	 * Date de la dernière évolution de la vitesse d'évolution
	 */
	public DateTime lastEvolutionEvolutionSpeed()
	{
		if (datesEvolutionEvolutionSpeed.Count == 0)
			return DateTime.Now;
		return datesEvolutionEvolutionSpeed [datesEvolutionEvolutionSpeed.Count - 1];
	}

	/**
	 * Date de la dernière évolution des symptômes
	 */
	public DateTime lastEvolutionSymptomes()
	{
		if (datesEvolutionSymptomes.Count == 0)
			return DateTime.Now;
		return datesEvolutionSymptomes [datesEvolutionSymptomes.Count - 1];
	}

	/**
	 * Date et Gradient d'infection du dernier recueil gradient d'infection
	 */
	public KeyValuePair<DateTime, uint> lastInfectionGradient()
	{
		if (infectionGradient.Count == 0)
			return new KeyValuePair<DateTime, uint> (DateTime.Now, 0);
		return infectionGradient [infectionGradient.Count - 1];
	}

	/**
	 * Calcule le pourcentage d'augmentation du gradient d'infection sur les 10 updates suivant la date passée en paramètres
	 * @param date Date après laquelle on calcule l'augmentation
	 * @return Pourcentage d'augmentation du gradient d'infection suivant cette date
	 */
	public float followingInfectionGradientImprovement(DateTime date)
	{
		if (infectionGradient.Count == 0)
			return 0f;
	
		int index = 0;
		// Récupère l'indice de la date dans la liste des gradients d'infection stockée
		while (index < infectionGradient.Count && DateTime.Compare(date, infectionGradient[index].Key) < 0) {
			index++;
		}

		// Si on est arrivé à la fin, c'est qu'il y a un problème, on n'a pas trouvé de date qui correspond
		if (index == infectionGradient.Count)
			return 0f;
		
		uint depart = infectionGradient [index].Value;
		int lastIndex = Mathf.Min (index + 10, infectionGradient.Count);
		return infectionGradient [lastIndex].Value / (float) depart;
	}

}

