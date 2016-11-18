using System;
using System.Collections.Generic;
using UnityEngine;

public class Links
{
	/**
	 * Liens terrestres
	 */
	private IDictionary<string, Pays> terrestres;
	/**
	 * Liens aériens
	 */
	private IDictionary<string, Pays> aeriens;
	/**
	 * Liens maritimes
	 */
	private IDictionary<string, Pays> maritimes;

	/**
	 * Constructeur
	 */
	public Links ()
	{
		terrestres = new Dictionary<string, Pays> ();
		aeriens = new Dictionary<string, Pays> ();
		maritimes = new Dictionary<string, Pays> ();
	}

	/**
	 * Ajout d'un lien terrestre
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addTerrestre(Pays pays) {
		if (!terrestres.ContainsKey(pays.nomPays))
			terrestres.Add (pays.nomPays, pays);
	}

	/**
	 * Ajout d'un lien aérien
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addAerien(Pays pays) {
		if (!aeriens.ContainsKey(pays.nomPays))
			aeriens.Add (pays.nomPays, pays);
	}

	/**
	 * Ajout d'un lien maritime
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addMaritime(Pays pays) {
		if (!maritimes.ContainsKey(pays.nomPays))
			maritimes.Add (pays.nomPays, pays);
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

