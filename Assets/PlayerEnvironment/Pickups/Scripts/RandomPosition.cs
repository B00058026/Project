using UnityEngine;
using System.Collections;

public class RandomPosition : MonoBehaviour {

    public Vector3[] positions;

	// Use this for initialization
	void Start () {
        int random = Random.Range(0, positions.Length);
        transform.position = positions[random];
	}
	
	// Update is called once per frame
	void Update () {
        
    }

}

