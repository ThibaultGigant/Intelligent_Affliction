using System;

public class CarteDeVisite
{
	/**
	 * Pays dont il est question
	 */
	public Pays pays;

	/**
	 * Liste des ressources prêt à échanger
	 */
	public PaysRessources ressources;

	public PaysRessources ressourcesDemandees;

	/**
	 * Ratio entre le nombre de soignés et le nombre d'infectés détectés par jour
	 * -1 signie qu'on ne communique pas l'info
	 */
	public float ratioSoigneInfecteDetecte;

	public CarteDeVisite (Pays pays)
	{
		this.pays = pays;
		ressources = new PaysRessources (pays);
		ressourcesDemandees = new PaysRessources (pays);
		ratioSoigneInfecteDetecte = -1;
	}

	public void addRessource(Ressource ressource) {
		ressources.resources.Add (ressource.nom, ressource);
	}

	public void addRessourceDemandee(Ressource ressource) {
		ressourcesDemandees.resources.Add (ressource.nom, ressource);
	}

}

