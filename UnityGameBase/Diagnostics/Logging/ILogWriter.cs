using System;
namespace UGB.Diagnostics.Logging
{
	public interface ILogWriter
	{
		bool enabled { get; set;}
		ELogLevel minLogLevel { get; set;}
		void Write( ELogLevel pLogLevel, string pMessage );
		void SetFormatter( ILogFormatter pFormatter);
	}
}