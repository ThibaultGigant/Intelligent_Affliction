using UnityEngine;
using System.Collections;

public abstract class Ressource
{
	/**
	 * Quantité possédée de cette ressource
	 * ---
	 * Faut-il un float ?
	 */
	public int quantity = 0;

	public Ressource ()
	{
	}

	/**
	 * Ajout de ressources
	 * @param nb nombre de ressources à rajouter. Doit être positif
	 */
	public void addRessource(int nb) {
		if (nb < 0)
			return;
		quantity += nb;
	}

	/**
	 * Retrait de ressources
	 * @param nb nombre de ressources à enlever. Doit être positif et inférieur à la quantité
	 */
	public void removeRessource(int nb) {
		if (nb < 0)
			return;
		
		if (nb > quantity)
			quantity = 0;
		else
			quantity -= nb;
	}
}