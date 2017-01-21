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
	public void addTerrestre(Pays pays) {
		if (!terrestres.ContainsKey(pays.nomPays))
			terrestres.Add (pays.nomPays, new Link(country, pays, "Terrestre"));
	}

	/**
	 * Ajout d'un lien aérien
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addAerien(Pays pays) {
		if (!aeriens.ContainsKey(pays.nomPays))
			aeriens.Add (pays.nomPays, new Link(country, pays, "Aerien"));
	}

	/**
	 * Ajout d'un lien maritime
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addMaritime(Pays pays) {
		if (!maritimes.ContainsKey(pays.nomPays))
			maritimes.Add (pays.nomPays, new Link(country, pays, "Maritime"));
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

