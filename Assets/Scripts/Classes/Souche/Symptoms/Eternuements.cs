﻿using System;

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
		this.lethalityIndex = 1.1f;
	}

	/**
	 * Renvoie un facteur influençant la puissance du symptôme
	 * Dépend des connaissances accumulées sur le symptôme et du pourcentage de la population assignée à la médecine
	 * @return facteur d'atténuation du symptôme
	 */
	public override float getAffectedByHumans()
	{
		return computeHumanEffectOnSymptom ("KnowledgeEternuements");
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
		return 1.1f * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par l'air de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillAirSpreading()
	{
		return 3.5f * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par le contact de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public override float affectSkillContactSpreading()
	{
		return 2f * getAffectedByHumans();
	}
}

