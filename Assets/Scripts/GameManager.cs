using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public	static	int		gameStageNumber;
	public	static	int		gameEnemyNumber;

	public	bool	gameClear;

	public	static	Text	gameTextEnemy;
	public	Text	gameTextSpeed;
	public	Text	gameTextClear;

	void Awake() {

		gameEnemyNumber = 0;
		gameTextEnemy = GameObject.Find ("EnemyNumber").GetComponent<Text>();

	}

	void Start () {

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	void Update () {

		if (!gameClear && gameEnemyNumber <= 0) {
			
			StartCoroutine (Clear ());

		}

	}

	IEnumerator Clear() {

		gameTextEnemy.gameObject.SetActive (false);
		gameTextSpeed.gameObject.SetActive (false);
		gameTextClear.gameObject.SetActive (true);

		yield return new WaitForSecondsRealtime (5);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		SceneManager.LoadScene ("Main");

	}

	public	static	void	TextUpdateEnemy() {

		gameTextEnemy.text = "ENEMY\n" + gameEnemyNumber.ToString ();
		//GameObject.Find ("EnemyNumber").GetComponent<Text>().text = "ENEMY\n" + gameEnemyNumber.ToString ();

	}

}
