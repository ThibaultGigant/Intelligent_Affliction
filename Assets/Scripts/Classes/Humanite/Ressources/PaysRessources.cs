using System;
using UnityEngine;
using System.Collections.Generic;

public class PaysRessources
{

	/**
	 * Population possédant ces ressources
	 */
	Population population;

	/**
	 * Le pays rattaché à ces ressources
	 */
	Pays pays;

	/**
	 * Ensemble des Ressource du pays, accessibles par leur nom
	 */
	public IDictionary<string, Ressource> resources;

	public PaysRessources (Pays pays)
	{
		this.pays = pays;
		population = pays.population;
		SetupRessources ();
	}

	/**
	 * Initialisation des ressources du pays
	 */
	private void SetupRessources() {
		resources = new Dictionary<string, Ressource> ();
		resources.Add ("Nourriture", new Nourriture (pays));
		resources.Add ("KnowledgeToux", new Knowledge (pays, "Toux", 1));
		resources.Add ("KnowledgeEternuements", new Knowledge (pays, "Eternuements", 1));
		resources.Add ("KnowledgeFievre", new Knowledge (pays, "Fievre", 1));
		resources.Add ("KnowledgeDiarrhee", new Knowledge (pays, "Diarrhee", 1));
		resources.Add ("KnowledgeSueurs", new Knowledge (pays, "Sueurs", 1));
		resources.Add ("KnowledgeArretDesOrganes", new Knowledge (pays, "ArretDesOrganes", 1));
		//resources.Add ("KnowledgeResistance", new Knowledge (pays, "Resistance", 1));
		//resources.Add ("KnowledgeSpreading", new Knowledge (pays, "Spreading", 1));
		resources.Add ("Transports", new RessourceTransports (pays));
		resources.Add ("Loisirs", new RessourceLoisirs (pays));
	}

	/**
	 * Redéfinition des indexeurs
	 * Rend la class transparente, et se comporte comme un dictionnaire
	 */
	public Ressource this[string name] {
		get {
			return resources [name];
		}
		set {
			resources [name] = value;
		}
	}

	public void consome() {
		foreach (string key in resources.Keys) {
			resources [key].consome (true);
		}
	}
}

