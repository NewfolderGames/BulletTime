using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private	NavMeshAgent	enemyNavMeshAgent;

	private	GameObject	enemyTarget;
	private	Transform	enemyTargetTransform;

	public	Transform	enemyWeaponTransform;

	public	int	enemyHealth = 10;

	void Awake() {

	}

	void Start () {

		enemyNavMeshAgent = GetComponent<NavMeshAgent> ();

		enemyTarget = GameObject.Find ("Player");
		enemyTargetTransform = enemyTarget.transform;

	}

	void Update () {

		if (enemyTarget != null) {

			enemyNavMeshAgent.SetDestination (enemyTargetTransform.position);


		}

	}

	public void EnemyDamage(int damage) {

		enemyHealth -= damage;
		if (enemyHealth <= 0) {

			Destroy (gameObject);

		}

	}

}
