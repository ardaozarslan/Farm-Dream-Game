using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils>
{
	public delegate void Function();

	public void InvokeNextFrame(Function function)
	{
		try
		{
			StartCoroutine(_InvokeNextFrame(function));
		}
		catch
		{
			Debug.Log("Trying to invoke " + function.ToString() + " but it doesnt seem to exist");
		}
	}

	private IEnumerator _InvokeNextFrame(Function function)
	{
		yield return null;
		function();
	}
}
