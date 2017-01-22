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
		this.lethalityIndex = DonneeSouche.lethalitySymptomes[this.name];
		this.detectableIndex = DonneeSouche.detectabilitySymptomes[this.name];
	}

	/**
	 * Renvoie un facteur influençant la puissance du symptôme
	 * Dépend des connaissances accumulées sur le symptôme et du pourcentage de la population assignée à la médecine
	 * @return facteur d'atténuation du symptôme
	 */
	public override float getAffectedByHumans()
	{
		return computeHumanEffectOnSymptom ("KnowledgeSueurs");
	}

	/**
	 * Altère la résistance au froid de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillResistanceCold()
	{
		return 1f * getAffectedByHumans();
	}

	/**
	 * Altère la résistance à la chaleur de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillResistanceHeat()
	{
		return 1f * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par l'eau de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillWaterSpreading()
	{
		return 1.2f * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par l'air de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillAirSpreading()
	{
		return 1f * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par le contact de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillContactSpreading()
	{
		return 2.5f * getAffectedByHumans();
	}
}

