using UnityEngine;
using System.Collections;

public class ZoomPays : MonoBehaviour {

	private MeshCollider collider;
	private Vector3 boundsSize;
	private float size;

	// Use this for initialization
	void Start () {
		collider = GetComponent<MeshCollider> ();
		boundsSize = collider.bounds.size;
		size = Mathf.Max (boundsSize.x, Mathf.Max (boundsSize.y, boundsSize.z));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
