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
		if (!inventoryItem.IsDraggable || eventData.button != PointerEventData.InputButton.Left) return;
		lastParent = transform.parent;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		inventoryItem.InventoryCell.gridLayoutGroup.enabled = false;
		canvasGroup.blocksRaycasts = false;
		rectTransform.position = eventData.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!inventoryItem.IsDraggable || eventData.button != PointerEventData.InputButton.Left) return;
		rectTransform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!inventoryItem.IsDraggable || eventData.button != PointerEventData.InputButton.Left) return;
		canvasGroup.blocksRaycasts = true;

		// Get the target cell
		GameObject target = eventData.pointerCurrentRaycast.gameObject;
		if (target == null)
		{
			transform.SetParent(lastParent);
			inventoryItem.InventoryCell.gridLayoutGroup.enabled = true;
			canvasGroup.blocksRaycasts = true;
			return;
		}
		InventoryCell targetCell = target.GetComponent<InventoryCell>();
		if (targetCell == null)
		{
			InventoryItem targetItem = target.GetComponent<InventoryItem>();
			if (targetItem != null)
			{
				targetCell = targetItem.InventoryCell;
			}
		}
		if (targetCell != null)
		{
			if (inventoryItem.InventoryCell.CellType == InventoryCell.cellType.HotbarInventory && targetCell.CellType == InventoryCell.cellType.HotbarInventory)
			{
				// Update the inventory cells
				transform.SetParent(targetCell.transform);
				if (targetCell.InventoryItem != null)
				{
					targetCell.InventoryItem.transform.SetParent(lastParent);
					targetCell.InventoryItem.InitializeCell(inventoryItem.InventoryCell);
				}
				else
				{
					inventoryItem.InventoryCell.InventoryItem = null;
				}
				// hotbarInventory.AssignItem(inventoryItem, targetCell);

				inventoryItem.InventoryCell.gridLayoutGroup.enabled = true;
				canvasGroup.blocksRaycasts = true;
				inventoryItem.InitializeCell(targetCell);
				
				hotbarInventory.UpdateInventory();
			}
			else if (inventoryItem.InventoryCell.CellType == InventoryCell.cellType.MainInventory && targetCell.CellType == InventoryCell.cellType.HotbarInventory && hotbarInventory.CanAssignItem(targetCell, inventoryItem))
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
