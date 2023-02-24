
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
	public InventoryManager inventoryManager;

	private int index;
	public int Index
	{
		get { return index; }
		set { this.index = value; }
	}
	private InventoryItem inventoryItem;
	public InventoryItem InventoryItem
	{
		get { return inventoryItem; }
		set { this.inventoryItem = value; }
	}

	public enum cellType
	{
		HotbarInventory = 100,
		MainInventory = 200,
		Other = 300
	}
	public cellType CellType;

	private void Awake() {
		inventoryManager = InventoryManager.Instance;
	}

	public void UpdateSelf()
	{

	}

}
