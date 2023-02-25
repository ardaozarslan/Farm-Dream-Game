using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!inventoryItem.IsDraggable) return;
		lastParent = transform.parent;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		inventoryItem.InventoryCell.gridLayoutGroup.enabled = false;
		canvasGroup.blocksRaycasts = false;
		rectTransform.position = eventData.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!inventoryItem.IsDraggable) return;
		rectTransform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!inventoryItem.IsDraggable) return;
		canvasGroup.blocksRaycasts = true;

		// Get the target cell
		GameObject target = eventData.pointerCurrentRaycast.gameObject;
		InventoryCell targetCell = target.GetComponent<InventoryCell>();
		if (targetCell != null && targetCell.CellType == InventoryCell.cellType.HotbarInventory && hotbarInventory.CanAssignItem(inventoryItem))
		{
			// Update the inventory cells
			transform.SetParent(lastParent);
			hotbarInventory.AssignItem(inventoryItem, targetCell);
			inventoryItem.InventoryCell.gridLayoutGroup.enabled = true;
			canvasGroup.blocksRaycasts = true;
		}
		else
		{
			// Return the item to its original position
			transform.SetParent(lastParent);
			inventoryItem.InventoryCell.gridLayoutGroup.enabled = true;
			canvasGroup.blocksRaycasts = true;
		}
	}

	// public void SetItem(Item item, int stackSize, int cellIndex)
	// {
	// 	this.item = item;
	// 	this.stackSize = stackSize;
	// 	this.cellIndex = cellIndex;
	// }
}
