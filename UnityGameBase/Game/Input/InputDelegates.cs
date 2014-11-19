using UnityEngine;
using System.Collections;

public class InputDelegates
{
	public delegate void OnTouchEventDelegate(TouchInformation pTouchInfo);
	public delegate void OnKeyMappingDelegate(string pKeyMappingName);
}

