using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
	public FarmField farmField;
	public Item seedItem;
	public List<GameObject> plantScaleModels = new List<GameObject>();

	public void Initialize(FarmField _farmField, Item _seedItem)
	{
		farmField = _farmField;
		farmField.plant = this;
		seedItem = _seedItem;

		foreach (GameObject child in plantScaleModels)
		{
			child.transform.localScale = Vector3.zero;
			StartCoroutine(Grow(child.transform));
		}

	}

	private IEnumerator Grow(Transform childTransform)
	{
		float growTime = seedItem.ItemData.GetGrowTime();
		float timer = 0;
		while (timer < growTime)
		{
			timer += Time.deltaTime;
			childTransform.localScale = Vector3.one * (timer / growTime);
			yield return null;
		}
		childTransform.localScale = Vector3.one;
	}
}
