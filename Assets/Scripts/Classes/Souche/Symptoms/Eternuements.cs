using System;

public class Eternuements : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Eternuements (int cout)
	{
		coutEvolution = cout;
		this.name = "Eternuements";
	}

	/**
	 * Altère les skills de la souche
	 */
	public override void affectSkills() {

	}
}

