using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemClickHandler : MonoBehaviour, IPointerDownHandler
{
	private InventoryItem inventoryItem;

	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;

	public MainInventory mainInventory;
	public HotbarInventory hotbarInventory;

	private Transform lastParent;

	private void Awake()
	{
		inventoryItem = GetComponent<InventoryItem>();
		rectTransform = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		mainInventory = MainInventory.Instance;
		hotbarInventory = HotbarInventory.Instance;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (inventoryItem.InventoryCell.CellType == InventoryCell.cellType.HotbarInventory)
			{
				inventoryItem.InventoryCell.DestroyItem();
				inventoryItem.InventoryCell.InventoryItem = null;
				inventoryItem.InventoryCell.UpdateSelf();
				hotbarInventory.UpdateInventory();
			}
		}
	}
}

