using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public	static	int		gameLevel;
	public	static	int		gameEnemyNumber;

	public	static	Text	gameTextEnemy;

	void Awake() {

		gameTextEnemy = GameObject.Find ("EnemyNumber").GetComponent<Text>();

	}

	void Start () {

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	void Update () {
		
	}

	public	static	void	TextUpdateEnemy() {

		gameTextEnemy.text = "ENEMY\n" + gameEnemyNumber.ToString ();

	}

}
