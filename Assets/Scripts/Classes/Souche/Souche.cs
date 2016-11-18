﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Souche {

	/**
	 * Nombre de personnes infectées par cette souche dans ce pays
	 */
	private int nbInfected;

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
	public Souche() {
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
	public int getNbInfected() 
	{
		return nbInfected;
	}

	/**
	 * Ajout de Personnes infectées
	 * @param toAdd: Nombre de personnes infectées à rajouter. Ce nombre doit être positif.
	*/
	public void addInfectedPeople(int toAdd)
	{
		if (toAdd < 0)
			return;
		nbInfected += toAdd;
	}

	/**
	 * Suppression de Personnes infectées, probablement soignées
	 * @param toRemove: Nombre de personnes infectées à rajouter. Ce nombre doit être positif et inférieur au nombre de personnes infectées.
	*/
	public void removeInfectedPeople(int toRemove)
	{
		if (toRemove < 0)
			return;
		if (toRemove < nbInfected)
			nbInfected -= toRemove;
		else
			nbInfected = 0;
	}
}
