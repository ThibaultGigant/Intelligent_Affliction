using UnityEngine;
using System.Collections;

public class Knowledge : Ressource {
	/**
	 * Sujet sur lequel la connaissance s'applique (symptôme ou skill)
	 */
	public string sujetKnowledge;
	/**
	 * Coût en point de recherche nécessaire pour acquérir une connaissance de ce sujet
	 */
	public int coutDeRecherche;

	/**
	 * Effectue l'envoi de la ressource à un autre pays.
	 * Retire la quantité de ressource voulu à la ressource
	 * @param toSend Quantité de ressource que l'on doit envoyer
	 * @return Nombre de ressources envoyé
	 */
	public override int send(int toSend)
	{
		// TODO
		return 0;
	}

	/**
	 * Effectue la réception d'une ressource d'un autre pays.
	 * Ajoute la quantité de ressource voulu à la ressource
	 * @param toReceive Quantité de ressource que l'on reçoit
	 * @return Nombre de ressources reçu
	 */
	public override int receive(int toReceive)
	{
		// TODO
		return 0;
	}
}
