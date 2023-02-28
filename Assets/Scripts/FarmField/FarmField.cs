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
		plant = Instantiate(plantPrefab, transform.position, Quaternion.identity, transform).GetComponent<Plant>();
		plant.Initialize(this, seedItem);
		// farmPushTrigger.gameObject.SetActive(true);
		EnablePushColliders();
	}

	public void EnablePushColliders()
	{
		farmPushTriggerCollider.enabled = true;
		farmPushSolidCollider.enabled = true;
	}

	// TODO: Will implement this once cropping the plants is implemented
	public void DisablePushColliders()
	{
		farmPushTriggerCollider.enabled = false;
		farmPushSolidCollider.enabled = false;
		farmPushSolidCollider.isTrigger = true;
	}
}
