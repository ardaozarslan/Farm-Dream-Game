using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarInventory : Singleton<HotbarInventory>
{
	public InventoryManager inventoryManager;
	public List<InventoryCell> inventoryCells;
	public MainInventory mainInventory;

	private void Awake()
	{
		inventoryManager = InventoryManager.Instance;
	}
	private void Start()
	{

		inventoryCells = inventoryManager.hotbarInventoryCells;
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			inventoryCell.Index = i;
			inventoryCell.CellType = InventoryCell.cellType.HotbarInventory;
			inventoryCell.UpdateSelf();
		}

		mainInventory = MainInventory.Instance;
	}

	public bool AddItem(Item item)
	{
		int count = item.stackSize;
		// Find an existing stack for the item
		for (int index = 0; index < inventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = inventoryCells[index];
			if (inventoryCell.InventoryItem != null && inventoryCell.InventoryItem.Item.id == item.id && inventoryCell.InventoryItem.Item.stackSize < item.maxStackSize)
			{
				// Debug.Log("Found existing stack for item: " + item.name + " at index: " + index);
				int space = item.maxStackSize - inventoryCell.InventoryItem.Item.stackSize;
				int addCount = Mathf.Min(count, space);
				inventoryCell.InventoryItem.UpdateStackSize(inventoryCell.InventoryItem.Item.stackSize + addCount);
				count -= addCount;
				if (count == 0)
				{
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
				GameObject inventoryItemObject = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform);
				InventoryItem inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();
				item.stackSize = count;
				inventoryItem.Item = item;
				inventoryItem.UpdateSelf();
				count -= addCount;
				if (count == 0)
				{
					return true;
				}
			}
		}
		item.stackSize = count;
		// hotbarInventory.AddItem(item);

		return false;
	}

}