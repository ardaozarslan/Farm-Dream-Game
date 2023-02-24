using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
	public ManagersManager managersManager;
	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;

	public GameObject inventoryItemPrefab;

	public List<InventoryCell> mainInventoryCells = new List<InventoryCell>();
	public List<InventoryCell> hotbarInventoryCells = new List<InventoryCell>();

	public delegate void InventoryCellChanged(InventoryCell inventoryCell);
	public event InventoryCellChanged inventoryCellChanged;

	private void Start()
	{
		managersManager = ManagersManager.Instance;
		mainInventory = MainInventory.Instance;
		hotbarInventory = HotbarInventory.Instance;
	}

	private void OnEnable() {
		inventoryCellChanged += InventoryCellChangeReceive;
	}

	private void OnDisable() {
		inventoryCellChanged -= InventoryCellChangeReceive;
	}

	public void InventoryCellChangeCall(InventoryCell inventoryCell)
	{
		inventoryCellChanged?.Invoke(inventoryCell);
	}

	public void InventoryCellChangeReceive(InventoryCell inventoryCell)
	{
		Debug.Log("InventoryCellChangeReceive");
	}


	public Item AddItem(Item item) {
		int count = item.stackSize;
		int availableSpace = CheckForSpace(item);
		int remaining = 0;
		if (availableSpace < count) {
			remaining = count - availableSpace;
			item.stackSize = availableSpace;
			mainInventory.AddItem(item);
			Debug.Log("Not enough room. " + remaining + " more needed.");
		}
		else {
			mainInventory.AddItem(item);
			Debug.Log(item.name + " was added.");

			for (int i = 0; i < mainInventoryCells.Count; i++)
			{
				InventoryCell inventoryCell = mainInventoryCells[i];
				if (inventoryCell.InventoryItem) {
					Debug.Log(inventoryCell.InventoryItem.Item.name + " " + inventoryCell.InventoryItem.Item.stackSize);
				}
			}
		}
		Item remainingItem = new Item(item, remaining);
		return remainingItem;
	}

	public int CheckForSpace(Item item) {
		int count = item.stackSize;
		int availableSpace = 0;
		// Find an existing stack for the item
		for (int index = 0; index < mainInventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = mainInventoryCells[index];
			if (inventoryCell.InventoryItem != null && inventoryCell.InventoryItem.Item.id == item.id && inventoryCell.InventoryItem.Item.stackSize < item.maxStackSize)
			{
				// Debug.Log("Found existing stack for item: " + item.name + " at index: " + index);
				int space = item.maxStackSize - inventoryCell.InventoryItem.Item.stackSize;
				availableSpace += space;
			}
		}

		// Find an empty slot for the item
		for (int index = 0; index < mainInventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = mainInventoryCells[index];
			if (inventoryCell.InventoryItem == null)
			{
				availableSpace += item.maxStackSize;
			}
		}

		return availableSpace;
	}


	// public bool Add(Item item)
	// {
	// 	if (!item.isDefaultItem)
	// 	{
	// 		if (inventory.Count >= space)
	// 		{
	// 			Debug.Log("Not enough room.");
	// 			return false;
	// 		}

	// 		inventory.Add(item);
	// 		Debug.Log(item.name + " was added.");
	// 	}

	// 	return true;
	// }

	// public void Remove(Item item)
	// {
	// 	inventory.Remove(item);
	// 	Debug.Log(item.name + " was removed.");
	// }
}
