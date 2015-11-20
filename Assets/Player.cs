using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Debug.Log("Player click ");
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayInfo;
			
			if(Physics.Raycast(rayOrigin, out rayInfo, 25f)){
				Debug.Log("You a casting a ray");
				Debug.DrawLine(rayOrigin.direction , rayInfo.point);
				
			}
			
			
		}   
	
	}
	void Crafting(){
		//When a player is crafting, send a raycast to represent the noise being made


		if(Input.GetMouseButtonDown(0)){
			Debug.Log("Player click ");
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayInfo;
		


			if(Physics.Raycast(rayOrigin, out rayInfo, 100f)){
				Debug.Log("You a casting a ray");
				Debug.DrawLine(rayOrigin.direction , rayInfo.point);

			}


		}




	}
	void OnCollisionEnter(Collision entity){
		if(entity.gameObject.name == "Cube"){
			Destroy (entity.gameObject);


		}
		
		
	}
}
