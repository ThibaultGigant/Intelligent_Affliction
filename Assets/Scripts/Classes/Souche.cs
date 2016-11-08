using UnityEngine;
using System.Collections;

public class Souche {

	private int nbInfected;     // Nombre de personnes infectées par cette souche dans ce pays

	/*
		Les attributs suivants représentent en réalité des pourcentages :
		ce seront des entiers de 0 à 100 (pourcentage d'acquisition de la capacité)
	*/
	int coldResistance;         // Résistance au froid de la souche
	int heatResistance;         // Résistance à la chaleur de la souche
	int airPropagation;         // Propagation dans l'air
	int waterPropagation;       // Propagation dans l'eau
	int contactPropagation;     // Propagation par contact

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
	 * @param toAdd: Nombre de personnes infectées à rajouter
	*/
	public void addInfectedPeople(int toAdd)
	{
		nbInfected += toAdd;
	}

	/**
	 * Suppression de Personnes infectées, probablement soignées
	 * @param toRemove: Nombre de personnes infectées à rajouter
	*/
	public void removeInfectedPeople(int toRemove)
	{
		nbInfected -= toRemove;
	}
}
