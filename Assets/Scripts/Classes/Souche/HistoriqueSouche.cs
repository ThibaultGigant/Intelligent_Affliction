using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Classe représentant la "mémoire" de la souche, qui va lui permettre de faire des choix
 */
public class HistoriqueSouche
{
	/**
	 * Nombre de personnes infectées par cette souche dans ce pays à l'itération précédente
	 */
	private uint previousNbInfected = 0;
	/**
	 * Dictionnaire contenant les dates d'évolution des compétences de transmission
	 * Clé = mode de transmission
	 * Valeur = liste des dates d'évolution
	 */
	private IDictionary<string, List<DateTime>> datesEvolutionTransmission;
	/**
	 * Dictionnaire contenant les dates d'évolution des compétences de résistance
	 * Clé = mode de résistance
	 * Valeur = liste des dates d'évolution
	 */
	private IDictionary<string, List<DateTime>> datesEvolutionResistance;
	/**
	 * Liste des dates d'évolution de la vitesse d'évolution
	 */
	private List<DateTime> datesEvolutionEvolutionSpeed;
	/**
	 * Dictionnaire contenant les dates d'évolution des symptômes
	 * Clé = symptôme
	 * Valeur = liste des dates d'évolution
	 */
	private IDictionary<string, List<DateTime>> datesEvolutionSymptoms;
	/**
	 * Gradient d'infection des dernières itérations
	 */
	private LimitedList< KeyValuePair<DateTime, uint> > infectionGradient;


	/**
	 * Constructeur
	 */
	public HistoriqueSouche ()
	{

		this.datesEvolutionTransmission = new Dictionary<string, List<DateTime>> ();
		foreach (string s in DonneeSouche.listTransmissionSkills) {
			this.datesEvolutionTransmission.Add (s, new List<DateTime> ());
		}

		this.datesEvolutionResistance = new Dictionary<string, List<DateTime>> ();
		foreach (string s in DonneeSouche.listResistanceSkills) {
			this.datesEvolutionResistance.Add (s, new List<DateTime> ());
		}

		this.datesEvolutionSymptoms = new Dictionary<string, List<DateTime>> ();
		foreach (string s in DonneeSouche.listSymptoms) {
			this.datesEvolutionSymptoms.Add (s, new List<DateTime> ());
		}

		this.datesEvolutionEvolutionSpeed = new List<DateTime> ();

		this.infectionGradient = new LimitedList< KeyValuePair<DateTime, uint> > (1000);
	}

	/**
	 * Retourne le nombre d'infectés qu'il y avait à l'itération précédente
	 * @return Nombre d'infectés à l'itération précédente
	 */
	public uint getPreviousNbInfected()
	{
		return this.previousNbInfected;
	}

	/**
	 * Règle le nombre d'infectés à l'itération précédente à la valeur voulue
	 */
	public void setPreviousNbInfected(uint previous)
	{
		this.previousNbInfected = previous;
	}

	/**
	 * Date de la dernière évolution d'une transmission
	 * Renvoie DateTime.MinValue si on n'a pas encore fait évoluer la transmission
	 */
	public DateTime lastEvolutionTransmission()
	{
		DateTime res = DateTime.MinValue;
		foreach (string s in DonneeSouche.listTransmissionSkills) {
			List<DateTime> liste = this.datesEvolutionTransmission [s];
			if (liste.Count > 0) {
				if (res.Equals(DateTime.MinValue))
					res = liste [liste.Count - 1];
				else if (res.CompareTo(liste[liste.Count - 1]) < 0)
					res = liste[liste.Count - 1];
			}
		}
		return res;
	}

	/**
	 * Date de la dernière évolution de la résistance
	 * Renvoie DateTime.MinValue si on n'a pas encore fait évoluer la résistance
	 */
	public DateTime lastEvolutionResistance()
	{
		DateTime res = DateTime.MinValue;
		foreach (string s in DonneeSouche.listResistanceSkills) {
			List<DateTime> liste = this.datesEvolutionResistance [s];
			if (liste.Count > 0) {
				if (res.Equals(DateTime.MinValue))
					res = liste [liste.Count - 1];
				else if (res.CompareTo(liste[liste.Count - 1]) < 0)
					res = liste[liste.Count - 1];
			}
		}
		return res;
	}

	/**
	 * Date de la dernière évolution de la vitesse d'évolution
	 */
	public DateTime lastEvolutionEvolutionSpeed()
	{
		if (datesEvolutionEvolutionSpeed.Count == 0)
			return DateTime.MinValue;
		return datesEvolutionEvolutionSpeed [datesEvolutionEvolutionSpeed.Count - 1];
	}

	/**
	 * Date de la dernière évolution des symptômes
	 * Renvoie DateTime.MinValue si on n'a pas encore fait évoluer les symptômes
	 */
	public DateTime lastEvolutionSymptomes()
	{
		DateTime res = DateTime.MinValue;
		foreach (string s in DonneeSouche.listSymptoms) {
			List<DateTime> liste = this.datesEvolutionSymptoms [s];
			if (liste.Count > 0) {
				if (res.Equals(DateTime.MinValue))
					res = liste [liste.Count - 1];
				else if (res.CompareTo(liste[liste.Count - 1]) < 0)
					res = liste[liste.Count - 1];
			}
		}
		return res;
	}

	/**
	 * Date et Gradient d'infection du dernier recueil gradient d'infection
	 * @return Dernier gradient d'infection
	 */
	public KeyValuePair<DateTime, uint> lastInfectionGradient()
	{
		if (infectionGradient.Count == 0)
			return new KeyValuePair<DateTime, uint> (DateTime.MinValue, 0);
		return infectionGradient [infectionGradient.Count - 1];
	}

	/**
	 * Calcule le pourcentage d'augmentation du gradient d'infection sur les 10 updates suivant la date passée en paramètres
	 * @param date Date après laquelle on calcule l'augmentation
	 * @return Pourcentage d'augmentation du gradient d'infection suivant cette date
	 */
	public float followingInfectionGradient(DateTime date)
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
		int lastIndex = Mathf.Min (index + 10, infectionGradient.Count - 1);
		return infectionGradient [lastIndex].Value / (float) depart;
	}

	/**
	 * Renvoie la date de la fois où on a augmenté la transmission et ça a grandement augmenté le gradient d'infection
	 * @return Date voulue
	 */
	public DateTime lastBestEvolutionTransmissionDate()
	{
		DateTime res = DateTime.MinValue;
		float bestImprovement = 0;
		float temp;
		List<DateTime> liste;
		foreach (string s in DonneeSouche.listTransmissionSkills) {
			liste = datesEvolutionTransmission [s];
			temp = followingInfectionGradient (liste [liste.Count - 1]);
			if (bestImprovement <= temp) {
				res = liste [liste.Count - 1];
				bestImprovement = temp;
			}
		}
		return res;
	}

	/**
	 * Renvoie la date de la fois où on a augmenté la résistance et ça a grandement augmenté le gradient d'infection
	 * @return Date voulue
	 */
	public DateTime lastBestEvolutionResistanceDate()
	{
		DateTime res = DateTime.MinValue;
		float bestImprovement = 0;
		float temp;
		List<DateTime> liste;
		foreach (string s in DonneeSouche.listResistanceSkills) {
			liste = datesEvolutionResistance [s];
			temp = followingInfectionGradient (liste [liste.Count - 1]);
			if (bestImprovement <= temp) {
				res = liste [liste.Count - 1];
				bestImprovement = temp;
			}
		}
		return res;
	}

	/**
	 * Renvoie la date de la fois où on a augmenté les symptomes et ça a grandement augmenté le gradient d'infection
	 * @return Date voulue
	 */
	public DateTime lastBestEvolutionSymptomsDate()
	{
		DateTime res = DateTime.MinValue;
		float bestImprovement = 0;
		float temp;
		List<DateTime> liste;
		foreach (string s in DonneeSouche.listSymptoms) {
			liste = datesEvolutionSymptoms [s];
			temp = followingInfectionGradient (liste [liste.Count - 1]);
			if (bestImprovement <= temp) {
				res = liste [liste.Count - 1];
				bestImprovement = temp;
			}
		}
		return res;
	}

	/**
	 * Renvoie la propriété (compétence de transmission, résistance, ou un symptome) qui a été augmenté à la date passée en argument
	 * @param date Date où s'est déroulée l'évolution
	 * @return Propriété améliorée à la date passée en paramètres
	 */
	public string getCorrespondingProperty(DateTime date)
	{
		foreach (string s in DonneeSouche.listTransmissionSkills) {
			if (datesEvolutionTransmission [s].Contains (date))
				return s;
		}

		foreach (string s in DonneeSouche.listResistanceSkills) {
			if (datesEvolutionResistance [s].Contains (date))
				return s;
		}

		foreach (string s in DonneeSouche.listSymptoms) {
			if (datesEvolutionSymptoms [s].Contains (date))
				return s;
		}

		if (datesEvolutionEvolutionSpeed.Contains (date))
			return "EvolutionSpeed";

		return null;
	}

	/**
	 * Ajoute une évolution de la transmission dans le sens signifié par la string passée en paramètre
	 * @param s Chaine de caractères donnant le sens de l'évolution
	 * @param date Date de l'évolution
	 */
	public void addTransmissionEvolution(string s, DateTime date)
	{
		this.datesEvolutionTransmission [s].Add (date);
	}

	/**
	 * Ajoute une évolution de la résistance dans le sens signifié par la string passée en paramètre
	 * @param s Chaine de caractères donnant le sens de l'évolution
	 * @param date Date de l'évolution
	 */
	public void addResistanceEvolution(string s, DateTime date)
	{
		this.datesEvolutionResistance [s].Add (date);
	}

	/**
	 * Ajoute une évolution d'un symptôme dans le sens signifié par la string passée en paramètre
	 * @param s Chaine de caractères donnant le sens de l'évolution
	 * @param date Date de l'évolution
	 */
	public void addSymptomEvolution(string s, DateTime date)
	{
		this.datesEvolutionSymptoms [s].Add (date);
	}

	/**
	 * Ajoute une évolution de la vitesse d'évolution
	 * @param date Date de l'évolution
	 */
	public void addEvolutionSpeedEvolution(DateTime date)
	{
		this.datesEvolutionEvolutionSpeed.Add (date);
	}

	/**
	 * Ajoute un gradient d'infection à la date indiquée
	 * @param date Date d'ajout
	 * @param gradient Gradient d'infection à ajouter
	 */
	public void addInfectionGradient (DateTime date, uint gradient)
	{
		this.infectionGradient.Add (new KeyValuePair<DateTime, uint>(date, gradient));
	}
}

