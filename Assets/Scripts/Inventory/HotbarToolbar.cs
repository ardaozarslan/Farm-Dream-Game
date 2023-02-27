using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarToolbar : Singleton<HotbarToolbar>
{
	public InventoryManager inventoryManager;
	public List<InventoryCell> inventoryCells;

	public int SelectedIndex { get; set; }

	private void Awake()
	{
		SelectedIndex = 0;
		inventoryManager = InventoryManager.Instance;
	}
	private void Start()
	{

		inventoryCells = inventoryManager.hotbarToolbarCells;
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			inventoryCell.Index = i;
			inventoryCell.CellType = InventoryCell.cellType.HotbarToolbar;
			inventoryCell.UpdateSelf();
		}
		UpdateSelection();

	}

	public void UpdateInventory() {
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			InventoryCell hotbarInventoryCell = HotbarInventory.Instance.inventoryCells[i];
			if (hotbarInventoryCell.InventoryItem == null) {
				if (inventoryCell.InventoryItem != null) {
					inventoryCell.DestroyItem();
					inventoryCell.InventoryItem = null;
					inventoryCell.UpdateSelf();
				}
			}
			else if (inventoryCell.InventoryItem != null)
			{
				if (InventoryItem.IsSameItem(inventoryCell.InventoryItem, hotbarInventoryCell.InventoryItem))
				{
					inventoryCell.InventoryItem.UpdateStackSizeWithoutCall(hotbarInventoryCell.InventoryItem.Item.stackSize);
					inventoryCell.InventoryItem.UpdateSelf();
				}
				else
				{
					inventoryCell.DestroyItem();
					inventoryCell.InventoryItem = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform).GetComponent<InventoryItem>();
					inventoryCell.InventoryItem.Item = new Item(hotbarInventoryCell.InventoryItem.Item.ItemData, hotbarInventoryCell.InventoryItem.Item.stackSize);
					inventoryCell.InventoryItem.UpdateStackSizeWithoutCall(hotbarInventoryCell.InventoryItem.Item.stackSize);
					inventoryCell.InventoryItem.InitializeCell(inventoryCell);
					inventoryCell.InventoryItem.UpdateSelf();
					inventoryCell.UpdateSelf();
				}
			}
			else {
				inventoryCell.InventoryItem = Instantiate(inventoryManager.inventoryItemPrefab, inventoryCell.transform).GetComponent<InventoryItem>();
				inventoryCell.InventoryItem.Item = new Item(hotbarInventoryCell.InventoryItem.Item.ItemData, hotbarInventoryCell.InventoryItem.Item.stackSize);
				inventoryCell.InventoryItem.UpdateStackSizeWithoutCall(hotbarInventoryCell.InventoryItem.Item.stackSize);
				inventoryCell.InventoryItem.InitializeCell(inventoryCell);
				inventoryCell.InventoryItem.UpdateSelf();
				inventoryCell.UpdateSelf();
			}
		}
	}

	public void ChangeSelection(int index) {
		SelectedIndex = index;
		UpdateSelection();
	}

	public void ChangeSelection(string name) {
		switch (name)
		{
			case "up":
				SelectedIndex = (SelectedIndex + 1) % inventoryCells.Count;
				break;
			case "down":
				SelectedIndex = (SelectedIndex - 1 + inventoryCells.Count) % inventoryCells.Count;
				break;
			default:
				break;
		}
		UpdateSelection();
	}

	public void UpdateSelection() {
		for (int i = 0; i < inventoryCells.Count; i++)
		{
			InventoryCell inventoryCell = inventoryCells[i];
			if (i == SelectedIndex) {
				inventoryCell.Select(true);
			}
			else {
				inventoryCell.Select(false);
			}
		}
	}

	public InventoryItem GetSelectedItem() {
		return inventoryCells[SelectedIndex].InventoryItem;
	}

	public void UseItem(ref Action useItemFunction) {
		InventoryItem inventoryItem = GetSelectedItem();
		if (inventoryItem != null) {
			useItemFunction = inventoryItem.Item.ItemData.OnUse;
		}
	}
}
