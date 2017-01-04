using UnityEngine;
using System.Collections;

public class Link
{

	public Pays destinationCountry;

	public string type;

	//TODO : garder trace du traffic sous forme de tableaux avec les infos pertinentes

	/**
	 * Constructeur
	 */
	public Link(Pays dest, string t)
	{
		this.destinationCountry = dest;
		this.type = t;
	}
}

