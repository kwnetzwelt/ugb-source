using System;

[Flags]
public enum EDebugEntryState
{
	normal = 0x00,
	hidden = 0x01,
	urgent = 0x02
}