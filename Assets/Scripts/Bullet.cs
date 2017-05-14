using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public	int		damage;
	public	bool	enemy;

	void OnCollisionEnter(Collision other) {

		if (!enemy && other.gameObject.CompareTag ("Enemy")) {

			Enemy enemyInfo = other.gameObject.GetComponent<Enemy> ();
			enemyInfo.EnemyDamage (damage);

		}
		if (enemy && other.gameObject.CompareTag ("Player")) {

			Debug.Log ("Hit");

		}
		Destroy (gameObject);

	}

}
