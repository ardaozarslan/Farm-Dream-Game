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

	public bool CanAssignItem(InventoryItem inventoryItem)
	{
		// TODO: Check if item is already in hotbar and if so, remove the other one
		// TODO: Check if item has any use case in hotbar, if not, return false
		return true;
	}

	public void AssignItem(InventoryItem inventoryItem, InventoryCell inventoryCell)
	{
		inventoryCell.InventoryItem = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform).GetComponent<InventoryItem>();
		inventoryCell.InventoryItem.Item = new Item(inventoryItem.Item.ItemData, inventoryManager.GetTotalItemCount(inventoryItem));
		inventoryCell.InventoryItem.UpdateStackSize(inventoryManager.GetTotalItemCount(inventoryItem));
		inventoryCell.InventoryItem.InitializeCell(inventoryCell);
		inventoryCell.InventoryItem.UpdateSelf();
		inventoryCell.UpdateSelf();

		// TODO: add a link between the hotbar item and the main inventory item so that when the main inventory item count changes, the hotbar item count changes as well
	}

}