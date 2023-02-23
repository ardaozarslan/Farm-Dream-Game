using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
	public ManagersManager managersManager;
	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;

	public List<InventoryCell> mainInventoryCells = new List<InventoryCell>();
	public List<InventoryCell> hotbarInventoryCells = new List<InventoryCell>();

	private void Start() {
		managersManager = ManagersManager.Instance;
		mainInventory = MainInventory.Instance;
		hotbarInventory = HotbarInventory.Instance;

		MainInventory.Instance.onItemAdded += UpdateInventoryCell;
		MainInventory.Instance.onItemRemoved += UpdateInventoryCell;

		HotbarInventory.Instance.onItemAdded += UpdateInventoryCell;
		HotbarInventory.Instance.onItemRemoved += UpdateInventoryCell;
	}

	public void UpdateInventoryCell(InventoryCell inventoryCell) {
		inventoryCell.UpdateSelf();
	}

	// public bool Add(Item item)
	// {
	// 	if (!item.isDefaultItem)
	// 	{
	// 		if (inventory.Count >= space)
	// 		{
	// 			Debug.Log("Not enough room.");
	// 			return false;
	// 		}

	// 		inventory.Add(item);
	// 		Debug.Log(item.name + " was added.");
	// 	}

	// 	return true;
	// }

	// public void Remove(Item item)
	// {
	// 	inventory.Remove(item);
	// 	Debug.Log(item.name + " was removed.");
	// }
}
