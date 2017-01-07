using System;

public class Sueurs : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Sueurs (int cout)
	{
		coutEvolution = cout;
		this.name = "Sueurs";
	}

	/**
	 * Altère les skills de la souche
	 */
	public override void affectSkills() {

	}
}

