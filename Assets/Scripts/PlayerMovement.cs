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

	public	float		playerCameraFov = 60;
	public	float		playerCameraFovSlowmotion = 120f;

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
	private	MapObstacle.Obstacle	playerObstacleType;
	private	MapObstacle				playerObstacle;

	private	bool	playerAvailableObstacle;
	private	bool	playerUsingObstacle;

	// Rotation
	private	float	playerSensitivity;

	private	float	playerRotationHorizonal;
	private	float	playerRotationVertical;

	// Time
	private	float	playerTimeSlowdown;
	private	float	playerTimeSlowdownDurantion;
	private	bool	playerAvailableSlowdown = true;


	void	Awake() {

		playerCharacterController = GetComponent<CharacterController> ();

		playerSpeed = 5f;
		playerSpeedJump = 4f;
		playerSensitivity = 2.5f;

		playerTimeSlowdown = 0.1f;
		playerTimeSlowdownDurantion = 10f;

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
			if (Input.GetButtonDown ("Jump") && playerAvailableMove) {

				if (playerAvailableObstacle) {

					RaycastHit rayhit;
					if (Physics.Raycast (transform.position + Vector3.down * 0.5f, transform.forward, out rayhit, 15f, LayerMask.GetMask ("Obstacle"))) {

						MapObstacle obstacle = rayhit.collider.GetComponent<MapObstacle> ();
						playerObstacleType = obstacle.obstacleType;

						if (playerAvailableJump || obstacle.obstacleMidair) {

							StartCoroutine (PlayObstacle (playerObstacleType));

						}
							
						return;

					}

				}

				if (playerAvailableJump) {

					playerVelocity += Vector3.up * playerSpeedJump;
					playerAvailableJump = false;

				} 

			}

			// Movement
			playerMovement.Set (playerMovementHorizontal, 0f, playerMovementVertical);
			playerMovement = transform.rotation * (playerMovement + playerVelocity);
			playerCharacterController.Move (playerMovement * Time.deltaTime);

		}

		// Time
		if (playerAvailableSlowdown) {

			if (Input.GetMouseButtonDown (2)) {

				playerAvailableSlowdown = false;
				Time.timeScale = playerTimeSlowdown;
				Time.fixedDeltaTime = Time.timeScale * 0.02f;
				Debug.Log ("aayy");

			}

		} else {

			Time.timeScale += 1f / playerTimeSlowdownDurantion * Time.unscaledDeltaTime;
			if (Time.timeScale >= 1f) {

				playerAvailableSlowdown = true;
				return;

			}

		}

	}

	void	OnTriggerEnter (Collider other) {

		if(other.CompareTag("Obstacle")) {

			playerObstacle = other.GetComponent<MapObstacle>();
			playerAvailableObstacle = true;

		}

	}

	void	OnTriggerExit (Collider other) {

		if(other.CompareTag("Obstacle")) {

			MapObstacle obstacle = other.GetComponent<MapObstacle>();
			playerAvailableObstacle = false;

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

			case MapObstacle.Obstacle.Climb4M:
				playerFakeCameraAnimator.Play ("ObstacleClimb4M", -1, 0f);
				time = 2.5f;
				break;

			case MapObstacle.Obstacle.Slide:
				playerFakeCameraAnimator.Play ("ObstacleSlide", -1, 0f);
				time = 1f;
				break;

			case MapObstacle.Obstacle.Swing:
				playerFakeCameraAnimator.Play ("ObstacleSwing", -1, 0f);
				time = 2f;
				break;

		}

		AnimatorStateInfo info = playerFakeCameraAnimator.GetCurrentAnimatorStateInfo (0);

		yield return new WaitForSeconds (time);

		playerAvailableMove = true;
		playerUsingObstacle = false;

		transform.position = playerFakeCameraTransform.position + Vector3.down * 0.7f;
		playerCameraTransform.rotation = playerFakeCameraTransform.rotation;
		playerRotationVertical = 0f;

		playerCamera.gameObject.SetActive (true);
		playerFakeCamera.gameObject.SetActive (false);

	}


}
