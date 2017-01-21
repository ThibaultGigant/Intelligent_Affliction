using UnityEngine;
using System.Collections;

public class Link
{

	public Pays destinationCountry;

	public Pays originCountry;

	public string type;

	//TODO : garder trace du traffic sous forme de tableaux avec les infos pertinentes

	/**
	 * Constructeur
	 * @param origin Pays d'origine
	 * @param dest Pays de destination
	 * @param type Type de lien (maritime, terestre, aerien)
	 */
	public Link(Pays origin, Pays dest, string type)
	{
		this.originCountry = origin;
		this.destinationCountry = dest;
		this.type = type;
	}
}

