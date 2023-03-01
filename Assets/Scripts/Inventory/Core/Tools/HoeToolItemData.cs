using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "HoeToolItemData", menuName = "ScriptableObjects/Tools/HoeToolItemData", order = 1)]
public class HoeToolItemData : BaseItemData
{
	public Item.StackType stackType = Item.StackType.Durability;

	public override Item.StackType GetStackType() { return stackType; }

	public override void OnUse(Item item = null)
	{


	}

}