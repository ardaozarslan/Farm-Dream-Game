using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
	public Image itemImage;
	public TextMeshProUGUI itemStackSizeText;
	private InventoryManager inventoryManager;
	private ItemManager itemManager;

	private bool isDraggable = true;
	public bool IsDraggable
	{
		get { return isDraggable; }
		private set { this.isDraggable = value; }
	}

	private Item item;
	public Item Item
	{
		get { return item; }
		set { this.item = value; }
	}

	private InventoryCell inventoryCell;
	public InventoryCell InventoryCell
	{
		get { return inventoryCell; }
		set { this.inventoryCell = value; }
	}

	private void Awake()
	{
		inventoryManager = InventoryManager.Instance;
		itemManager = ItemManager.Instance;
		item = new Item(itemManager.GetItemData("Wheat"), 1);
	}

	private void Start()
	{

	}

	public void SetDraggable(bool _isDraggable)
	{
		isDraggable = _isDraggable;
	}

	public void InitializeCell(InventoryCell cell)
	{
		inventoryCell = cell;
		cell.InventoryItem = this;
		UpdateSelf();
		inventoryCell.UpdateSelf();

		if (inventoryCell.CellType == InventoryCell.cellType.HotbarInventory)
		{
			isDraggable = false;
		}
		else if (inventoryCell.CellType == InventoryCell.cellType.MainInventory)
		{
			isDraggable = true;
		}
	}


	public void UpdateStackSize(int amount)
	{
		item.stackSize = amount;
		if (item.stackSize <= 0)
		{
			// TODO: Destroy this object
			item.stackSize = 0;
			item = null;
		}
		UpdateSelf();
		Debug.Log("stack size: " + item.stackSize);
		if (inventoryCell != null)
		{
			inventoryManager.InventoryCellChangeCall(inventoryCell);
			inventoryCell.UpdateSelf();
		}
	}


	public void UpdateSelf()
	{
		itemImage.sprite = item.sprite;
		itemImage.color = Color.white;
		if (item.isStackable)
		{
			itemStackSizeText.enabled = true;
		}
		else
		{
			itemStackSizeText.enabled = false;
		}
		itemStackSizeText.text = item.stackSize.ToString();
	}


}
