using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
	private Rigidbody rb;
	private List<FarmField> allFarmFields = new List<FarmField>();
	private List<FarmField> closeFarmFields = new List<FarmField>();
	public FarmField closestFarmField;
	private InputManager inputManager;

	public GameObject playerDebugObject;

	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;
	public HotbarToolbar hotbarToolbar;

	private void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		inputManager = InputManager.Instance;
		mainInventory = MainInventory.Instance;
		hotbarInventory = HotbarInventory.Instance;
		hotbarToolbar = HotbarToolbar.Instance;

		allFarmFields.AddRange(FindObjectsOfType<FarmField>(true));
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

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("FarmPushTrigger"))
		{
			Vector3 pushDirection = transform.position - other.GetComponent<FarmPushTrigger>().transform.position;
			pushDirection.y = 0;
			float distance = Mathf.Max(0.05f, pushDirection.magnitude);
			pushDirection.Normalize();
			pushDirection *= Mathf.Min(5f, 1 / distance);
			// Debug.Log("Pushing player: " + pushDirection + " distance: " + distance);
			rb.AddForce(pushDirection * 20, ForceMode.Force);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("FarmField"))
		{
			FarmField farmField = other.GetComponent<FarmField>();
			if (closeFarmFields.Contains(farmField))
			{
				closeFarmFields.Remove(farmField);
			}
		}
		else if (other.CompareTag("FarmPushSolid"))
		{
			other.isTrigger = false;
			// other.GetComponentInParent<FarmField>().farmPushTriggerCollider.enabled = false;
		}
		else if (other.CompareTag("FarmPushTrigger"))
		{
			if (!other.GetComponentInParent<FarmField>().farmPushSolidCollider.isTrigger)
			{
				other.GetComponentInParent<FarmField>().farmPushTriggerCollider.enabled = false;
			}
		}
	}

	public void TapInteract(InputAction.CallbackContext context)
	{
		// if (closestFarmField != null)
		// {
		Debug.Log("Tap Interact!");
		// }
	}

	public void HoldInteract(InputAction.CallbackContext context)
	{
		Debug.Log("Hold Interact!");
	}

	public void TapUse(InputAction.CallbackContext context)
	{
		Debug.Log("Tap Use!");
		Action<Item> useItemFunction = (Item item) => { };
		Item useItem = null;
		hotbarToolbar.UseItem(ref useItemFunction, out useItem);
		if (useItemFunction != null && useItem.itemType != Item.ItemType.None)
		{
			useItemFunction(useItem);
		}
	}

	public void HoldUse(InputAction.CallbackContext context)
	{
		Debug.Log("Hold Use!");
	}

	public void PlantSeed(Item item, ref bool success)
	{
		if (closestFarmField == null)
		{
			Utils.CreateWorldTextPopup("No farm\nfield nearby!", playerDebugObject.transform, null, Color.red);
			success = false;
			return;
		}
		else if (closestFarmField.farmState != FarmField.FarmState.Empty)
		{
			Utils.CreateWorldTextPopup("Farm field is\not empty!", playerDebugObject.transform, null, Color.red);
			success = false;
			return;
		}
		closestFarmField.PlantSeed(item);
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
		if (closestFarmField)
		{

			closestFarmField.ActivateHighlight(true);
		}
	}

	private void FixedUpdate()
	{
		if (playerDebugObject != null)
		{
			playerDebugObject.transform.position = transform.position + Vector3.up * 0.5f;
		}
	}
}
