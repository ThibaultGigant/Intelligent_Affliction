using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skills {
	/**
	 * Souche dont on décrit les compétences
	 */
	Souche souche;
	/**
	 * Résistance au froid
	 */
	public int resistanceToCold;
	/**
	 * Résistance à la chaleur
	 */
	public int resistanceToHeat;
	/**
	 * Habileté de propagation par les eaux
	 */
	public int waterSpreading;
	/**
	 * Habileté de propagation par les airs
	 */
	public int airSpreading;
	/**
	 * Habileté de propagation par le contact
	 */
	public int contactSpreading;

	public Skills(Souche souche)
	{
		this.souche = souche;
		this.resistanceToCold = 5;
		this.resistanceToHeat = 5;
		this.waterSpreading = 5;
		this.airSpreading = 5;
		this.contactSpreading = 5;
	}

	/**
	 * Calcul de l'effet qu'ont les connaissances des humains et la population assignée à la médecine sur la capacité dont la connaissance liée est passée en paramètre
	 * @param skillKnowledge Connaissance associée à la capacité qu'on étudie
	 * @return Facteur d'atténuation dû aux humains
	 */
	public float computeHumanEffectOnSkill(string skillKnowledge)
	{
		return (1 + this.souche.country.resources [skillKnowledge].getQuantity ()) * this.souche.country.getPourcentageCategory ("Medecine") * 10;
	}

	/**
	 * Retourne la résistance au froid calculée pour cette souche
	 * On prend en compte les facteurs multiplicateurs des symptomes
	 */
	public int getResistanceCold ()
	{
		float res = this.resistanceToCold;
		res -= computeHumanEffectOnSkill("KnowledgeResistance");

		foreach (KeyValuePair<string, AbstactSymptom> symptom in this.souche.symptoms) {
			res *= symptom.Value.affectSkillResistanceCold();
		}
		return Mathf.Clamp (Mathf.FloorToInt (res), 0, 100);
	}

	/**
	 * Retourne la résistance à la chaleur calculée pour cette souche
	 * On prend en compte les facteurs multiplicateurs des symptomes
	 */
	public int getResistanceHeat ()
	{
		float res = this.resistanceToHeat;
		res -= computeHumanEffectOnSkill("KnowledgeResistance");
		foreach (KeyValuePair<string, AbstactSymptom> symptom in this.souche.symptoms) {
			res *= symptom.Value.affectSkillResistanceHeat();
		}
		return Mathf.Clamp (Mathf.FloorToInt (res), 0, 100);
	}

	/**
	 * Retourne la transmission par l'eau calculée pour cette souche
	 * On prend en compte les facteurs multiplicateurs des symptomes
	 */
	public int getWaterSpreading ()
	{
		float res = this.waterSpreading;
		res -= computeHumanEffectOnSkill("KnowledgeSpreading");
		res += (this.souche.country.climat.humidite - 50) / 2;

		foreach (KeyValuePair<string, AbstactSymptom> symptom in this.souche.symptoms) {
			res *= symptom.Value.affectSkillWaterSpreading();
		}

		return Mathf.Clamp (Mathf.FloorToInt (res), 0, 100);
	}

	/**
	 * Retourne la transmission par l'air calculée pour cette souche
	 * On prend en compte les facteurs multiplicateurs des symptomes
	 */
	public int getAirSpreading ()
	{
		float res = this.airSpreading;
		res -= computeHumanEffectOnSkill("KnowledgeSpreading");
		res += (this.souche.country.climat.chaleur - 50) / 2;

		foreach (KeyValuePair<string, AbstactSymptom> symptom in this.souche.symptoms) {
			res *= symptom.Value.affectSkillAirSpreading();
		}
		return Mathf.Clamp (Mathf.FloorToInt (res), 0, 100);
	}

	/**
	 * Retourne la transmission par contact calculée pour cette souche
	 * On prend en compte les facteurs multiplicateurs des symptomes
	 */
	public int getContactSpreading ()
	{
		float res = this.contactSpreading;
		res -= computeHumanEffectOnSkill("KnowledgeSpreading");
		res += this.souche.country.getNbPopulation () / this.souche.country.superficie * 100;

		foreach (KeyValuePair<string, AbstactSymptom> symptom in this.souche.symptoms) {
			res *= symptom.Value.affectSkillContactSpreading();
		}
		return Mathf.Clamp (Mathf.FloorToInt (res), 0, 100);
	}
}
