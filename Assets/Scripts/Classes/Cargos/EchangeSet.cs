using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EchangeSet {
	/**
	 * Ensemble des échanges d'un pays
	 * Pour avoir accès à un Echange en particulier :
	 * echanges[Pays][typeRessource]
	 * où Pays est une référence sur le Pays emetteur
	 * et typeRessource est le nom de la ressource reçu
	 */
	 public IDictionary<Pays, IDictionary<String, Echange>> echanges;

	/**
	 * Change les paramètres du cargo
	 */
	public EchangeSet()
	{
		this.echanges = new Dictionary<Pays, IDictionary<String, Echange>> ();
	}

	
}
