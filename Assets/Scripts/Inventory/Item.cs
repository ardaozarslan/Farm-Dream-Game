using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	private ItemData itemData;
	public ItemData ItemData { get { return itemData; } }

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

	public Item(ItemData itemData, int stackSize = 1)
	{
		this.name = itemData.name;
		this.itemType = itemData.itemType;
		this.description = itemData.description;
		this.sprite = itemData.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = itemData.maxStackSize;
		this.stackType = itemData.stackType;
		this.itemData = itemData;

		// InitId();
		switch (this.itemData.stackType)
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

	public Item(Item item, int stackSize = 1)
	{
		this.name = item.name;
		this.itemType = item.itemType;
		this.description = item.description;
		this.sprite = item.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = item.maxStackSize;
		this.stackType = item.stackType;
		this.itemData = item.itemData;

		// InitId();
		switch (this.itemData.stackType)
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

	private void InitId()
	{
		switch (this.itemData.stackType)
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
