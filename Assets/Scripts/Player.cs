using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	private Rigidbody rb;
	private List<GameObject> farmFields = new List<GameObject>();
	private GameObject closestFarmField;
	private InputManager inputManager;

	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;

	private void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		inputManager = InputManager.Instance;
		mainInventory = MainInventory.Instance;
		hotbarInventory = HotbarInventory.Instance;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmField"))
		{
			// Debug.Log("Farm entered");
			if (!farmFields.Contains(other.gameObject))
			{
				farmFields.Add(other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("FarmField"))
		{
			// Debug.Log("Farm exited");
			if (farmFields.Contains(other.gameObject))
			{
				farmFields.Remove(other.gameObject);
			}
		}
	}

	public void TapInteract(InputAction.CallbackContext context)
	{
		if (closestFarmField != null)
		{
			Debug.Log("Farm - Tap Interact!");
			// closestFarmField.GetComponent<FarmField>().TapInteract(context);
		}
	}

	public void HoldInteract(InputAction.CallbackContext context)
	{
		if (closestFarmField != null)
		{
			Debug.Log("Farm - Hold Interact!");
			// closestFarmField.GetComponent<FarmField>().HoldInteract(context);
		}
	}

	private void Update()
	{
		if (farmFields.Count > 0)
		{
			closestFarmField = farmFields[0];
			foreach (GameObject farmField in farmFields)
			{
				if (Vector3.Distance(transform.position, farmField.transform.position) < Vector3.Distance(transform.position, closestFarmField.transform.position))
				{
					closestFarmField = farmField;
				}
			}
		}
		else
		{
			closestFarmField = null;
		}
	}
}
