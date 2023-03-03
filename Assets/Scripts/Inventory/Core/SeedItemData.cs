using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GD.MinMaxSlider;

[CreateAssetMenu(fileName = "SeedItemData", menuName = "ScriptableObjects/SeedItemData", order = 1)]
public class SeedItemData : BaseItemData
{
	[ReadOnly]
	public Item.ItemType itemType = Item.ItemType.Seed;
	public Item.StackType stackType = Item.StackType.Stackable;

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
	[MinMaxSlider(0, 10)]
	[Tooltip("How many items are harvested from the plant")]
	public Vector2Int harvestAmound = new Vector2Int(1, 3);
	[MinMaxSlider(0, 10)]
	[Tooltip("How many seeds are returned from the plant")]
	public Vector2Int returnSeedAmount = new Vector2Int(0, 1);

	public CropItemData harvestItemData;

	public GameObject plantPrefab;

	public override Item.ItemType GetItemType() { return itemType; }
	public override Item.StackType GetStackType() { return stackType; }
	public GameObject GetPlantPrefab() { return plantPrefab; }
	public float GetGrowTime() { return growTime; }
	public float GetGrowTimeVariance() { return growTimeVariance; }
	public float GetGrowScale() { return growScale; }
	public float GetGrowScaleVariance() { return growScaleVariance; }
	
	public CropItemData GetHarvestItemData() { return harvestItemData; }
	public Vector2Int GetHarvestAmount() { return harvestAmound; }
	public Vector2Int GetReturnSeedAmount() { return returnSeedAmount; }


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
