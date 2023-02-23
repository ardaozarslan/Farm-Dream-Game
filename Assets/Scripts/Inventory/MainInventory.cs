using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainInventory : Singleton<MainInventory>
{
	public InventoryManager inventoryManager;
	public List<InventoryCell> inventoryCells;
	public HotbarInventory hotbarInventory;

	public event Action<InventoryCell> onItemAdded;
	public event Action<InventoryCell> onItemRemoved;

	private void Start()
	{
		inventoryManager = InventoryManager.Instance;
		inventoryCells = inventoryManager.mainInventoryCells;
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			inventoryCell.Index = i;
			inventoryCell.UpdateSelf();
		}

		hotbarInventory = HotbarInventory.Instance;
	}


	public bool AddItem(Item item)
	{
		int count = item.stackSize;
		// Find an existing stack for the item
		for (int index = 0; index < inventoryCells.Count; index++)
		{
			InventoryCell inventoryCell = inventoryCells[index];
			if (inventoryCell.item != null && inventoryCell.item.id == item.id && inventoryCell.StackSize < item.maxStackSize)
			{
				// Debug.Log("Found existing stack for item: " + item.name + " at index: " + index);
				int space = item.maxStackSize - inventoryCell.StackSize;
				int addCount = Mathf.Min(count, space);
				inventoryCell.StackSize += addCount;
				count -= addCount;
				onItemAdded?.Invoke(inventoryCell);
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
			if (inventoryCell.item == null)
			{
				int addCount = Mathf.Min(count, item.maxStackSize);
				inventoryCell.item = item;
				inventoryCell.StackSize = addCount;
				onItemAdded?.Invoke(inventoryCell);
				count -= addCount;
				if (count == 0)
				{
					return true;
				}
			}
		}
		item.stackSize = count;
		hotbarInventory.AddItem(item);

		return false;
	}

	public void RemoveItem(InventoryCell inventoryCell)
	{
		inventoryCell.StackSize = 0;
		// items[stack.x, stack.y] = stack;
		onItemRemoved?.Invoke(inventoryCell);
	}
}