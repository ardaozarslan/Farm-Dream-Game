using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ToolItemData", menuName = "ScriptableObjects/ToolItemData", order = 1)]
public class ToolItemData : BaseItemData
{
	public Item.ItemType itemType = Item.ItemType.Tool;
	// Change this in specific tool SOs
	// public new Item.StackType stackType = Item.StackType.Durability;

	public override Item.ItemType GetItemType() { return itemType; }

}