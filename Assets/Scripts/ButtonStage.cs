using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStage : MonoBehaviour {

	public	string	buttonSceneName;
	public	int		buttonStageNumber;

	public	void	Enter() {

		GameManager.gameStageNumber = buttonStageNumber;

		SceneManager.LoadScene (buttonSceneName);

	}

}
