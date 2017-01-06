using System;

public class ArretDesOrganes : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public ArretDesOrganes (int cout)
	{
		coutEvolution = cout;
		this.name = "ArretDesOrganes";
	}

	/**
	 * Altère les skills de la souche
	 */
	public override void affectSkills() {

	}
}

