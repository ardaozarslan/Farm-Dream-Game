using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarToolbar : Singleton<HotbarToolbar>
{
	public InventoryManager inventoryManager;
	public List<InventoryCell> inventoryCells;

	private void Awake()
	{
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
}
