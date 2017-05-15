using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStage : MonoBehaviour {

	public	string	buttonSceneName;

	public	void	Enter() {

		SceneManager.LoadScene (buttonSceneName);

	}

}
