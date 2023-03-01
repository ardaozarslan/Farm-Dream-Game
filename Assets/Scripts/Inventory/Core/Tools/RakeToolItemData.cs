using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RakeToolItemData", menuName = "ScriptableObjects/Tools/RakeToolItemData", order = 1)]
public class RakeToolItemData : BaseItemData
{
	public Item.StackType stackType = Item.StackType.Durability;

	public override Item.StackType GetStackType() { return stackType; }

	public override void OnUse(Item item = null)
	{


	}

}