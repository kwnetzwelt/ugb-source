using System;
using System.Collections.Generic;


public class Logger : IDisposable
{
	const string defaultSep = " ";
	static List<Logger> mLoggers = new List<Logger>(); 

	private Logger ()
	{
	}

	#region IDisposable implementation

	public void Dispose ()
	{
		if(mLoggers.Contains(this))
			mLoggers.Remove(this);
	}

	#endregion

	public ILogWriter mWriter;

	static void RegisterLogger(Logger pLogger)
	{
		mLoggers.Add(pLogger);
	}

	/// <summary>
	/// Factory to create a logger entry. The Entry is automatically registered and deregistered ( upon Dispose())
	/// </summary>
	/// <returns>The logger.</returns>
	/// <param name="pWriter">P writer.</param>
	/// <param name="pFormatter">P formatter.</param>
	public static Logger CreateLogger(ILogWriter pWriter, ILogFormatter pFormatter)
	{
		var logger = new Logger();
		logger.mWriter = pWriter;
		logger.mWriter.SetFormatter(pFormatter);

		RegisterLogger(logger);
		return logger;
	}

	/// <summary>
	/// Factory to create a logger entry. The Entry is automatically registered and deregistered ( upon Dispose())
	/// </summary>
	/// <returns>The logger.</returns>
	/// <param name="pWriter">P writer.</param>
	/// <param name="pFormatter">P formatter.</param>
	public static Logger CreateLogger(ILogWriter pWriter, System.Type pFormatter)
	{
		var formatter = (ILogFormatter)Activator.CreateInstance(pFormatter);
		return CreateLogger(pWriter, formatter);
	}

	/// <summary>
	/// Factory to create a logger entry. The Entry is automatically registered and deregistered ( upon Dispose())
	/// </summary>
	/// <returns>The logger.</returns>
	/// <param name="pWriter">P writer.</param>
	/// <param name="pFormatter">P formatter.</param>
	public static Logger CreateLogger(System.Type pWriter, System.Type pFormatter)
	{
		var writer = (ILogWriter) Activator.CreateInstance(pWriter);
		var formatter = (ILogFormatter) Activator.CreateInstance(pFormatter);
		return CreateLogger(writer, formatter);
	}

	static void Log( ELogLevel pLogLevel, params object[] pText)
	{
		string[] sText = new string[pText.Length];
		for(int i = 0;i< pText.Length;i++)
		{
			sText[i] = pText.ToString();
		}
		Log(pLogLevel, sText);
	}

	static void Log( ELogLevel pLogLevel, params string[] pText)
	{
		foreach(var l in mLoggers)
		{
			if(l.mWriter.enabled && l.mWriter.minLogLevel >= pLogLevel)
				l.mWriter.Write(pLogLevel, String.Join(defaultSep,pText));
		}
	}

	public static void Log(params string[] pText)
	{
		Log (ELogLevel.debug,pText);
	}
	
	public static void LogWarning(params string[] pText)
	{
		Log (ELogLevel.warning,pText);
	}

	
	public static void LogError(params string[] pText)
	{
		Log (ELogLevel.error,pText);
	}

	public static void LogException(System.Exception pException, string pText)
	{
		string oString = pText + defaultSep + pException.Message + pException.StackTrace;
		Log(ELogLevel.exception,oString);
	}

}