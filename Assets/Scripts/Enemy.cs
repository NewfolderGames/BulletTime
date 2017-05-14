using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private	NavMeshAgent	enemyNavMeshAgent;

	private	GameObject	enemyTarget;
	private	Transform	enemyTargetTransform;

	public	int	enemyHealth = 10;

	public	Transform	enemyWeaponTransform;
	public	GameObject	enemyWeaponBullet;
	public	AudioClip	enemyWeaponSound;
	private	bool	enemyWeaponAvailableShoot = true;
	private float	enemyWeaponShootDelay = 0.25f;
	private	int		enemyWeaponDamage = 5;

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

			Quaternion rotation = Quaternion.LookRotation (enemyTargetTransform.position - enemyWeaponTransform.position);
			enemyWeaponTransform.rotation = Quaternion.Slerp (enemyWeaponTransform.rotation, rotation, Time.deltaTime * 5f);

			if (enemyWeaponAvailableShoot) {

				RaycastHit rayhit;
				if (Physics.Raycast (enemyWeaponTransform.position, enemyWeaponTransform.forward, out rayhit, 25f)) {

					if (rayhit.collider.CompareTag("Player")) {
						StartCoroutine (EnemyShoot ());
					}

				}

			}

		}

	}

	public void EnemyDamage(int damage) {

		enemyHealth -= damage;
		if (enemyHealth <= 0) {

			Destroy (gameObject);

		}

	}

	private	IEnumerator	EnemyShoot() {

		enemyWeaponAvailableShoot = false;

		GameObject bullet = Instantiate (enemyWeaponBullet, enemyWeaponTransform.position, enemyWeaponTransform.rotation);
		bullet.GetComponent<Rigidbody> ().AddForce (enemyWeaponTransform.forward * 7.5f + Vector3.up * 0.125f, ForceMode.Impulse);
		bullet.GetComponent<Bullet> ().damage = enemyWeaponDamage;

		enemyTarget.GetComponent<AudioSource> ().PlayOneShot (enemyWeaponSound);

		yield return new WaitForSeconds (enemyWeaponShootDelay);

		enemyWeaponAvailableShoot = true;

	}

}
