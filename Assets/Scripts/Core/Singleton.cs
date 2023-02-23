using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				T[] objs = FindObjectsOfType<T>(true);
				if (objs.Length > 0)
				{
					// Debug.Log("found at least one instance of " + typeof(T).Name + " in the scene. Using the first one found.");
					T instance = objs[0];
					_instance = instance;
					if (instance.gameObject.transform.parent == null)
					{
						DontDestroyOnLoad(instance.gameObject);
					}
				}
				else
				{
					GameObject go = new GameObject();
					go.name = typeof(T).Name;
					_instance = go.AddComponent<T>();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	}
}
