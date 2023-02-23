using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	private Vector2 lastInputVector = Vector2.zero;
	private Player player;
	private InputManager inputManager;

	private void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
		player = GetComponent<Player>();
	}

	private void Start()
	{
		inputManager = InputManager.Instance;
		inputManager.playerInputActions.Player.Jump.performed += Jump;
		inputManager.playerInputActions.Player.TapInteract.performed += TapInteract;
		inputManager.playerInputActions.Player.HoldInteract.performed += HoldInteract;
		inputManager.playerInputActions.Player.Inventory.performed += Inventory;
	}

	private void OnDisable()
	{
		inputManager.playerInputActions.Player.Jump.performed -= Jump;
		inputManager.playerInputActions.Player.TapInteract.performed -= TapInteract;
		inputManager.playerInputActions.Player.HoldInteract.performed -= HoldInteract;
		inputManager.playerInputActions.Player.Inventory.performed -= Inventory;
	}

	private void FixedUpdate()
	{
		Movement();
	}

	private void TapInteract(InputAction.CallbackContext context)
	{
		// Debug.Log("Tap Interact!");
		player.TapInteract(context);
	}

	private void HoldInteract(InputAction.CallbackContext context)
	{
		// Debug.Log("Hold Interact!");
		player.HoldInteract(context);
	}

	private void Inventory(InputAction.CallbackContext context)
	{
		// Debug.Log("Inventory!");
		UIManager.Instance.TriggerInventoryPanel();
	}



	private void Movement()
	{
		Vector2 inputVector = inputManager.playerInputActions.Player.Movement.ReadValue<Vector2>();
		float speed = 10f;
		float newVelocityX = Mathf.Lerp(rb.velocity.x, inputVector.x * speed, 10f * Time.deltaTime);
		float newVelocityZ = Mathf.Lerp(rb.velocity.z, inputVector.y * speed, 10f * Time.deltaTime);
		rb.velocity = new Vector3(newVelocityX, rb.velocity.y, newVelocityZ);
		// rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(inputVector.x, rb.velocity.y, inputVector.y) * speed, 10f * Time.deltaTime);

		if (inputVector != Vector2.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(inputVector.x, 0f, inputVector.y)), 10f * Time.deltaTime);
		}

		lastInputVector = inputVector;
	}

	private void Jump(InputAction.CallbackContext context)
	{
		Debug.Log("Jump!");
		rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
	}
}
