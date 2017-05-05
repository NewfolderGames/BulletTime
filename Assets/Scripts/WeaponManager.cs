using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	public	PlayerWeapon[]	weapons;
	public	int	weaponSlotNumber;

	void Start() {

		for (int i = 0; i < weapons.Length; i++) {

			if (i != weaponSlotNumber) weapons [i].gameObject.SetActive (false);
			else weapons [i].gameObject.SetActive (true);

		}

	}

	void Update() {

		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (wheel > 0) {

			weaponSlotNumber = Mathf.Clamp (weaponSlotNumber + 1, 0, weapons.Length - 1);
			for (int i = 0; i < weapons.Length; i++) {

				if (i != weaponSlotNumber) weapons [i].gameObject.SetActive (false);
				else weapons [i].gameObject.SetActive (true);

			}

		} else if (wheel < 0) {

			weaponSlotNumber = Mathf.Clamp (weaponSlotNumber - 1, 0, weapons.Length - 1);
			for (int i = 0; i < weapons.Length; i++) {

				if (i != weaponSlotNumber) weapons [i].gameObject.SetActive (false);
				else weapons [i].gameObject.SetActive (true);

			}

		}

	}

}
