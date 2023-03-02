using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
	public string name;
	public string id;
	public ItemType itemType;
	public string description;
	public Sprite sprite;
	public int stackSize;
	public int maxStackSize;
	public StackType stackType;
	private BaseItemData itemData;
	public BaseItemData ItemData { get { return itemData; } }

	public enum ItemType
	{
		Tool = 100,
		Crop = 200,
		Seed = 300,
		Consumable = 400,
		Other = 500,
		None = 1000,
	}

	public enum StackType
	{
		Stackable = 100,
		Durability = 200,
		Other = 500,
		None = 1000,
	}

	public string GetId()
	{
		return this.id.Split(":")[0];
	}

	public string GetFullId()
	{
		return this.id;
	}

	public Item(BaseItemData itemData, int stackSize = 1)
	{
		this.name = itemData.name;
		this.itemType = itemData.GetItemType();
		this.description = itemData.description;
		this.sprite = itemData.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = itemData.maxStackSize;
		this.stackType = itemData.GetStackType();
		this.itemData = itemData;

		InitId();
	}

	public Item(Item item, int stackSize = 1)
	{
		BaseItemData itemData = item.itemData;
		this.name = itemData.name;
		this.itemType = itemData.GetItemType();
		this.description = itemData.description;
		this.sprite = itemData.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = itemData.maxStackSize;
		this.stackType = itemData.GetStackType();
		this.itemData = itemData;

		InitId();
	}

	private void InitId()
	{
		Debug.Log("item stack type: " + this.itemData.GetStackType());
		switch (this.itemData.GetStackType())
		{
			case StackType.Stackable:
				this.id = this.itemData.id.ToString();
				break;
			case StackType.Durability:
				this.stackSize = 1;
				System.Guid guid = System.Guid.NewGuid();
				string guidString = guid.ToString();
				this.id = this.itemData.id + ":" + guidString;
				break;
			default:
				break;
		}
	}
}
