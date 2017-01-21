using UnityEngine;
using System.Collections;
using System;

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
	/**
	 *
	 */
	public String nom;

	/**
	 * Pays auquel est rattaché la ressource
	 */
	public Pays pays;

	/**
	 * Les class qui héritières doivent
	 * se nommer en initialisant "nom"
	 */
	public Ressource (Pays pays)
	{
		this.pays = pays;
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
	 * @param toReceive Ressource reçue
	 * @return Nombre de ressources reçu
	 */
	public abstract int receive (Ressource toReceive);

	/**
	 * Consommation journalière de ressources
	 * @param flag Indique si l'on souhaite une consommation effective,
	 * ou seulement une estimation de la consommation
	 * @return le montant à consommer
	 */
	public abstract int consome (bool flag);
}