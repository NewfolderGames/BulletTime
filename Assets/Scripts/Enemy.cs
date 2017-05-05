using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private	NavMeshAgent	enemyNavMeshAgent;

	private	GameObject	enemyTarget;
	private	Transform	enemyTargetTransform;


	void Start () {

		enemyTarget = GameObject.Find ("Player");
		enemyTargetTransform = enemyTarget.transform;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
