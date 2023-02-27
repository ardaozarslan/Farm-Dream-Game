using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
	public new string name;
	public int id;
	public Item.ItemType itemType;
	public string description;
	public Sprite sprite;
	public int maxStackSize;
	public bool isStackable;

	public virtual void OnUse()
	{
		Debug.Log("Using " + name);
	}
}
