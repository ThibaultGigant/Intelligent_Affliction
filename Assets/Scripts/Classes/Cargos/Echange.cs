using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Echange {
	/**
	 * Type de la ressource à envoyer
	 */
	public String ressourceEnvoye;

	/**
	 * Nombre de ressources à envoyer
	 */
	public uint nbRessourceEnvoye;

	/**
	 * Type de la ressource à recevoir
	 */
	public String ressourceRecu;

	/**
	 * Nombre de ressources à recevoir
	 */
	public uint nbRessourceRecu;

	/**
	 * Lien sur lequel le cargo se déplace
	 */
	public Link link;

	/**
	 * Garde une trace des cent dernières envois
	 */
	public LimitedQueue<uint> historiqueEnvoi;

	/**
	 * Garde une trace des cent dernières receptions
	 */
	public LimitedQueue<uint> historiqueReception;

	private GameObject cargo;

	/**
	 * Change les paramètres du cargo
	 */
	public Echange(	String ressourceEnvoye,	uint nbRessourceEnvoye,
					String ressourceRecu, 	uint nbRessourceRecu,
					Link link)
	{
		this.ressourceEnvoye = ressourceEnvoye;
		this.nbRessourceEnvoye = nbRessourceEnvoye;
		this.ressourceRecu = ressourceRecu;
		this.nbRessourceRecu = nbRessourceRecu;
		this.link = link;
		this.historiqueEnvoi = new LimitedQueue<uint>(100);
		this.historiqueEnvoi = new LimitedQueue<uint>(100);
	}

	/**
	 * Met à jour le nombre de ressource que le pays continu de nous envoyer
	 */
	public void checkEchangeReception(uint montant)
	{
		/**
		 * Si la moyenne des x dernières recepetions diffère de plus du tier de la dernière reception
		 * et que la variance est "assez grande"
		 * On met à jour le nombre de ressource que l'on s'attend à recevoir les porchaines fois à la moyenne
		 * calculée
		 * TODO : définir x et "assez grand"
		 */
		Couple<float, float> moyVar = Utils.moyenneVariance(historiqueReception.ToArray());
		uint consommation = (uint) link.originCountry.resources [ressourceRecu].consome (false);
		if ( Mathf.Abs(moyVar.first - montant) > montant / 3.0
			&& Mathf.Sqrt(moyVar.second) >  montant / 3.0) {
			nbRessourceRecu = (uint) moyVar.first;
		}
	}

	public void effectuerEchange() {
		Pays origin = link.originCountry;
		Ressource send = origin.resources [ressourceEnvoye].offre ();

		send.quantity = (uint)Mathf.Min ((float)nbRessourceRecu, (float)send.quantity * 0.5f);

		origin.resources [ressourceEnvoye].quantity -= send.quantity;

		cargo = GameObject.Instantiate(Parametres.prefabCargo, new Vector3(90f, 30f, -10f), Quaternion.identity);
		cargo.GetComponentInChildren<Cargo> ();
		Cargo c = cargo.GetComponentInChildren<Cargo>();
		c.resources.Add (send.nom, send);
		c.go ();
	}
}
