using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAppear : MonoBehaviour {

	public	GameObject[]	appearObjects;
	public	GameObject[]	disappearObjects;

	public void Activate() {

		for (int i = 0; i < appearObjects.Length; i++) {

			appearObjects [i].SetActive (true);
			disappearObjects [i].SetActive (false);

		}

	}

}
