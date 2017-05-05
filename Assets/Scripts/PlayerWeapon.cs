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
	private	bool	weaponAvailableReload = true;
	private bool	weaponAvailableSight = true;
	private	bool	weaponNoammo;

	public	Transform	weaponSight;
	public	Vector3		weaponSightPosition;

	private	Animator	weaponAnimator;

	public	ParticleSystem	weaponShell;	

	public	Transform	weaponCameraTransform;
	public	GameObject	weaponBullet;

	public	AudioSource	weaponSoundSource;
	public	AudioClip	weaponSoundShoot;

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

	void	OnEnable() {

		if (weaponAmmoCurrent != weaponAmmoMax) weaponAvailableReload = true;
		else weaponAvailableReload = false;

		weaponAvailableSight = true;
		if (weaponNoammo) {

			weaponAvailableShoot = false;
			weaponAnimator.Play ("IdleNoammo", -1, 0f);

		} else {

			weaponAvailableShoot = true;
			weaponAnimator.Play ("Idle", -1, 0f);

		}

	}

	void	WeaponSight(bool onoff) {

		if (onoff) weaponSight.localPosition = weaponSightPosition;
		else weaponSight.localPosition = Vector3.zero;

	}

	IEnumerator WeaponShoot() {

		weaponShell.Emit (1);
		weaponAmmoCurrent--;
		if (weaponAmmoCurrent <= 0) weaponNoammo = true;

		weaponAvailableReload = false;
		weaponAvailableShoot = false;

		if (weaponNoammo) weaponAnimator.SetTrigger ("FireNoammo");
		else weaponAnimator.SetTrigger ("Fire");

		GameObject bullet = Instantiate (weaponBullet, weaponCameraTransform.position, weaponCameraTransform.rotation);
		bullet.GetComponent<Rigidbody> ().AddForce (transform.forward * 100f + Vector3.up * 0.5f, ForceMode.Impulse);
		Destroy (bullet, 15f);

		weaponSoundSource.PlayOneShot (weaponSoundShoot);

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

		weaponAvailableReload = true;
		weaponAvailableShoot = true;
		weaponAvailableSight = true;

		weaponAmmoCurrent = weaponAmmoMax;
		if (!weaponNoammo) { 
			
			weaponAmmoCurrent++;
			weaponAvailableReload = false;

		}

		weaponNoammo = false;

	}

}
