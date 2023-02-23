using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
	public Image itemImage;
	public TextMeshProUGUI itemStackSizeText;

	private int stackSize;
	public int StackSize
	{
		get { return stackSize; }
		set { this.stackSize = value; }
	}
	private int index;
	public int Index
	{
		get { return index; }
		set { this.index = value; }
	}
	public Item item;


	public void UpdateSelf()
	{
		if (item != null)
		{
			itemImage.sprite = item.sprite;
			itemImage.color = Color.white;
			itemStackSizeText.text = stackSize.ToString();
		}
		else
		{
			itemImage.sprite = null;
			itemImage.color = Color.clear;
			itemStackSizeText.text = "";
		}
	}

}
