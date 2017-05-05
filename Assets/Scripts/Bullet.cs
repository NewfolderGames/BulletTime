using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public	int	damage;

	void OnCollisionEnter(Collision other) {

		if (other.gameObject.CompareTag ("Enemy")) {

			Enemy enemy = other.gameObject.GetComponent<Enemy> ();
			enemy.EnemyDamage (damage);

		}
		Destroy (gameObject);

	}

}
