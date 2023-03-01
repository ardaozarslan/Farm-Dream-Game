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
	public HotbarToolbar hotbarToolbar;

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
		hotbarToolbar = HotbarToolbar.Instance;

		UpdateInventory();
	}

	public bool CanAssignItem(InventoryCell inventoryCell, InventoryItem inventoryItem)
	{

		// TODO: Check if item has any use case in hotbar, if not, return false (or not :shrug:)
		return true;
	}

	public void AssignItem(InventoryItem inventoryItem, InventoryCell inventoryCell)
	{
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell currentInventoryCell = inventoryCells[i];
			if (currentInventoryCell.InventoryItem != null && currentInventoryCell.InventoryItem.Item.GetFullId() == inventoryItem.Item.GetFullId())
			{
				currentInventoryCell.DestroyItem();
				currentInventoryCell.InventoryItem = null;
				currentInventoryCell.UpdateSelf();
				break;
			}
		}
		if (inventoryCell.InventoryItem != null)
		{
			inventoryCell.DestroyItem();
			inventoryCell.InventoryItem = null;
			inventoryCell.UpdateSelf();
		}

		inventoryCell.InventoryItem = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform).GetComponent<InventoryItem>();
		// Debug.Log("inventoryCell.InventoryItem1: " + inventoryCell.InventoryItem);
		inventoryCell.InventoryItem.Item = new Item(inventoryItem.Item.ItemData, inventoryManager.GetTotalItemCount(inventoryItem));
		inventoryCell.InventoryItem.Item.id = inventoryItem.Item.id;
		// Debug.Log("inventoryCell.InventoryItem2: " + inventoryCell.InventoryItem);
		if (inventoryCell.InventoryItem.Item.ItemData.GetStackType() == Item.StackType.Stackable)
		{
			inventoryCell.InventoryItem.UpdateStackSize(inventoryManager.GetTotalItemCount(inventoryItem));
			// Debug.Log("inventoryCell.InventoryItem3: " + inventoryCell.InventoryItem);
		}
		inventoryCell.InventoryItem.InitializeCell(inventoryCell);
		Debug.Log("inventoryCell.InventoryItem4: " + inventoryCell.InventoryItem);
		inventoryCell.InventoryItem.UpdateSelf();
		Debug.Log("inventoryCell.InventoryItem5: " + inventoryCell.InventoryItem);
		// inventoryCell.InventoryItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
		inventoryCell.UpdateSelf();

		UpdateInventory();
	}

	public void UpdateInventory()
	{
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			if (inventoryCell.InventoryItem != null)
			{
				// Debug.Log("there is an item in the hotbar inventory cell: " + i);
				InventoryItem inventoryItem = inventoryCell.InventoryItem;
				if (inventoryItem.Item.ItemData.GetStackType() != Item.StackType.Stackable)
				{
					inventoryItem.UpdateSelf();
					inventoryCell.UpdateSelf();
					continue;
				}
				int count = inventoryManager.GetTotalItemCount(inventoryItem);
				// Debug.Log("count: " + count);
				if (count == 0)
				{
					inventoryCell.DestroyItem();
					inventoryCell.InventoryItem = null;
					inventoryCell.UpdateSelf();
				}
				else
				{
					inventoryItem.UpdateStackSizeWithoutCall(count);
					inventoryItem.UpdateSelf();
					inventoryCell.UpdateSelf();
				}
			}
		}

		UpdateHotbarToolbar();
	}

	public void UpdateHotbarToolbar()
	{
		hotbarToolbar.UpdateInventory();
	}

}