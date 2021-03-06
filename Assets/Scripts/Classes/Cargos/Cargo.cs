﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cargo : MonoBehaviour {
	/**
	 * Nombre de personnes embarquées dans le cargo
	 */
	public uint nbPersonnes;

	/**
	 * Nombre de personnes migrantes infectées transportées par le cargo
	 */
	public uint nbInfected;

	/**
	 * Nombre de touristes infectés, pas comptabilisés dans le nombre de personnes à ajouter au nouveau pays
	 */
	public uint nbInfectedTourists;

	/**
	 * Souche éventuellement transportée par les personnes infectées
	 */
	public Souche souche;

	/**
	 * Lien sur lequel le cargo se déplace
	 */
	public Link link;

	/**
	 * Pourcentage de la route déjà traversée par le cargo
	 */
	public float advancement = 0f;

	/**
	 * Ensemble des Ressource transportées, accessibles par leur nom
	 */
	public IDictionary<string, Ressource> resources;

	private bool onGoing = false;

	// Use this for initialization
	void Awake () {
		resources = new Dictionary<string, Ressource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ClockManager.newDay && onGoing) {
			advancement++;
			if (advancement >= 1) {
				dechargement();
			}
		}
	}

	/**
	 * Change les paramètres du cargo
	 */
	public void setParameters(uint nbPersonnes, uint nbInfected, uint nbInfectedTourists, Link link, IDictionary<string, Ressource> resources, Souche souche)
	{
		this.nbPersonnes = nbPersonnes;
		this.nbInfected = nbInfected;
		this.link = link;
		this.resources = resources;
		this.souche = souche;
		this.nbInfectedTourists = nbInfectedTourists;
	}

	/**
	 * Application du déchargement
	 */
	public void dechargement()
	{
		Pays country = link.destinationCountry;

		// Fusion des souches
		if (this.souche != null)
			country.createSouche(this.souche, this.nbInfected + this.nbInfectedTourists);

		// Ajout des ressources
		foreach (KeyValuePair<string, Ressource> entry in resources)
		{
			Debug.Log ("Cargo dechargement " + entry.Key);
			country.resources [entry.Key].receive (entry.Value);
			// Mise à jour des contrats d'échanges
			if (country.echangeSet.echanges [link.originCountry].Keys.Contains (entry.Key)) {
				Echange echange = country.echangeSet.echanges [link.originCountry] [entry.Key];
				echange.historiqueReception.Enqueue ((uint)entry.Value.quantity);

				echange.checkEchangeReception((uint)entry.Value.quantity);
			}

		}

		// Ajout des migrants
		country.addPeople(this.nbPersonnes);
		country.addInfectedPeople (this.nbInfected);

		Destroy (gameObject);

	}

	public void go() {
		onGoing = true;
	}
}
