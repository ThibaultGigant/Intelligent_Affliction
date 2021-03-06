﻿using UnityEngine;
using System.Collections;

public class RessourceLoisirs : Ressource {

	public RessourceLoisirs(Pays pays) : base(pays) {
		nom = "Loisirs";
	}

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
	 * @param toReceive Ressource que l'on reçoit
	 * @return Nombre de ressources reçu
	 */
	public override int receive(Ressource toReceive)
	{
		float moyenneQ = ((float)quantity + (float)toReceive.quantity) / 2f;

		uint tmp = quantity;

		quantity = (uint) Mathf.Max(quantity, Utils.tirageNormale (moyenneQ, (float)(tmp - toReceive.quantity) / 2f));

		return (int)(quantity - tmp);
	}

	public override int consome (bool flag) {
		return 0;
	}

	public override Ressource offre() {
		Loisirs loisir = (Loisirs)(pays.population.categories ["Loisirs"]);
		int nb = loisir.offre ((int)quantity, 1f);
		if (nb <= 0)
			return null;

		RessourceLoisirs loisirs = new RessourceLoisirs (pays);
		loisirs.addRessource (nb);
		return (Ressource) loisirs;
	}
}
