using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PickaxeToolItemData", menuName = "ScriptableObjects/Tools/PickaxeToolItemData", order = 1)]
public class PickaxeToolItemData : BaseItemData
{
	public Item.StackType stackType = Item.StackType.Durability;

	public override Item.StackType GetStackType() { return stackType; }

	public override void OnUse(Item item = null)
	{


	}

}