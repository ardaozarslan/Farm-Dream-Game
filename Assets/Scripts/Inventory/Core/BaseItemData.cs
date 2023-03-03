using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class BaseItemData : ScriptableObject
{
	public new string name;
	public string id;
	// [HideInInspector]
	private Item.ItemType _itemType = Item.ItemType.None;
	[Multiline]
	public string description;
	public Sprite sprite;
	public int maxStackSize;
	private Item.StackType _stackType = Item.StackType.None;

	public virtual Item.ItemType GetItemType() { return _itemType; }
	public virtual Item.StackType GetStackType() { return _stackType; }

	public virtual void OnUse(Item item = null)
	{
		Debug.Log("Using " + name + " without OnUse() override.");
	}
}
