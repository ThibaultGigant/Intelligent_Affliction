using UnityEngine;
using System.Collections;

public class Link
{

	public Pays destinationCountry;

	public Pays originCountry;

	public string type;

	public int taille;

	//TODO : garder trace du traffic sous forme de tableaux avec les infos pertinentes

	/**
	 * Constructeur
	 * @param origin Pays d'origine
	 * @param dest Pays de destination
	 * @param type Type de lien (maritime, terestre, aerien)
	 * @param taille Représente la capacité maximale que peut porter les
	 * cargos voyageant par ce lien
	 */
	public Link(Pays origin, Pays dest, string type, int taille)
	{
		this.originCountry = origin;
		this.destinationCountry = dest;
		this.type = type;
		this.taille = taille;
	}

	/**
	 * Constructeur
	 * @param origin Pays d'origine
	 * @param dest Pays de destination
	 * @param type Type de lien (maritime, terestre, aerien)
	 * @param taille Représente la capacité maximale que peut porter les
	 * cargos voyageant par ce lien
	 */
	public Link(Pays origin, Pays dest, string type)
	{
		this.originCountry = origin;
		this.destinationCountry = dest;
		this.type = type;
		this.taille = 100;
	}
}

