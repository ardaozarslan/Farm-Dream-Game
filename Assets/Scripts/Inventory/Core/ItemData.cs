using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
	public new string name;
	public string id;
	public Item.ItemType itemType;
	public string description;
	public Sprite sprite;
	public int maxStackSize;
	public Item.StackType stackType;

	public virtual GameObject GetPlantPrefab() { return null; }
	public virtual float GetGrowTime() { return 0f; }

	public virtual void OnUse(Item item = null)
	{
		Debug.Log("Using " + name + " without OnUse() override.");
	}
}
