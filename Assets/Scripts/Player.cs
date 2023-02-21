using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	private Rigidbody rb;
	private PlayerInputActions playerInputActions;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		playerInputActions.Player.Jump.performed += Jump;
		// playerInputActions.Player.Movement.performed += Movement;
	}

	private void FixedUpdate() {
		Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
		float speed = 10f;
		rb.AddForce(new Vector3(inputVector.x, 0f, inputVector.y) * speed, ForceMode.Force);
	}

	// private void Movement(InputAction.CallbackContext context)
	// {
	// 	Debug.Log(context);
	// 	Vector2 inputVector = context.ReadValue<Vector2>();
	// 	float speed = 5f;
	// 	rb.AddForce(new Vector3(inputVector.x, 0f, inputVector.y) * speed, ForceMode.Force);
	// }

	public void Jump(InputAction.CallbackContext context)
	{
		Debug.Log(context);
		Debug.Log("Jump!");
		rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
	}
}
