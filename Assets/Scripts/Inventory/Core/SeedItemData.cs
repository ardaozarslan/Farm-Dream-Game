using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedItemData", menuName = "ScriptableObjects/SeedItemData", order = 1)]
public class SeedItemData : ItemData
{
	// public CropItemData cropItemData;

	[Range(1f, 60f)]
	[Tooltip("How long it takes for the plant to grow in seconds")]
	public float growTime = 10f;
	[Range(0f, 5f)]
	[Tooltip("How much the grow time can vary. Only decreases the individual plant model grow time. Ripening is not affected.")]
	public float growTimeVariance = 1f;
	[Range(0.2f, 3f)]
	[Tooltip("Scale of the original plant model")]
	public float growScale = 1f;
	[Range(0f, 1f)]
	[Tooltip("How much the scale of the plant can vary")]
	public float growScaleVariance = 0.1f;
	[Range(1, 10)]
	[Tooltip("How many seeds are used to plant")]
	public int useCount = 1;

	public GameObject plantPrefab;

	public override GameObject GetPlantPrefab() { return plantPrefab; }
	public override float GetGrowTime() { return growTime; }
	public override float GetGrowTimeVariance() { return growTimeVariance; }
	public override float GetGrowScale() { return growScale; }
	public override float GetGrowScaleVariance() { return growScaleVariance; }


	public override void OnUse(Item item = null)
	{
		// Debug.Log("Using seed: " + name);
		if (item == null) return;
		if (InventoryManager.Instance.CheckForItem(new Item(item.ItemData, useCount)).stackSize < useCount)
		{
			Utils.CreateWorldTextPopup("Not enough seeds!", Player.Instance.playerDebugObject.transform, null, Color.red);
			return;
		}

		bool success = true;
		Player.Instance.PlantSeed(item, ref success);
		if (success)
		{
			InventoryManager.Instance.RemoveItem(new Item(item.ItemData, useCount));
		}

	}
}
