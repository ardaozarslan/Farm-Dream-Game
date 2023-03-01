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

	public Dictionary<string, List<InventoryItem>> inventoryItems = new Dictionary<string, List<InventoryItem>>();
	public Dictionary<string, int> inventoryItemCounts = new Dictionary<string, int>();

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

	public void ChangeHotBarSelection(int index)
	{
		hotbarToolbar.ChangeSelection(index);
	}

	public void ChangeHotBarSelection(string name)
	{
		hotbarToolbar.ChangeSelection(name);
	}

	public void InventoryCellChangeCall(InventoryCell changedInventoryCell)
	{
		inventoryCellChanged?.Invoke(changedInventoryCell);
	}

	public void InventoryCellChangeReceive(InventoryCell changedInventoryCell)
	{
		// Debug.Log("InventoryCellChangeReceive");
		CalculateItemCounts();
		SortMainInventory();
		UpdateHotbarInventory();

	}

	private void CalculateItemCounts()
	{
		// TODO: Optimize this so it doesn't have to loop through all the cells (maybe it is not a big problem?)
		// TODO: Find a better way to count items with unique id's (like tools: 52:23476283476)
		inventoryItems = new Dictionary<string, List<InventoryItem>>();
		for (int i = 0; i < mainInventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = mainInventoryCells[i];
			if (inventoryCell.InventoryItem)
			{
				if (inventoryItems.ContainsKey(inventoryCell.InventoryItem.Item.id))
				{
					inventoryItems[inventoryCell.InventoryItem.Item.id].Add(inventoryCell.InventoryItem);

				}
				else
				{
					inventoryItems.Add(inventoryCell.InventoryItem.Item.id, new List<InventoryItem> { inventoryCell.InventoryItem });
				}
			}
		}
		inventoryItemCounts = new Dictionary<string, int>();
		foreach (KeyValuePair<string, List<InventoryItem>> entry in inventoryItems)
		{
			int count = 0;
			for (int i = 0; i < entry.Value.Count; i++)
			{
				InventoryItem inventoryItem = entry.Value[i];
				if (inventoryItem.Item.stackType == Item.StackType.Stackable)
				{
					count += entry.Value[i].Item.stackSize;
				}
			}
			inventoryItemCounts.Add(entry.Key, count);
		}
		String inventoryItemsString = "";
		foreach (KeyValuePair<string, int> entry in inventoryItemCounts)
		{
			ItemData itemData = managersManager.itemManager.GetItemData(entry.Key);
			inventoryItemsString += itemData.name + ": " + entry.Value + "\n";
		}
		if (inventoryItemsString.Length > 0)
		{
			inventoryItemsString = inventoryItemsString.Substring(0, inventoryItemsString.Length - 1);
		}
		Debug.Log(inventoryItemsString);
	}

	private void UpdateHotbarInventory()
	{
		hotbarInventory.UpdateInventory();
	}

	public int GetTotalItemCount(string itemId)
	{
		if (inventoryItemCounts.ContainsKey(itemId))
		{
			return inventoryItemCounts[itemId];
		}
		else
		{
			return 0;
		}
	}

	public int GetTotalItemCount(InventoryItem inventoryItem)
	{
		return GetTotalItemCount(inventoryItem.Item.id);
	}

	public int GetTotalItemCount(ItemData itemData)
	{
		return GetTotalItemCount(itemData.id);
	}

	private void SortMainInventory()
	{
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
			// Debug.Log(item.name + " was added.");
		}
		Item remainingItem = new Item(item, remaining);
		return remainingItem;
	}

	/// <summary>
	/// First checks if there is enough items to remove. If there is, it removes the item and returns null. If there isn't, it returns the item with available stackSize.
	/// </summary>
	/// <param name="item">item to remove</param>
	public Item RemoveItem(Item item)
	{
		int count = item.stackSize;
		Item availableItem = CheckForItem(item);
		if (availableItem == null || availableItem.stackSize < count)
		{
			Debug.Log("Not enough items to remove.");
			return availableItem;
		}
		// REMOVE THE ITEM
		mainInventory.RemoveItem(item);
		return null;
	}

	public Item CheckForItem(Item item)
	{
		int available = GetTotalItemCount(item.ItemData);
		if (available >= item.stackSize)
		{
			return new Item(item, item.stackSize);
		}
		else
		{
			return new Item(item, available);
		}
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
