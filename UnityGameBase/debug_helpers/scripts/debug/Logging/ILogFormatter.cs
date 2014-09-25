using System;

public interface ILogFormatter
{
	string FormatMessage(ELogLevel pLevel, string pMessage);
}


