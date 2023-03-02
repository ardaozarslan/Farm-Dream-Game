using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
	private Rigidbody rb;
	// private List<FarmField> allFarmFields = new List<FarmField>();
	private List<IPointOfInterest> closePointOfInterests = new List<IPointOfInterest>();
	public IPointOfInterest closestPointOfInterest;
	private IPointOfInterest lastClosestPointOfInterest;

	// private List<FarmField> closeFarmFields = new List<FarmField>();
	// public FarmField closestFarmField;
	private InputManager inputManager;

	public GameObject playerDebugObject;

	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;
	public HotbarToolbar hotbarToolbar;

	public PlayerController.HoldInteractInputState holdInteractInputState = PlayerController.HoldInteractInputState.Canceled;

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

		// allFarmFields.AddRange(FindObjectsOfType<FarmField>(true));
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Interactable"))
		{
			// FarmField farmField = other.GetComponent<FarmField>();
			// // Debug.Log("Farm entered");
			// if (!closeFarmFields.Contains(farmField))
			// {
			// 	closeFarmFields.Add(farmField);
			// }
			IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();
			if (!closePointOfInterests.Contains(pointOfInterest))
			{
				closePointOfInterests.Add(pointOfInterest);
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
		if (other.CompareTag("Interactable"))
		{
			// FarmField farmField = other.GetComponent<FarmField>();
			// if (closeFarmFields.Contains(farmField))
			// {
			// 	closeFarmFields.Remove(farmField);
			// }
			IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();
			pointOfInterest.ActivateHighlight(false);
			if (closePointOfInterests.Contains(pointOfInterest))
			{
				closePointOfInterests.Remove(pointOfInterest);
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

	public void TapInteractInput(InputAction.CallbackContext context)
	{
		// if (closestFarmField != null)
		// {
		Debug.Log("Tap Interact!");
		// InteractWithPointOfInterest(IPointOfInterest.InputType.Tap);
		// }
	}

	public void HoldInteractInputStarted(InputAction.CallbackContext context)
	{
		Debug.Log("Hold Interact Started!");
		holdInteractInputState = PlayerController.HoldInteractInputState.Started;
		if (closestPointOfInterest != null && closestPointOfInterest is IInteractable)
		{
			if ((closestPointOfInterest as IInteractable).Interact(IPointOfInterest.InputType.Hold, true))
			{
				Utils.CreateWorldTextPopup("Hold Interact Started!", playerDebugObject.transform, Vector3.one * 0.2f);
				// TODO: 	Start the action here (animations, progress bar, etc.)
				// TODO: 	Also stop the movement of the player until it is canceled
			}
			else
			{
				GetComponent<PlayerController>().DisallowHoldInteractInput();
				// TODO: 	holdInteractInputState = PlayerController.HoldInteractInputState.Canceled;
			}
		}


	}

	public void HoldInteractInputPerformed(InputAction.CallbackContext context)
	{
		Debug.Log("Hold Interact Performed!");
		holdInteractInputState = PlayerController.HoldInteractInputState.Performed;
		if (closestPointOfInterest != null && closestPointOfInterest is IInteractable)
		{
			InteractWithPointOfInterest(IPointOfInterest.InputType.Hold);
			Utils.CreateWorldTextPopup("Hold Interact Performed!", playerDebugObject.transform, Vector3.one * 0.2f);
			// TODO: Finish the action (animations, give the item, etc.)
		}
	}

	public void HoldInteractInputCanceled(InputAction.CallbackContext context)
	{
		Debug.Log("Hold Interact Canceled!");
		Utils.CreateWorldTextPopup("Hold Interact Canceled!", playerDebugObject.transform, Vector3.one * 0.2f);
		// TODO: Cancel the action here (animations, progress bar, etc.)
		// TODO: Resume the movement of the player
		holdInteractInputState = PlayerController.HoldInteractInputState.Canceled;
	}

	public void TapUseInput(InputAction.CallbackContext context)
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

	public void HoldUseInput(InputAction.CallbackContext context)
	{
		Debug.Log("Hold Use!");
	}

	public void PlantSeed(Item item, ref bool success)
	{
		if (closestPointOfInterest is not FarmField)
		{
			Utils.CreateWorldTextPopup("No farm\nfield nearby!", playerDebugObject.transform, null, Color.red);
			success = false;
			return;
		}
		else if ((closestPointOfInterest as FarmField).farmState != FarmField.FarmState.Empty)
		{
			Utils.CreateWorldTextPopup($"Farm field:\n{(closestPointOfInterest as FarmField).farmState.ToString()}!", playerDebugObject.transform, null, Color.red);
			success = false;
			return;
		}
		(closestPointOfInterest as FarmField).UseItemOnThis(item);
	}

	public void InteractWithPointOfInterest(IPointOfInterest.InputType interactionType, bool _checking = false)
	{
		(closestPointOfInterest as IInteractable).Interact(interactionType);
	}

	private void Update()
	{
		if (closestPointOfInterest != null)
		{
			lastClosestPointOfInterest = closestPointOfInterest;
		}
		if (closePointOfInterests.Count > 0)
		{
			closestPointOfInterest = closePointOfInterests[0];
			foreach (IPointOfInterest pointOfInterest in closePointOfInterests)
			{
				if (Vector3.Distance(transform.position, pointOfInterest.GetGameObject().transform.position) < Vector3.Distance(transform.position, closestPointOfInterest.GetGameObject().transform.position))
				{
					closestPointOfInterest = pointOfInterest;
				}
			}
		}
		else
		{
			closestPointOfInterest = null;
		}

		lastClosestPointOfInterest?.ActivateHighlight(false);
		closestPointOfInterest?.ActivateHighlight(true);
	}

	private void FixedUpdate()
	{
		if (playerDebugObject != null)
		{
			playerDebugObject.transform.position = transform.position + Vector3.up * 1.5f;
		}
	}
}
