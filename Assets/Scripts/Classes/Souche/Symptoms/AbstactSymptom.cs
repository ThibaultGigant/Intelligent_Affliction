using System;

public abstract class AbstactSymptom
{
	/**
	 * Nom du symptôme
	 */
	protected String name;

	/**
	 * Coût de l'évolution du symptome
	 */
	public int coutEvolution {
		get { return coutEvolution; }
		set { coutEvolution = value; }
	}

	/**
	 * Retourne le nom du symptome
	 */
	public String getName()
	{
		return this.name;
	}

	/**
	 * Altère les skills de la souche
	 */
	public abstract void affectSkills();

}

