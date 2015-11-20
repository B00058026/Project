using UnityEngine;
using System.Collections;

public class enemyAIv2 : MonoBehaviour {
	Ray enemyRay;
	public Color rayColor;
	bool follow;
	
	private NavMeshAgent agent;
	public GameObject targetObject;
	public Transform[] wayPoints;


	private int NextDestination  = 0;

	// Use this for initialization
	void Start () {
			 
		agent = GetComponent<NavMeshAgent> ();


	}
	
	// Update is called once per frame
	void Update () {
			 
		enemyRay = new Ray (transform.position,transform.forward*10);
		Debug.DrawRay (transform.position, transform.forward*10, rayColor);

		if (Physics.Raycast (transform.position, transform.forward, 10)) {
			follow = true;
			//AttackState ();
			
		} 


		if(follow==true){
			AttackState ();


		}
		if(follow==false){
			PassiveState();



		}
		else{
			//follow = false;

		}


	}
	void AttackState(){
		Debug.Log ("Enemy is in a attack state");
		agent.SetDestination (targetObject.transform.position);
		if(agent.remainingDistance z> 30f){
			follow=false;
			
		}
		
		
	


	}
	void PassiveState(){
		Debug.Log ("Enemy is in a passive state");

		if(agent.remainingDistance < 0.5f){
			agent.SetDestination(wayPoints[NextDestination].position);
			NextDestination = (NextDestination + 1) % wayPoints.Length;
			
		}






	}
	void HuntingState(){
		//if enemy detects a sound nearby, enemy will follow the sound to the location


	}

}
