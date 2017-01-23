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
	 * Valeur de développement des connaissances
	 * Entre 0 (aucune connaissance) et 1 (fortes connaissances)
	 */
	public float developpement;

	public Knowledge (Pays pays, string sujet, int cout) : base(pays)
	{
		sujetKnowledge = sujet;
		coutDeRecherche = cout;
		developpement = 0;
		nom = "Knowledge" + sujet;
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
	public override int receive(Ressource toReceiv)
	{
		Knowledge toReceive = (Knowledge) toReceiv;

		uint tmp = quantity;

		float moyenneQ = ((float)quantity + (float)toReceive.quantity) / 2f;
		float moyenneD = ((float)developpement + (float)toReceive.developpement) / 2f;

		developpement = Mathf.Max(developpement, Utils.tirageNormale (moyenneD, (float)(developpement - toReceive.developpement) / 2f));
		quantity = (uint) Mathf.Max(quantity, Utils.tirageNormale (moyenneQ, (float)(tmp - toReceive.quantity) / 2f));

		return (int)(quantity - tmp);
	}

	public override int consome (bool flag) {
		return 0;
	}

	public override Ressource offre() {
		Knowledge knowledge = new Knowledge (pays, sujetKnowledge, coutDeRecherche);
		knowledge.developpement = developpement;
		return (Ressource) knowledge;
	}
}
