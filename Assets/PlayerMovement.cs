using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Component
	private	CharacterController playerCharacterController;

	// Movement
	private	Vector3	playerMovement;
	private	Vector3	playerVelocity;

	private	float	playerSpeed;
	private	float	playerSpeedJump;

	private	float	playerMovementHorizontal;
	private	float	playerMovementVertical;

	private	bool	playerAvailableJump;

	// Rotation
	public	Transform	playerCamera;

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

		// Rotation
		transform.Rotate (0f, playerRotationHorizonal, 0f);
		playerCamera.localRotation = Quaternion.Euler (playerRotationVertical, 0f, 0f);

		// Jump
		if (Input.GetButtonDown ("Jump") && playerAvailableJump) {

			playerVelocity += Vector3.up * playerSpeedJump;
			playerAvailableJump = false;

		}

		// Movement
		playerMovement.Set(playerMovementHorizontal, 0f, playerMovementVertical);
		playerMovement = transform.rotation * (playerMovement + playerVelocity);
		playerCharacterController.Move (playerMovement * Time.deltaTime);


	}

	void	FixedUpdate() {
	
		if (!playerCharacterController.isGrounded) {

			playerVelocity += Physics.gravity * Time.deltaTime;

		} else {

			playerVelocity = Vector3.zero;
			playerAvailableJump = true;

		}
	
	}

}
