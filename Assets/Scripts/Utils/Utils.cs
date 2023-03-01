using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public delegate void Function();
	public class UtilsStaticMB : Singleton<UtilsStaticMB> { }
	private static UtilsStaticMB utilsStaticMB = UtilsStaticMB.Instance;

	public static void InvokeNextFrame(Function function)
	{
		try
		{
			utilsStaticMB.StartCoroutine(_InvokeNextFrame(function));
		}
		catch
		{
			Debug.Log("Trying to invoke " + function.ToString() + " but it doesnt seem to exist");
		}
	}
	public static void WaitForSecondsAndInvoke(float seconds, Function function)
	{
		try
		{
			utilsStaticMB.StartCoroutine(_WaitForSecondsAndInvoke(seconds, function));
		}
		catch
		{
			Debug.Log("Trying to invoke " + function.ToString() + " but it doesnt seem to exist");
		}
	}

	private static IEnumerator _InvokeNextFrame(Function function)
	{
		yield return null;
		function();
	}

	private static IEnumerator _WaitForSecondsAndInvoke(float seconds, Function function)
	{
		yield return new WaitForSeconds(seconds);
		function();
	}

	public static bool AreCollidersOverlapping(Collider collider1, Collider collider2)
	{
		// Get the bounds of each collider
		Bounds bounds1 = collider1.bounds;
		Bounds bounds2 = collider2.bounds;

		// Calculate the center of the overlapping box
		Vector3 center = bounds1.center + (bounds2.center - bounds1.center) / 2f;

		// Calculate the size of the overlapping box
		Vector3 size = new Vector3(
			Mathf.Min(bounds1.max.x, bounds2.max.x) - Mathf.Max(bounds1.min.x, bounds2.min.x),
			Mathf.Min(bounds1.max.y, bounds2.max.y) - Mathf.Max(bounds1.min.y, bounds2.min.y),
			Mathf.Min(bounds1.max.z, bounds2.max.z) - Mathf.Max(bounds1.min.z, bounds2.min.z)
		);

		// Check for overlapping colliders
		Collider[] overlappingColliders = Physics.OverlapBox(center, size / 2f);

		// Return true if both colliders are in the overlappingColliders array
		return System.Array.IndexOf(overlappingColliders, collider1) != -1 &&
			   System.Array.IndexOf(overlappingColliders, collider2) != -1;
	}

	public static void CreateWorldTextPopup(string text, Transform parent, Vector3? localScale = null, Color? color = null)
	{
		CreateWorldTextPopup(parent, text, 40, color ?? Color.white, 1f, localScale ?? Vector3.one * 0.2f);
	}

	public static void CreateWorldTextPopup(Transform parent, string text, int fontSize, Color color, float popupTime, Vector3 localScale)
	{
		if (!Globals.Instance.showDebugPopupText) return;

		float riseStartAmount = 1.25f;
		float riseAmount = 5f;

		Vector3 startPosition = parent.position + Vector3.up * riseStartAmount;
		GameObject textParentObject = new GameObject("WorldTextPopupParent");
		textParentObject.transform.localScale = localScale;
		textParentObject.transform.position = startPosition;
		textParentObject.transform.SetParent(parent, true);
		textParentObject.transform.RotateAround(textParentObject.transform.position, Vector3.right, Camera.main.transform.rotation.eulerAngles.x);

		Vector3 finalPopupLocalToDummyPosition = textParentObject.transform.localPosition + Vector3.up * riseAmount;
		Vector3 finalPopupWorldPosition = textParentObject.transform.TransformPoint(finalPopupLocalToDummyPosition);
		Vector3 finapPopupLocalPosition = textParentObject.transform.InverseTransformPoint(finalPopupWorldPosition);

		CodeMonkey.Utils.UtilsClass.CreateWorldTextPopup(textParentObject.transform, text, Vector3.zero, fontSize, color, finapPopupLocalPosition, popupTime);


		WaitForSecondsAndInvoke(popupTime, () =>
		{
			InvokeNextFrame(() =>
			{
				Object.Destroy(textParentObject);
			});
		});
	}

}
