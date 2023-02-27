using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	private Rigidbody rb;
	private List<FarmField> allFarmFields = new List<FarmField>();
	private List<FarmField> closeFarmFields = new List<FarmField>();
	private FarmField closestFarmField;
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

		allFarmFields.AddRange(FindObjectsOfType<FarmField>());
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmField"))
		{
			FarmField farmField = other.GetComponent<FarmField>();
			// Debug.Log("Farm entered");
			if (!closeFarmFields.Contains(farmField))
			{
				closeFarmFields.Add(farmField);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("FarmField"))
		{
			FarmField farmField = other.GetComponent<FarmField>();
			// Debug.Log("Farm exited");
			if (closeFarmFields.Contains(farmField))
			{
				closeFarmFields.Remove(farmField);
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
		if (closeFarmFields.Count > 0)
		{
			closestFarmField = closeFarmFields[0];
			foreach (FarmField farmField in closeFarmFields)
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

		foreach (FarmField farmField in allFarmFields)
		{
			farmField.ActivateHighlight(false);
		}
		if (closestFarmField) {

			closestFarmField.ActivateHighlight(true);
		}
	}
}
