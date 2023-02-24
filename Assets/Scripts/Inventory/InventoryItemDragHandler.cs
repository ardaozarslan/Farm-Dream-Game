using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : MonoBehaviour //, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// private Item item;
	// private int stackSize;
	// private int cellIndex;

	// private RectTransform rectTransform;
	// private CanvasGroup canvasGroup;

	// public MainInventory mainInventory;
	// public HotbarInventory hotbarInventory;

	// private void Awake()
	// {
	// 	rectTransform = GetComponent<RectTransform>();
	// 	canvasGroup = GetComponent<CanvasGroup>();
	// }

	// private void Start() {
	// 	mainInventory = MainInventory.Instance;
	// 	hotbarInventory = HotbarInventory.Instance;
	// }

	// public void OnBeginDrag(PointerEventData eventData)
	// {
	// 	if (item == null || stackSize == 0) return;
	// 	canvasGroup.blocksRaycasts = false;
	// 	rectTransform.SetAsLastSibling();
	// 	rectTransform.position = eventData.position;
	// }

	// public void OnDrag(PointerEventData eventData)
	// {
	// 	if (item == null || stackSize == 0) return;
	// 	rectTransform.position = eventData.position;
	// }

	// public void OnEndDrag(PointerEventData eventData)
	// {
	// 	if (item == null || stackSize == 0) return;
	// 	canvasGroup.blocksRaycasts = true;

	// 	// Get the target cell
	// 	GameObject target = eventData.pointerCurrentRaycast.gameObject;
	// 	InventoryCell targetCell = target.GetComponent<InventoryCell>();
	// 	if (targetCell != null && targetCell.CellType == InventoryCell.cellType.HotbarInventory && hotbarInventory.CanDropItem(item, stackSize, cellIndex, targetCell.Index))
	// 	{
	// 		// Update the inventory cells
	// 		hotbarInventory.DropItem(item, stackSize, cellIndex, targetCell.Index);
	// 		targetCell.UpdateSelf();
	// 		InventoryCell sourceCell = mainInventory.inventoryCells[cellIndex];
	// 		sourceCell.UpdateSelf();
	// 	}
	// 	else
	// 	{
	// 		// Return the item to its original position
	// 		rectTransform.position = InventoryUtils.GetCellCenter(cellIndex);
	// 	}
	// }

	// public void SetItem(Item item, int stackSize, int cellIndex)
	// {
	// 	this.item = item;
	// 	this.stackSize = stackSize;
	// 	this.cellIndex = cellIndex;
	// }
}
