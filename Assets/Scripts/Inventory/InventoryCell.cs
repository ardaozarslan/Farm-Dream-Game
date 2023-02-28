
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
	[HideInInspector]
	public Image image;

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
		image = GetComponent<Image>();
		inventoryManager = InventoryManager.Instance;
		gridLayoutGroup = GetComponent<GridLayoutGroup>();
	}

	public void UpdateSelf()
	{
		if (inventoryItem != null && inventoryItem.Item.stackSize <= 0)
		{
			// Utils.Instance.InvokeNextFrame(DestroyItem);
			DestroyItem();
		}
	}

	public void DestroyItem()
	{
		if (inventoryItem != null)
		{
			Destroy(inventoryItem.gameObject);
			inventoryItem = null;
			UpdateSelf();
		}
	}

	public void Select(bool isSelected)
	{
		if (isSelected)
		{
			image.color = Color.HSVToRGB(0f, 0.6f, 1f);
			var tempColor = image.color;
			tempColor.a = 1f;
			image.color = tempColor;
		}
		else
		{
			image.color = Color.HSVToRGB(0f, 0f, 1f);
			var tempColor = image.color;
			tempColor.a = 0.5f;
			image.color = tempColor;
		}
	}

}
