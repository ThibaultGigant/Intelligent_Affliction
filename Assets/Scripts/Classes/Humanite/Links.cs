using System;
using System.Collections.Generic;
using UnityEngine;

public class Links
{
	/**
	 * Pays d'origine de l'ensemble des liens
	 */
	public Pays country;

	/**
	 * Liens terrestres
	 */
	private IDictionary<string, Link> terrestres;
	/**
	 * Liens aériens
	 */
	private IDictionary<string, Link> aeriens;
	/**
	 * Liens maritimes
	 */
	private IDictionary<string, Link> maritimes;

	/**
	 * Constructeur
	 */
	public Links ()
	{
		terrestres = new Dictionary<string, Link> ();
		aeriens = new Dictionary<string, Link> ();
		maritimes = new Dictionary<string, Link> ();
	}

	/**
	 * Ajout d'un lien terrestre
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public Link addTerrestre(Pays pays) {
		Link newLink = null;
		if (!terrestres.ContainsKey(pays.nomPays)) {
			newLink = new Link (country, pays, "Terrestre");
			terrestres.Add (pays.nomPays, newLink);
		}
		return newLink;
	}

	/**
	 * Ajout d'un lien aérien
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public Link addAerien(Pays pays) {
		Link newLink = null;
		if (!aeriens.ContainsKey(pays.nomPays)) {
			newLink = new Link (country, pays, "Aerien");
			aeriens.Add (pays.nomPays, newLink);
		}
		return newLink;
	}

	/**
	 * Ajout d'un lien maritime
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public Link addMaritime(Pays pays) {
		Link newLink = null;
		if (!maritimes.ContainsKey(pays.nomPays)) {
			newLink = new Link (country, pays, "Maritime");
			maritimes.Add (pays.nomPays, newLink);
		}
		return newLink;
	}

	/**
	 * Suppression d'un lien terrestre
	 * @param pays Pays avec lequel le lien est supprimé. Le lien doit exister
	 */
	public void removeTerrestre(Pays pays) {
		if (terrestres.ContainsKey(pays.nomPays))
			terrestres.Remove (pays.nomPays);
	}

	/**
	 * Suppression d'un lien aérien
	 * @param pays Pays avec lequel le lien est supprimé. Le lien doit exister
	 */
	public void removeAerien(Pays pays) {
		if (aeriens.ContainsKey(pays.nomPays))
			aeriens.Remove (pays.nomPays);
	}

	/**
	 * Suppression d'un lien maritime
	 * @param pays Pays avec lequel le lien est supprimé. Le lien doit exister
	 */
	public void removeMaritime(Pays pays) {
		if (maritimes.ContainsKey(pays.nomPays))
			maritimes.Remove (pays.nomPays);
	}
}

