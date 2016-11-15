using UnityEngine;
using System.Collections;

public class PaysManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach ( Transform t in transform )
			t.gameObject.AddComponent <Pays>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
