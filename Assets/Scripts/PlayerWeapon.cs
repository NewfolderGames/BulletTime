using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

	public	int		weaponAmmoCurrent;
	public	int		weaponAmmoMax;
	public	int		weaponAmmoTotal;

	public	float	weaponTimeShoot;
	public	float	weaponTimeReload;
	public	float	weaponTimeReloadNoammo;

	public	bool	weaponSemiauto;

	private	bool	weaponAvailableShoot = true;
	private	bool	weaponAvailableReload;
	private bool	weaponAvailableSight = true;
	private	bool	weaponNoammo;

	public	Transform	weaponSight;
	public	Vector3		weaponSightPosition;

	private	Animator	weaponAnimator;

	void	Awake() {

		weaponAnimator = GetComponent<Animator> ();

	}

	void	Update() {

		if (weaponAvailableShoot) {

			if ((Input.GetButton ("Fire1") && !weaponSemiauto) || (Input.GetButtonDown ("Fire1") && weaponSemiauto)) {

				StartCoroutine (WeaponShoot ());

			}

		}

		if (weaponAvailableReload && Input.GetKeyDown (KeyCode.R)) {

			StartCoroutine (WeaponReload ());

		}

		if (weaponAvailableSight && Input.GetButton ("Fire2")) WeaponSight (true);
		else WeaponSight (false);

	}

	void	WeaponSight(bool onoff) {

		if (onoff) weaponSight.localPosition = weaponSightPosition;
		else weaponSight.localPosition = Vector3.zero;

	}

	IEnumerator WeaponShoot() {

		weaponAmmoCurrent--;
		if (weaponAmmoCurrent <= 0) weaponNoammo = true;

		weaponAvailableReload = false;
		weaponAvailableShoot = false;

		if (weaponNoammo) weaponAnimator.SetTrigger ("FireNoammo");
		else weaponAnimator.SetTrigger ("Fire");

		yield return new WaitForSeconds (weaponTimeShoot);

		weaponAvailableReload = true;
		if(!weaponNoammo) weaponAvailableShoot = true;

	}

	IEnumerator	WeaponReload() {

		weaponAvailableReload = false;
		weaponAvailableShoot = false;
		weaponAvailableSight = false;

		weaponAnimator.SetTrigger ("Reload");

		if(!weaponNoammo) yield return new WaitForSeconds (weaponTimeReload);
		else yield return new WaitForSeconds (weaponTimeReloadNoammo);

		weaponAmmoCurrent = weaponAmmoMax;

		weaponAvailableReload = false;
		weaponAvailableShoot = true;
		weaponAvailableSight = true;
		weaponNoammo = false;

	}

}
