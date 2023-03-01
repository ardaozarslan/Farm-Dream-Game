using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : Singleton<Globals>
{
	[Tooltip("Shows debug popup text when an item is picked up or dropped, etc.")]
	public bool showDebugPopupText = true;
	[Tooltip("Enable the developer console (~)")]
	public bool enableDeveloperConsole = true;
}
