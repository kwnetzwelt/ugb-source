using UnityEngine;
using System;

/// <summary>
/// Catches normal unity logging calls and redirects them to any registered loggers
/// </summary>
public class CLogCatcher : MonoBehaviour
{
	void Awake()
	{
		Application.RegisterLogCallback( OnLogCallback );
	}

	void OnLogCallback (string condition, string stackTrace, LogType type)
	{
		switch(type)
		{
		case LogType.Log: Logger.Log(condition,stackTrace); break;
		case LogType.Error: Logger.LogError(condition,stackTrace); break;
		case LogType.Warning: Logger.LogWarning(condition,stackTrace); break;
		
		case LogType.Assert: Logger.LogWarning(condition,stackTrace); break;
		case LogType.Exception: Logger.LogError(condition,stackTrace); break;
		}
	}
}