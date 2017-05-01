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

						StartCoroutine (PlayObstacle (playerObstacleType));

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

				case MapObstacle.Obstacle.Climb3M:

					playerAvailableObstacle = true;
					Debug.Log("Climbable3M In");
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

				case MapObstacle.Obstacle.Climb3M:

					playerAvailableObstacle = false;
					Debug.Log("Climbable3M Out");
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

		float time = 0f;

		playerAvailableMove = false;
		playerUsingObstacle = true;

		playerCamera.gameObject.SetActive (false);
		playerFakeCamera.gameObject.SetActive (true);

		switch(playerObstacleType) {
			
			case MapObstacle.Obstacle.Skip:
				playerFakeCameraAnimator.Play ("ObstacleSkip", -1, 0f);
				time = 1f;

				break;

			case MapObstacle.Obstacle.Climb3M:
				playerFakeCameraAnimator.Play ("ObstacleClimb3M", -1, 0f);
				time = 2f;
				break;

		}

		AnimatorStateInfo info = playerFakeCameraAnimator.GetCurrentAnimatorStateInfo (0);

		yield return new WaitForSeconds (time);

		playerAvailableMove = true;
		playerUsingObstacle = false;

		transform.position = playerFakeCameraTransform.position + Vector3.down * 0.7f;
		playerCameraTransform.rotation = playerFakeCameraTransform.rotation;

		playerCamera.gameObject.SetActive (true);
		playerFakeCamera.gameObject.SetActive (false);

	}


}
