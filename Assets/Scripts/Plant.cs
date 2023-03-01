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
			child.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
			child.transform.localScale = Vector3.zero;
			StartCoroutine(GrowIndividualModel(child.transform));
		}
		StartCoroutine(Grow());
	}

	private IEnumerator GrowIndividualModel(Transform childTransform)
	{
		float growTime = seedItem.ItemData.GetGrowTime() + Random.Range(-seedItem.ItemData.GetGrowTimeVariance(), 0);
		float growScale = seedItem.ItemData.GetGrowScale() + Random.Range(-seedItem.ItemData.GetGrowScaleVariance(), seedItem.ItemData.GetGrowScaleVariance());
		Vector3 growScaleVector = Vector3.one * growScale;
		float timer = 0;
		while (timer < growTime)
		{
			timer += Time.deltaTime;
			childTransform.localScale = growScaleVector * (timer / growTime);
			yield return null;
		}
		childTransform.localScale = growScaleVector;
	}

	private IEnumerator Grow()
	{
		farmField.farmState = FarmField.FarmState.Growing;
		float growTime = seedItem.ItemData.GetGrowTime();
		yield return new WaitForSeconds(growTime);
		farmField.farmState = FarmField.FarmState.ReadyToHarvest;
		Utils.CreateWorldTextPopup("Ready to\nharvest!", transform, Vector3.one * 0.2f);
	}
}
