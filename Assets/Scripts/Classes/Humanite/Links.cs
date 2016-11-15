using System;
using System.Collections.Generic;
using UnityEngine;

public class Links
{
	/**
	 * Liens terrestres
	 */
	public List<Pays> terrestres;
	/**
	 * Liens aériens
	 */
	public List<Pays> aeriens;
	/**
	 * Liens maritimes
	 */
	public List<Pays> maritimes;

	/**
	 * Constructeur
	 */
	public Links ()
	{
		terrestres = new List<Pays> ();
		aeriens = new List<Pays> ();
		maritimes = new List<Pays> ();
	}

	/**
	 * Ajout d'un lien terrestre
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addTerrestre(Pays pays) {
		if (!terrestres.Contains(pays))
			terrestres.Add (pays);
	}

	/**
	 * Ajout d'un lien aérien
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addAerien(Pays pays) {
		if (!aeriens.Contains(pays))
			aeriens.Add (pays);
	}

	/**
	 * Ajout d'un lien maritime
	 * @param pays Pays avec lequel le lien est créé. Le lien ne doit pas déjà exister
	 */
	public void addMaritime(Pays pays) {
		if (!maritimes.Contains(pays))
			maritimes.Add (pays);
	}

	/**
	 * Suppression d'un lien terrestre
	 * @param pays Pays avec lequel le lien est supprimé. Le lien doit exister
	 */
	public void removeTerrestre(Pays pays) {
		if (terrestres.Contains(pays))
			terrestres.Remove (pays);
	}

	/**
	 * Suppression d'un lien aérien
	 * @param pays Pays avec lequel le lien est supprimé. Le lien doit exister
	 */
	public void removeAerien(Pays pays) {
		if (aeriens.Contains(pays))
			aeriens.Remove (pays);
	}

	/**
	 * Suppression d'un lien maritime
	 * @param pays Pays avec lequel le lien est supprimé. Le lien doit exister
	 */
	public void removeMaritime(Pays pays) {
		if (maritimes.Contains(pays))
			maritimes.Remove (pays);
	}
}

