using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
	public List<BaseItemData> itemDataObjects;
	public ManagersManager managersManager;
    // Start is called before the first frame update
    void Start()
    {
		managersManager = ManagersManager.Instance;
        itemDataObjects = new List<BaseItemData>(Resources.LoadAll<BaseItemData>("ItemData"));
    }

	public BaseItemData GetItemData(string id) {
		if (id.Contains(":")) {
			id = id.Split(':')[0];
		}
		return itemDataObjects.Find(itemData => itemData.id == id);
	}

	// public ItemData GetItemData(string name) {
	// 	return itemDataObjects.Find(itemData => itemData.name == name);
	// }

    // Update is called once per frame
    void Update()
    {
        
    }
}
