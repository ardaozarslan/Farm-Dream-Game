
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
	public InventoryManager inventoryManager;
	[HideInInspector]
	public GridLayoutGroup gridLayoutGroup;

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
		MainInventory = 100,
		HotbarInventory = 200,
		HotbarToolbar = 300,
		Other = 400
	}
	public cellType CellType;

	private void Awake()
	{
		inventoryManager = InventoryManager.Instance;
		gridLayoutGroup = GetComponent<GridLayoutGroup>();
	}

	public void UpdateSelf()
	{

	}

	public void DestroyItem() {
		if (inventoryItem != null) {
			Destroy(inventoryItem.gameObject);
			inventoryItem = null;
			UpdateSelf();
		}
	}

}
