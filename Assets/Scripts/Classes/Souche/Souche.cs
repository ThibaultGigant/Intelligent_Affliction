﻿using UnityEngine;
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
	private uint nbInfected = 0;

	/**
	 * Points qui serviront à évoluer
	 */
	private int evolutionPoints = 0;

	/**
	 * Facteur vitesse d'évolution de la souche
	 * Influe sur le nombre de points gagnés lors de la production
	 */
	private float evolutionSpeed = 1;

	/**
	 * Historique des évolutions de la souche, pour le calcul des motivations
	 */
	private HistoriqueSouche historique;



	/*
		Les attributs suivants représentent en réalité des pourcentages :
		ce seront des entiers de 0 à 100 (pourcentage d'acquisition de la capacité)
	*/
	/**
	* Ensemble des symptomes de la souche
	*/
	public IDictionary<string, AbstactSymptom> symptoms;
	/**
	 * Ensemble des skills de la souche
	 */
	public Skills skills;

	/**
	 * Constructeur
	 */
	public Souche(Pays pays) {
		country = pays;

		symptoms = new Dictionary<string, AbstactSymptom> ();

		historique = new HistoriqueSouche ();
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
	 * Suppression de Personnes infectées, probablement soignées ou tuées par la maladie
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
	 * En fonction de gradient d'infection et du ratio des infectés, vitesse d'évolution
	 */
	public void produce()
	{
		this.evolutionPoints += Mathf.FloorToInt (1 * evolutionSpeed);
	}

	/**
	 * Indicateur de la volonté de la souche d'augmenter sa faculté de transmission
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution de ce caractère
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- grad : Augmentation du gradient d'infection après la dernière évolution de ce caratère
	 * 	- val : La valeur actuelle de la compétence de la souche, 
	 * 		Cette dernière prend en compte les effets des symptomes de la souche, ainsi que de la recherche et medecine des humains
	 * On suit la formule : duree/100 * ((1 - pourc)*5 + (1 / grad)*2) * (100 - val)
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationTransmission(DateTime date)
	{
		DateTime last = historique.lastBestEvolutionTransmissionDate();
		double duree = date.Subtract(last).TotalSeconds;
		float grad = (float) historique.lastInfectionGradient ().Value;
		// Si le gradient est négatif, on perd des amis, il faut augmenter la volonté de transmission
		if (grad < 0)
			grad = - grad / 100;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();
		float val = this.skills.getSkillValue (this.historique.getCorrespondingProperty (last));
		return (float) (duree / 100) * (5 * (1 - pourc) + 3 * (1 / grad)) * (100 - val);
	}
		
	/**
	 * Indicateur de la volonté de la souche d'augmenter sa faculté de résistance
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution de ce caractère
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- grad : Augmentation du gradient d'infection après la dernière évolution de ce caratère
	 * 	- val : La valeur actuelle de la compétence de la souche, 
	 * 		Cette dernière prend en compte les effets des symptomes de la souche, ainsi que de la recherche et medecine des humains
	 * On suit la formule : duree/100 * ((1 - pourc) + 5*(1/grad)) * (100 - val)
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationResistance(DateTime date)
	{
		DateTime last = historique.lastBestEvolutionResistanceDate();
		double duree = date.Subtract(last).TotalSeconds;
		float grad = (float) historique.lastInfectionGradient ().Value;
		// Si le gradient est négatif, on perd des amis, il faut augmenter la volonté de résistance
		if (grad < 0)
			grad = - grad / 5;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();
		float val = this.skills.getSkillValue (this.historique.getCorrespondingProperty (last));
		return (float) (duree / 100) * ((1 - pourc) + 5 * (1 / grad)) * (100 - val);
	}
		
	/**
	 * Indicateur de la volonté de la souche d'augmenter sa vitesse d'évolution
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution de ce caractère
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- speed : la vitesse actuelle
	 * 	- val : le nombre de points d'évolution que l'on a actuellement
	 * On suit la formule : duree/100 * (1-pourc)^3 * (val / speed)
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationEvolutionSpeed(DateTime date)
	{
		DateTime last = historique.lastEvolutionEvolutionSpeed();
		double duree = date.Subtract(last).TotalSeconds;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();

		return (float) (duree / 100) * Mathf.Pow((1 - pourc), 3) * (this.evolutionPoints / this.evolutionSpeed);
	}

	/**
	 * Indicateur de la volonté de la souche d'augmenter la "puissance" de l'un de ses symptômes, et donc sa létalité
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution d'un symptôme
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- val : Nombre de symptômes restant à déveloper
	 * On suit la formule : duree/100 * (5 * pourc) * val
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationLethality(DateTime date)
	{
		DateTime last = historique.lastEvolutionEvolutionSpeed();
		double duree = date.Subtract(last).TotalSeconds;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();

		return (float) (duree / 100) * 5 * pourc * (DonneeSouche.listSymptoms.Count - this.symptoms.Count);
	}

	/**
	 * Choix du type de transmission à améliorer et application de l'évolution de cette transmission
	 */
	private void evolveTransmission()
	{
	}

	/**
	 * Choix du type de résistance à améliorer et application de l'évolution de cette résistance
	 */
	private void evolveResistance()
	{
	}

	/**
	 * Application de l'augmentation de la vitesse d'évolution
	 */
	private void evolveEvolutionSpeed()
	{
	}

	/**
	 * Choix du type de transmission à améliorer et application de l'évolution vers la transmission
	 */
	private void evolveLethality()
	{
	}

	/**
	 * Application de la fusion entre une souche arrivante, et la souche actuelle.
	 * @param souche Souche qui arrive avec les migrants
	 * @param nbInfectedComing Nombre de migrants qui arrivent avec cette souche : Influencera l'impact des caractéristiques de la souche arrivante sur la souche courante
	 */
	public void fusion(Souche souche, uint nbInfectedComing)
	{
		
	}

	/**
	 * Application de la contamination de nouveaux habitants et de la mort de certains d'entre eux si c'est nécessaire
	 */
	public void contamination()
	{
		// Contamination et meurtre
	}


}
