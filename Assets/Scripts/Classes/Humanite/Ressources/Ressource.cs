using UnityEngine;
using System.Collections;

public abstract class Ressource
{
	/**
	 * Quantité possédée de cette ressource
	 */
	public int quantity = 0;
	/**
	 * La ressource est-elle échangeable ?
	 */
	public bool exchangeable;

	public Ressource ()
	{
	}

	/**
	 * Ajout de ressources
	 * @param nb nombre de ressources à rajouter. Doit être positif
	 * @return Le nombre de ressource réellement ajouté
	 */
	public int addRessource(int nb) {
		if (nb < 0)
			return 0;
		quantity += nb;
		return nb;
	}

	/**
	 * Retrait de ressources
	 * @param nb nombre de ressources à enlever. Doit être positif et inférieur à la quantité
	 * @return Le nombre de ressource réellement retiré
	 */
	public int removeRessource(int nb) {
		if (nb < 0)
			return 0;
		
		if (nb > quantity) {
			int temp = quantity;
			quantity = 0;
			return temp;
		}
		quantity -= nb;
		return nb;
	}

	/**
	 * Effectue l'envoi de la ressource à un autre pays.
	 * Retire la quantité de ressource voulu à la ressource
	 * @param toSend Quantité de ressource que l'on doit envoyer
	 * @return Nombre de ressources envoyé
	 */
	public abstract int send (int toSend);

	/**
	 * Effectue la réception d'une ressource d'un autre pays.
	 * Ajoute la quantité de ressource voulu à la ressource
	 * @param toReceive Quantité de ressource que l'on reçoit
	 * @return Nombre de ressources reçu
	 */
	public abstract int receive (int toReceive);
}