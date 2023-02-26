using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
	public ManagersManager managersManager;
	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;
	public HotbarToolbar hotbarToolbar;

	public GameObject inventoryItemPrefab;

	public List<InventoryCell> mainInventoryCells = new List<InventoryCell>();
	public List<InventoryCell> hotbarInventoryCells = new List<InventoryCell>();
	public List<InventoryCell> hotbarToolbarCells = new List<InventoryCell>();

	public Dictionary<ItemData, List<InventoryItem>> inventoryItems = new Dictionary<ItemData, List<InventoryItem>>();
	public Dictionary<ItemData, int> inventoryItemCounts = new Dictionary<ItemData, int>();

	public delegate void InventoryCellChanged(InventoryCell inventoryCell);
	public event InventoryCellChanged inventoryCellChanged;

	private void Start()
	{
		managersManager = ManagersManager.Instance;
		mainInventory = MainInventory.Instance;
		hotbarInventory = HotbarInventory.Instance;
		hotbarToolbar = HotbarToolbar.Instance;

		CalculateItemCounts();
	}

	private void OnEnable()
	{
		inventoryCellChanged += InventoryCellChangeReceive;
	}

	private void OnDisable()
	{
		inventoryCellChanged -= InventoryCellChangeReceive;
	}

	public void InventoryCellChangeCall(InventoryCell changedInventoryCell)
	{
		inventoryCellChanged?.Invoke(changedInventoryCell);
	}

	public void InventoryCellChangeReceive(InventoryCell changedInventoryCell)
	{
		Debug.Log("InventoryCellChangeReceive");
		CalculateItemCounts();
		SortMainInventory();
		UpdateHotbarInventory();

	}

	private void CalculateItemCounts()
	{
		// TODO: Optimize this so it doesn't have to loop through all the cells
		// TODO: Find a better way to count items with unique id's (like tools: 52:23476283476)
		inventoryItems = new Dictionary<ItemData, List<InventoryItem>>();
		for (int i = 0; i < mainInventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = mainInventoryCells[i];
			if (inventoryCell.InventoryItem)
			{
				if (inventoryItems.ContainsKey(inventoryCell.InventoryItem.Item.ItemData))
				{
					inventoryItems[inventoryCell.InventoryItem.Item.ItemData].Add(inventoryCell.InventoryItem);
				}
				else
				{
					inventoryItems.Add(inventoryCell.InventoryItem.Item.ItemData, new List<InventoryItem> { inventoryCell.InventoryItem });
				}
			}
		}
		inventoryItemCounts = new Dictionary<ItemData, int>();
		foreach (KeyValuePair<ItemData, List<InventoryItem>> entry in inventoryItems)
		{
			int count = 0;
			for (int i = 0; i < entry.Value.Count; i++)
			{
				InventoryItem inventoryItem = entry.Value[i];
				if (inventoryItem.Item.isStackable)
				{
					count += entry.Value[i].Item.stackSize;
				}
			}
			inventoryItemCounts.Add(entry.Key, count);
		}
		String inventoryItemsString = "";
		foreach (KeyValuePair<ItemData, int> entry in inventoryItemCounts)
		{
			inventoryItemsString += entry.Key.name + ": " + entry.Value + "\n";
		}
		Debug.Log(inventoryItemsString);
	}

	private void UpdateHotbarInventory() {
		hotbarInventory.UpdateInventory();
	}

	public int GetTotalItemCount(ItemData itemData)
	{
		if (inventoryItemCounts.ContainsKey(itemData))
		{
			return inventoryItemCounts[itemData];
		}
		else
		{
			return 0;
		}
	}

	public int GetTotalItemCount(InventoryItem inventoryItem)
	{
		return GetTotalItemCount(inventoryItem.Item.ItemData);
	}

	private void SortMainInventory() {
		mainInventory.SortInventoryCells();
	}


	public Item AddItem(Item item)
	{
		int count = item.stackSize;
		int availableSpace = CheckForSpace(item);
		int remaining = 0;
		if (availableSpace < count)
		{
			remaining = count - availableSpace;
			item.stackSize = availableSpace;
			mainInventory.AddItem(item);
			Debug.Log("Not enough room. " + remaining + " more needed.");
		}
		else
		{
			mainInventory.AddItem(item);
			Debug.Log(item.name + " was added.");

			// Logs all InventoryCells with items and their stackSize in them
			// for (int i = 0; i < mainInventoryCells.Count; i++)
			// {
			// 	InventoryCell inventoryCell = mainInventoryCells[i];
			// 	if (inventoryCell.InventoryItem) {
			// 		Debug.Log(inventoryCell.InventoryItem.Item.name + " " + inventoryCell.InventoryItem.Item.stackSize);
			// 	}
			// }
		}
		Item remainingItem = new Item(item, remaining);
		return remainingItem;
	}

	public int CheckForSpace(Item item)
	{
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

}
