using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainInventory : Singleton<MainInventory>
{
	public InventoryManager inventoryManager;
	public List<InventoryCell> inventoryCells;
	public HotbarInventory hotbarInventory;


	private void Awake()
	{
		inventoryManager = InventoryManager.Instance;
	}
	private void Start()
	{

		inventoryCells = inventoryManager.mainInventoryCells;
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			inventoryCell.Index = i;
			inventoryCell.CellType = InventoryCell.cellType.MainInventory;
			inventoryCell.UpdateSelf();
		}

		hotbarInventory = HotbarInventory.Instance;
	}

	public bool AddItem(Item item)
	{
		int count = item.stackSize;
		if (count == 0) {
			return false;
		}
		// Find an existing stack for the item
		for (int index = 0; index < inventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = inventoryCells[index];
			if (inventoryCell.InventoryItem != null && inventoryCell.InventoryItem.Item.GetFullId() == item.GetFullId() && inventoryCell.InventoryItem.Item.stackSize < item.maxStackSize)
			{
				// Debug.Log("Found existing stack for item: " + item.name + " at index: " + index + " with stack size: " + inventoryCell.InventoryItem.Item.stackSize);
				// Debug.Log("remaining count: " + count + " and space: " + (item.maxStackSize - inventoryCell.InventoryItem.Item.stackSize));
				int space = item.maxStackSize - inventoryCell.InventoryItem.Item.stackSize;
				int addCount = Mathf.Min(count, space);
				// Debug.Log("adding count: " + addCount);
				inventoryCell.InventoryItem.UpdateStackSizeWithoutCall(inventoryCell.InventoryItem.Item.stackSize + addCount);
				count -= addCount;
				if (count == 0)
				{
					inventoryManager.InventoryCellChangeCall(inventoryCell);
					return true;
				}
			}
		}

		// Find an empty slot for the item
		for (int index = 0; index < inventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = inventoryCells[index];
			if (inventoryCell.InventoryItem == null)
			{
				int addCount = Mathf.Min(count, item.maxStackSize);
				// Debug.Log("Found empty slot for item: " + item.name + " at index: " + index + " with stack size: " + addCount + " where the inventory cell's index is " + inventoryCell.Index);
				GameObject inventoryItemObject = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform);
				InventoryItem inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();
				inventoryItem.Item = new Item(item, addCount);
				inventoryItem.InitializeCell(inventoryCell);
				// Debug.Log("add item: " + item.name + " at index: " + index + " with stack size: " + addCount + " where the inventory cell's index is " + inventoryCell.Index);
				inventoryCell.InventoryItem.UpdateStackSizeWithoutCall(addCount);
				count -= addCount;
				// Debug.Log("Added item: " + item.name + " at index: " + index + " with stack size: " + inventoryCell.InventoryItem.Item.stackSize);
				if (count == 0)
				{
					inventoryManager.InventoryCellChangeCall(inventoryCell);
					return true;
				}
			}
		}
		// hotbarInventory.AddItem(item);
		return false;
	}

	public bool RemoveItem(Item item)
	{
		List<InventoryCell> inventoryCellsWithSameItem = new List<InventoryCell>();
		for (int index = 0; index < inventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = inventoryCells[index];
			if (inventoryCell.InventoryItem != null && inventoryCell.InventoryItem.Item.GetFullId() == item.GetFullId())
			{
				inventoryCellsWithSameItem.Add(inventoryCell);
			}
		}
		if (inventoryCellsWithSameItem.Count > 0)
		{
			inventoryCellsWithSameItem.Sort((InventoryCell ic1, InventoryCell ic2) =>
			{
				InventoryItem i1 = ic1.InventoryItem;
				InventoryItem i2 = ic2.InventoryItem;
				return i1.Item.stackSize.CompareTo(i2.Item.stackSize) * 1;
			});
			int count = item.stackSize;
			for (int i = 0; i < inventoryCellsWithSameItem.Count; i++)
			{
				InventoryCell currentInventoryCell = inventoryCellsWithSameItem[i];
				int removeCount = Mathf.Min(count, currentInventoryCell.InventoryItem.Item.stackSize);
				count -= removeCount;
				currentInventoryCell.InventoryItem.UpdateStackSize(currentInventoryCell.InventoryItem.Item.stackSize - removeCount);
				if (count == 0)
				{
					return true;
				}
			}
			return false;
		}
		else {
			return false;
		}
	}

	public void SortInventoryCells()
	{
		List<Transform> children = new List<Transform>();
		for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
		{
			Transform child = gameObject.transform.GetChild(i);
			children.Add(child);
			child.SetParent(null, false);
		}
		children.Sort((Transform t1, Transform t2) =>
		{
			InventoryCell ic1 = t1.GetComponent<InventoryCell>();
			InventoryCell ic2 = t2.GetComponent<InventoryCell>();

			InventoryItem i1 = ic1.InventoryItem;
			InventoryItem i2 = ic2.InventoryItem;

			if (i1 == null && i2 != null) return 1;
			if (i2 == null && i1 != null) return -1;
			if (i1 == null && i2 == null) return 0;

			// TODO: Sort durability items by durability
			int result = i1.Item.itemType.CompareTo(i2.Item.itemType);
			result = result == 0 ? i1.Item.GetFullId().CompareTo(i2.Item.GetFullId()) : result;
			result = result == 0 ? i1.Item.stackSize.CompareTo(i2.Item.stackSize) * -1 : result;
			return result;
		});
		foreach (Transform child in children)
		{
			child.SetParent(gameObject.transform, false);
		}
	}

}