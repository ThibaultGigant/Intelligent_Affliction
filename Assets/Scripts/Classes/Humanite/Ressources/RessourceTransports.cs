using UnityEngine;
using System.Collections;

public class RessourceTransports : Ressource {
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
