using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CropItemData", menuName = "ScriptableObjects/CropItemData", order = 1)]
public class CropItemData : BaseItemData
{
	[ReadOnly]
	public Item.ItemType itemType = Item.ItemType.Crop;
	public Item.StackType stackType = Item.StackType.Stackable;

	public override Item.ItemType GetItemType() { return itemType; }
	public override Item.StackType GetStackType() { return stackType; }

	public override void OnUse(Item item = null)
	{


	}
}