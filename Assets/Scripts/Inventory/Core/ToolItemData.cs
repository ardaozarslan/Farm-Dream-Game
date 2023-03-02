using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ToolItemData", menuName = "ScriptableObjects/ToolItemData", order = 1)]
public class ToolItemData : BaseItemData
{
	[ReadOnly]
	public Item.ItemType itemType = Item.ItemType.Tool;

	public override Item.ItemType GetItemType() { return itemType; }

}