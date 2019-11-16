using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueueModifier : MonoBehaviour {
	public Renderer[] renderers;

	private static int offset = 1950;
	
	// Update is called once per frame
	void Update () {
		float[] distance = new float[3];
		for(int i=0 ; i<3 ; i++) {
			distance[i] = Vector3.Distance(transform.position, renderers[i].transform.position);
		}
	}
}
