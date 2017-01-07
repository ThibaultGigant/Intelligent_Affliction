using System;

public class Toux : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Toux (int cout)
	{
		coutEvolution = cout;
		this.name = "Toux";
	}

	/**
	 * Altère les skills de la souche
	 */
	public override void affectSkills() {

	}
}

