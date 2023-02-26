using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	public PlayerInputActions playerInputActions;
	public event Action<InputActionMap> actionMapChange;
	public ManagersManager managersManager;

	public InputActionMap currentActionMap;

	public delegate void DeveloperConsoleStateChanged(bool state);
	public static event DeveloperConsoleStateChanged developerConsoleStateChanged;

	private void OnEnable()
	{
		developerConsoleStateChanged += DeveloperConsoleStateChangeReceive;
	}

	private void OnDisable()
	{
		developerConsoleStateChanged -= DeveloperConsoleStateChangeReceive;
	}

	public static void DeveloperConsoleStateChangeCall(bool state)
	{
		developerConsoleStateChanged?.Invoke(state);
	}

	public void DeveloperConsoleStateChangeReceive(bool state)
	{
		if (state)
		{
			TemporarilyDisableCurrentActionMap();
		}
		else
		{
			ReenableCurrentActionMap();
		}

	}


	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
	}

	private void Start()
	{
		managersManager = ManagersManager.Instance;
		SwitchActionMap(playerInputActions.Player);
	}

	public void SwitchActionMap(InputActionMap actionMap)
	{
		if (actionMap.enabled)
		{
			return;
		}
		playerInputActions.Disable();
		actionMapChange?.Invoke(actionMap);
		actionMap.Enable();
		currentActionMap = actionMap;
	}

	public void TemporarilyDisableCurrentActionMap()
	{
		currentActionMap.Disable();
	}

	public void ReenableCurrentActionMap()
	{
		currentActionMap.Enable();
	}
}
