using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Component
	private	CharacterController playerCharacterController;

	// Camera
	public	Camera		playerCamera;
	public	Camera		playerFakeCamera;

	public	Transform	playerCameraTransform;
	public	Transform	playerFakeCameraTransform;

	public	Animator	playerFakeCameraAnimator;

	// Animation
	public	AnimationClip[]	playerObstacleAnimations;

	// Movement
	private	Vector3	playerMovement;
	private	Vector3	playerVelocity;

	private	float	playerSpeed;
	private	float	playerSpeedJump;

	private	float	playerMovementHorizontal;
	private	float	playerMovementVertical;

	private	bool	playerAvailableMove = true;
	private	bool	playerAvailableJump;

	// Obstacle
	private	MapObstacle.Obstacle playerObstacleType;

	private	bool	playerAvailableObstacle;
	private	bool	playerUsingObstacle;

	// Rotation
	private	float	playerSensitivity;

	private	float	playerRotationHorizonal;
	private	float	playerRotationVertical;

	void	Awake() {

		playerCharacterController = GetComponent<CharacterController> ();

		playerSpeed = 5f;
		playerSpeedJump = 5f;
		playerSensitivity = 2.5f;

	}

	void	Update() {

		// Input
		playerMovementHorizontal	= Input.GetAxis ("Horizontal") * playerSpeed;
		playerMovementVertical		= Input.GetAxis ("Vertical") * playerSpeed;

		playerRotationHorizonal		= Input.GetAxis ("Mouse X") * playerSensitivity;
		playerRotationVertical	   -= Input.GetAxis ("Mouse Y") * playerSensitivity;
		playerRotationVertical		= Mathf.Clamp (playerRotationVertical, -90, 90);

		if (playerAvailableMove) {

			// Rotation
			transform.Rotate (0f, playerRotationHorizonal, 0f);
			playerCameraTransform.localRotation = Quaternion.Euler (playerRotationVertical, 0f, 0f);

			// Jump
			if (Input.GetButtonDown ("Jump") && playerAvailableJump && playerAvailableMove) {

				if (playerAvailableObstacle) {

					RaycastHit rayhit;
					if (Physics.Raycast (transform.position + Vector3.down * 0.5f, transform.forward, out rayhit, Mathf.Infinity, LayerMask.GetMask ("Obstacle"))) {

						playerObstacleType = rayhit.collider.GetComponent<MapObstacle> ().obstacleType;

						playerAvailableMove = false;
						playerUsingObstacle = true;

						playerCamera.gameObject.SetActive (false);
						playerFakeCamera.gameObject.SetActive (true);

						StartCoroutine (PlayObstacle (playerObstacleType));

						Debug.Log ("XXXXXXXXXXXXX");

						return;

					}

				}

				playerVelocity += Vector3.up * playerSpeedJump;
				playerAvailableJump = false;

			}

			// Movement
			playerMovement.Set (playerMovementHorizontal, 0f, playerMovementVertical);
			playerMovement = transform.rotation * (playerMovement + playerVelocity);
			playerCharacterController.Move (playerMovement * Time.deltaTime);

		}



	}

	void	OnTriggerEnter (Collider other) {

		if(other.CompareTag("Obstacle")) {

			MapObstacle obstacle = other.GetComponent<MapObstacle>();
			switch(obstacle.obstacleType) {

				case MapObstacle.Obstacle.Skip:
					 
					playerAvailableObstacle = true;
					Debug.Log("Skipable In");
					break;

			}

		}

	}

	void	OnTriggerExit (Collider other) {

		if(other.CompareTag("Obstacle")) {

			MapObstacle obstacle = other.GetComponent<MapObstacle>();
			switch(obstacle.obstacleType) {

				case MapObstacle.Obstacle.Skip:

					playerAvailableObstacle = false;
					Debug.Log("Skipable Out");
					break;

			}

		}

	}

	void	FixedUpdate() {

		if (!playerCharacterController.isGrounded) {

			playerVelocity += Physics.gravity * Time.deltaTime;

		} else {

			playerVelocity = Vector3.zero;
			playerAvailableJump = true;

		}
	
	}

	private	IEnumerator	PlayObstacle(MapObstacle.Obstacle obstacle) {

		playerFakeCameraAnimator.SetBool ("Skip", true);
		AnimatorStateInfo info = playerFakeCameraAnimator.GetCurrentAnimatorStateInfo ((int)obstacle);

		yield return new WaitForSeconds (info.length);

		playerAvailableMove = true;
		playerUsingObstacle = false;

		playerCamera.gameObject.SetActive (true);
		playerFakeCamera.gameObject.SetActive (false);

		transform.position = playerFakeCameraTransform.position + Vector3.down * playerFakeCameraTransform.localPosition.y;
		playerCameraTransform.rotation = playerFakeCameraTransform.rotation;

		playerFakeCameraAnimator.SetBool ("Skip", false);

	}


}
