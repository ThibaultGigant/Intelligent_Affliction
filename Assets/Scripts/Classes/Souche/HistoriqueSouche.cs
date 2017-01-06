using System;
using System.Collections.Generic;

/**
 * Classe représentant la "mémoire" de la souche, qui va lui permettre de faire des choix
 */
public class HistoriqueSouche
{
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
	 * A changer en limitedqueue
	 */
	private LimitedQueue<int> infectionGradient;

	/**
	 * Constructeur
	 */
	public HistoriqueSouche ()
	{
		datesEvolutionTransmission = new List<DateTime> ();
		datesEvolutionResistance = new List<DateTime> ();
		datesEvolutionEvolutionSpeed = new List<DateTime> ();
		datesEvolutionSymptomes = new List<DateTime> ();

		transmissionEvolutionList = new List<String> ();
		resistanceEvolutionList = new List<String> ();
		symptomsEvolutionList = new List<String> ();

		infectionGradient = new LimitedQueue<int> (1000);
	}
}

