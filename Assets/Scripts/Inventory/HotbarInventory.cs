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
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell currentInventoryCell = inventoryCells[i];
			if (currentInventoryCell.InventoryItem != null && currentInventoryCell.InventoryItem.Item.id == inventoryItem.Item.id)
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
		// TODO: Check if item has any use case in hotbar, if not, return false (or not :shrug:)
		return true;
	}

	public void AssignItem(InventoryItem inventoryItem, InventoryCell inventoryCell)
	{
		inventoryCell.InventoryItem = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform).GetComponent<InventoryItem>();
		inventoryCell.InventoryItem.Item = new Item(inventoryItem.Item.ItemData, inventoryManager.GetTotalItemCount(inventoryItem));
		inventoryCell.InventoryItem.UpdateStackSize(inventoryManager.GetTotalItemCount(inventoryItem));
		inventoryCell.InventoryItem.InitializeCell(inventoryCell);
		inventoryCell.InventoryItem.UpdateSelf();
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