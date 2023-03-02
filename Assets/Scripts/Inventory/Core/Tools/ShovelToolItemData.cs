using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ShovelToolItemData", menuName = "ScriptableObjects/Tools/ShovelToolItemData", order = 1)]
public class ShovelToolItemData : ToolItemData
{
	public Item.StackType stackType = Item.StackType.Durability;

	public override Item.StackType GetStackType() { return stackType; }

	public override void OnUse(Item item = null)
	{


	}

}