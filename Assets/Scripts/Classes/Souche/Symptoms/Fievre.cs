using System;

public class Fievre : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Fievre (int cout)
	{
		coutEvolution = cout;
		this.name = "Fievre";
	}

	/**
	 * Altère les skills de la souche
	 */
	public override void affectSkills() {

	}
}

