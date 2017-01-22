using UnityEngine;
using System.Collections;

public class Nourriture : Ressource
{

	/**
	 * Constructeur
	 * Nomme la ressource
	 */
	public Nourriture(Pays pays) : base(pays) {
		nom = "Nourriture";
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
		// TODO
		return 0;
	}

	/**
	 * Consommation journalière de nourriture par la population
	 * @param frag Indique si l'on veut une consomation effective,
	 * ou seulement une estimation de la consommation
	 * @return Si flag est vrai, retourne le nombre d'insatisfaits,
	 * sinon, retourne le montant à consommer
	 */
	public override int consome (bool flag) {
		/**
		 * Chaque personne mange environ une "ressource"
		 * [taille de la population] * x
		 * où x est choisi aléatoirement dans [0.95,1.05]
		 */
		int nb = (int) pays.population.totalPopulation * (int) ( 1.05f - Random.value / 10f );

		if (!flag)
			return nb;

		int consommer = pays.population.country.resources ["Nourriture"].removeRessource (nb);

		consommation.Enqueue (consommer);

		int insatisfaits = nb - consommer;

		//Debug.Log ("Nourriture " + nb);

		return insatisfaits;

	}
}