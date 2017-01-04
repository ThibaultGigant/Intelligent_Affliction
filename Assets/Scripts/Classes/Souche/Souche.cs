using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Souche {
	/**
	 * Pays que la souche a infecté
	 */
	public Pays country;
	/**
	 * Nombre de personnes infectées par cette souche dans ce pays
	 */
	private uint nbInfected;

	/**
	 * Points qui serviront à évoluer
	 */
	private int evolutionPoints = 0;

	/**
	 * Facteur vitesse d'évolution de la souche
	 */
	private float evolutionSpeed = 0;

	/**
	 * Date de la dernière évolution
	 */
	private DateTime lastEvolutionDate;

	/**
	 * Date de la dernière fois où on a produit des points d'évolution
	 */
	private DateTime lastProductionDate;

	/**
	 * A changer en limitedqueue
	 */
	private int infectionGradient;

	/*
		Les attributs suivants représentent en réalité des pourcentages :
		ce seront des entiers de 0 à 100 (pourcentage d'acquisition de la capacité)
	*/
	/**
	* Ensemble des symptomes de la souche
	*/
	public IDictionary<string, ISymptom> symptoms;
	/**
	 * Ensemble des skills de la souche
	 */
	public Skills skills;

	/**
	 * Constructeur
	 */
	public Souche(Pays pays) {
		country = pays;
		symptoms = new Dictionary<string, ISymptom> ();
		symptoms.Add ("Toux",new Toux(Parametres.coutToux));
		symptoms.Add ("Eternuements",new Eternuements(Parametres.coutEternuements));
		symptoms.Add ("Diarrhee",new Diarrhee(Parametres.coutDiarrhee));
		symptoms.Add ("Sueurs",new Sueurs(Parametres.coutSueurs));
		symptoms.Add ("Fievre",new Fievre(Parametres.coutFievre));
		symptoms.Add ("ArretDesOrganes",new ArretDesOrganes(Parametres.coutArretDesOrganes));
	}

	/**
	 * Retourne le nombre d'infectés du pays
	 * @return Entier donnant ce nombre
	*/
	public uint getNbInfected() 
	{
		return nbInfected;
	}

	/**
	 * Ajout de Personnes infectées
	 * @param toAdd: Nombre de personnes infectées à rajouter. Ce nombre doit être positif et inférieur à la population non-infectée.
	*/
	public void addInfectedPeople(uint toAdd)
	{
		if (toAdd < 0)
			return;
		uint nbNotInfected = country.getNbPopulation () - nbInfected;
		if (nbNotInfected < toAdd)
			nbInfected += nbNotInfected;
		nbInfected += toAdd;
	}

	/**
	 * Suppression de Personnes infectées, probablement soignées
	 * @param toRemove: Nombre de personnes infectées à rajouter. Ce nombre doit être positif et inférieur au nombre de personnes infectées.
	*/
	public void removeInfectedPeople(uint toRemove)
	{
		if (toRemove < 0)
			return;
		if (toRemove < nbInfected)
			nbInfected -= toRemove;
		else
			nbInfected = 0;
	}

	/**
	 * Effectue une évolution de la souche en fonction des motivations et points disponibles.
	 * Inhibition des connaissances de la population
	 */
	public void evolve()
	{
	}

	/**
	 * Production de points d'évolution
	 * En fonction de gradient d'infection et/ou ratio des infectés, vitesse d'évolution
	 */
	public void produce()
	{
	}

	private float motivationTransmission()
	{
		return 0;
	}


	private float motivationResistance()
	{
		return 0;
	}

	private float motivationEvolutionSpeed()
	{
		return 0;
	}

	private float motivationLethality()
	{
		return 0;
	}

	private void evolveTransmission()
	{
	}


	private void evolveResistance()
	{
	}

	private void evolveEvolutionSpeed()
	{
	}

	private void evolveLethality()
	{
	}

	public void fusion(Souche souche, uint nbInfectedComing)
	{
		
	}

	public void contamination()
	{
		// Contamination et meurtre
	}


}
