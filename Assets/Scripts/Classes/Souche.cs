using UnityEngine;
using System.Collections;

public class Souche {

	/**
	 * Nombre de personnes infectées par cette souche dans ce pays
	 */
	private int nbInfected;

	/*
		Les attributs suivants représentent en réalité des pourcentages :
		ce seront des entiers de 0 à 100 (pourcentage d'acquisition de la capacité)
	*/
	/** Résistance au froid de la souche */
	int coldResistance;
	/** Résistance à la chaleur de la souche */
	int heatResistance;
	/** Propagation dans l'air */
	int airPropagation;
	/** Propagation dans l'eau */
	int waterPropagation;
	/** Propagation par contact */
	int contactPropagation;

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
