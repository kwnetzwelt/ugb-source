using UnityEngine;
using System.Collections;
using System;
using System.Text;

[ExecuteInEditMode()]
public class XLog : MonoBehaviour
{
	[Flags]
	public enum LogMask
	{
		Log = 1,
		Warning = 2,
		Error = 4,
		Exception = 8,
		All = Log | Warning | Error | Exception
	}
	
	
	public static LogMask logMask = LogMask.All;
	
	/// <summary>
	/// Logs a message
	/// </summary>
	/// <param name='pMessage'>
	/// P message.
	/// </param>
	public static void Log(params System.Object[] pMessage)
	{
		if((logMask & LogMask.Log) != 0)
			UnityEngine.Debug.Log(Merge(pMessage));
	}
	
	/// <summary>
	/// Logs a warning with the given message
	/// </summary>
	/// <param name='pMessage'>
	/// P message.
	/// </param>
	public static void LogWarning(params System.Object[] pMessage)
	{
		if((logMask & LogMask.Warning) != 0)
			UnityEngine.Debug.LogWarning(Merge(pMessage));
	}
	
	/// <summary>
	/// Logs an error with the given message
	/// </summary>
	/// <param name='pMessage'>
	/// P message.
	/// </param>
	public static void LogError(params System.Object[] pMessage)
	{
		if((logMask & LogMask.Error) != 0)
			UnityEngine.Debug.LogError(Merge(pMessage));
	}	
	/// <summary>
	/// Logs the given exception.
	/// </summary>
	/// <param name='pException'>
	/// P exception.
	/// </param>
	public static void LogException(System.Exception pException)
	{
		if((logMask & LogMask.Exception) != 0)
			UnityEngine.Debug.LogException( pException );
	}
	
	static string Merge(System.Object[] pMessage)
	{
		if(pMessage.Length == 1)
			return pMessage[0].ToString();
		
		StringBuilder sb = new StringBuilder();
		for(int i = 0;i< pMessage.Length;i++)
		{
			sb.Append(pMessage[i].ToString());
			if(i < pMessage.Length-1)
				sb.Append(", ");
		}
		
		return sb.ToString();
	}
	
	public LogMask mMask;
	
	void OnEnable()
	{
		XLog.logMask = mMask;
		XLog.LogError("bla", 2);
	}
}

