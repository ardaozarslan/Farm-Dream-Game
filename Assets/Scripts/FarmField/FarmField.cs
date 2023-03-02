using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmField : MonoBehaviour, IInteractable, IItemUsable
{
	[HideInInspector]
	public FarmFieldHighlight farmFieldHighlight;
	public FarmPushTrigger farmPushTrigger;
	public Collider farmPushTriggerCollider;
	public Collider farmPushSolidCollider;
	public Plant plant;
	public FarmState farmState = FarmState.Empty;

	public enum FarmState
	{
		Empty = 0,
		Planted = 10,
		Growing = 20,
		ReadyToHarvest = 30,
		NeedsWater = 40,
		NeedsWeed = 50,
		NeedsRake = 60,
	}

	public GameObject GetGameObject() => gameObject;

	public bool Interact(IPointOfInterest.InputType type, bool _isCheckOnly = false)
	{
		bool canInteract = true;
		if (type == IPointOfInterest.InputType.Hold)
		{
			if (farmState != FarmState.ReadyToHarvest)
			{
				Utils.CreateWorldTextPopup($"Farm field is\nnot {FarmState.ReadyToHarvest.ToString()}!", transform, null, Color.red);
				canInteract = false;
			}
			if (_isCheckOnly)
			{
				return canInteract;
			}
			else
			{
				if (canInteract)
				{
					HarvestPlant();
					return true;
				}
				return false;
			}
		}
		else if (type == IPointOfInterest.InputType.Tap)
		{
			// Check conditions here

			if (_isCheckOnly)
			{
				return canInteract;
			}
			else
			{
				if (canInteract)
				{
					// Do something
				}
				return false;
			}
		}
		else {
			return false;
		}


	}

	// public string CanInteract(IPointOfInterest.InputType type)
	// {
	// 	if (type == IPointOfInterest.InputType.Hold)
	// 	{
	// 		if (farmState != FarmState.ReadyToHarvest)
	// 		{
	// 			return $"Farm field is\nnot {FarmState.ReadyToHarvest.ToString()}!";
	// 		}
	// 		return "true";
	// 	}
	// 	return "unknown";
	// }

	public void UseItemOnThis(Item item)
	{
		if (item.ItemData.GetItemType() == Item.ItemType.Seed)
		{
			PlantSeed(item);
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		farmFieldHighlight = GetComponentInChildren<FarmFieldHighlight>(true);
		ActivateHighlight(false);
	}

	public void ActivateHighlight(bool _activate)
	{
		farmFieldHighlight.gameObject.SetActive(_activate);
	}

	public void PlantSeed(Item seedItem)
	{
		GameObject plantPrefab = seedItem.ItemData.GetPlantPrefab();
		if (plantPrefab == null) return;
		farmState = FarmState.Planted;
		plant = Instantiate(plantPrefab, transform.position, Quaternion.identity, transform).GetComponent<Plant>();
		plant.Initialize(this, seedItem);
		// farmPushTrigger.gameObject.SetActive(true);
		EnablePushColliders();
	}

	public void HarvestPlant()
	{
		Utils.CreateWorldTextPopup($"Harvest here!", transform, null, Color.white);
	}

	public void EnablePushColliders()
	{
		farmPushTriggerCollider.enabled = true;
		farmPushSolidCollider.enabled = true;

		Collider playerCollider = Player.Instance.GetComponent<Collider>();
		if (Utils.AreCollidersOverlapping(farmPushSolidCollider, playerCollider))
		{
			farmPushSolidCollider.isTrigger = true;
		}
		else
		{
			farmPushSolidCollider.isTrigger = false;
			farmPushTriggerCollider.enabled = false;
		}
	}

	// TODO: Will implement this once harvesting the plants is implemented
	public void DisablePushColliders()
	{
		farmPushTriggerCollider.enabled = false;
		farmPushSolidCollider.enabled = false;
		farmPushSolidCollider.isTrigger = true;
	}
}
