using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmField : MonoBehaviour
{
	[HideInInspector]
	public FarmFieldHighlight farmFieldHighlight;
	public FarmPushTrigger farmPushTrigger;
	public Collider farmPushTriggerCollider;
	public Collider farmPushSolidCollider;
	public Plant plant;
	public FarmState farmState = FarmState.Empty;

	public enum FarmState {
		Empty = 0,
		Planted = 10,
		Growing = 20,
		ReadyToHarvest = 30,
		// Harvested = 40
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

	public void EnablePushColliders()
	{
		farmPushTriggerCollider.enabled = true;
		farmPushSolidCollider.enabled = true;

		Collider playerCollider = Player.Instance.GetComponent<Collider>();
		if (Utils.AreCollidersOverlapping(farmPushSolidCollider, playerCollider)) {
			farmPushSolidCollider.isTrigger = true;
		} else {
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
