using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof (NavMeshAgent))]
public class RobotEnemy : BaseRobot {
	public GameObject target;
    private Transform monster; 
    public Vector3 destination; NavMeshAgent agent;
	public GameObject[] items;
	
	void Start () {
		// Cache agent component and destination
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		destination = agent.destination;
        monster = GetComponent<Transform>();
		hp = 20;
		
	}
	int attack = 10;
	int attack_range = 5;
	int attack_speed = 1;
	int move_speed = 7;
	float time_limit = 1.0f;
	float time_cnt = 0;
	public float rate = 0.1f;
	void Update () {
		// Update destination if the target moves one unit
		if(target){
			time_limit = 1.0f/attack_speed;
			// Transfrom monster = transform;
			if (Vector3.Distance (transform.position, target.transform.position) > attack_range) {
				destination = target.GetComponent<Transform>().position;
				agent.SetDestination(destination);
				agent.speed = move_speed;
				time_cnt = time_limit;
			}
			else{
				// agent.SetDestination(monster.position);
				time_cnt += Time.deltaTime;
				if (time_cnt >= time_limit) {
					target.GetComponent<RobotPlayer>().GetDamage(attack);
					Debug.Log(string.Format("Attack !!! time=${0} {1}", Time.time,attack));
					time_cnt = 0.0f;
				}
				agent.speed = 0;
				
			}
		}
		
	}
	public override void GetDamage(int dmg)
    {
        hp -= dmg*Global.damageFactor;
        if (!IsAlive())
        {
            Die();
        }
    }

	public override void Die(){
		Global.enemy_count--;
		Destroy(this.gameObject);
		GameObject.Instantiate(items[0],monster.position,Quaternion.identity);
	}
}

