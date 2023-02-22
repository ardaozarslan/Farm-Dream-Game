using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public static PlayerInputActions playerInputActions;
	public static event Action<InputActionMap> actionMapChange;

	private void Awake() {
		playerInputActions = new PlayerInputActions();
	}

	private void Start()
	{
		SwitchActionMap(playerInputActions.Player);
	}

	public static void SwitchActionMap(InputActionMap actionMap)
	{
		if (actionMap.enabled)
		{
			return;
		}
		playerInputActions.Disable();
		actionMapChange?.Invoke(actionMap);
		actionMap.Enable();
	}
}
