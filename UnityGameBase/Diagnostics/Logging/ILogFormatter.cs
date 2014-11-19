using System;
namespace UGB.Diagnostics.Logging
{
	public interface ILogFormatter
	{
		string FormatMessage(ELogLevel pLevel, string pMessage);
	}
}