using UnityEngine;
using System.Collections;

public class enemyAIv2 : MonoBehaviour {
	Ray enemyRay;
	public Color rayColor;
	bool follow;
	
	private NavMeshAgent agent;
	public GameObject targetObject;
	public Transform[] wayPoints;
	public float Health;
	private Camera cam;
	private int NextDestination  = 0;
	private int health;
	private float timer;

	// Use this for initialization
	void Start () {
			 
		agent = GetComponent<NavMeshAgent> ();
		cam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
		health = 2;
		timer = 0f;

	}
	void AttackState(){
		Debug.Log ("Enemy is in a attack state");
		agent.SetDestination (targetObject.transform.position);
		if (agent.remainingDistance > 30f) {
			follow = false;
			
		}
	}
	// Update is called once per frame
	void Update () {
			 
		enemyRay = new Ray (transform.position,transform.forward*50);
		Debug.DrawRay (transform.position, transform.forward*50, rayColor);

		if (Physics.Raycast (transform.position, transform.forward, 50)) {
			follow = true;
			//AttackState ();
			
		} 


		if(follow==true){
			AttackState();


		}
		if(follow==false){
			PassiveState();

				

		}
		if (health > 0)
		{
			if (Vector3.Distance(transform.position, cam.transform.root.transform.position) < 5f)
			{
				Debug.DrawRay(cam.transform.position, Vector3.forward * 5f, Color.white);
				if (Input.GetKeyDown(KeyCode.Mouse0) && timer >= 1.0f)
				{
					timer = 0f; 
					Ray ray = new Ray(cam.transform.position, cam.transform.forward);
					
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 5f))
					{
						if (hit.collider.gameObject == gameObject)
						{
							health--;
						}
					}
				}
			}
		}
		if(health==0){
				Destroy(gameObject);


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
