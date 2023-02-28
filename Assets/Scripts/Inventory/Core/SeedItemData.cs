using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedItemData", menuName = "ScriptableObjects/SeedItemData", order = 1)]
public class SeedItemData : ItemData
{
	// public CropItemData cropItemData;
	/// <summary>
	/// How long it takes for the plant to grow in seconds
	/// </summary>
	public float growTime = 10f;
	public int useCount = 1;
	public GameObject plantPrefab;

	public override GameObject GetPlantPrefab() { return plantPrefab; }
	public override float GetGrowTime() { return growTime; }

	public override void OnUse(Item item = null)
	{
		// Debug.Log("Using seed: " + name);
		if (item == null) return;
		if (InventoryManager.Instance.CheckForItem(new Item(item.ItemData, useCount)).stackSize < useCount)
		{
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
