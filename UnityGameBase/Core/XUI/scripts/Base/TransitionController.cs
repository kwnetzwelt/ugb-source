using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.XUI
{
/// <summary>
/// this component is the baseclass for all transitions
/// </summary>
	public abstract class TransitionController : MonoBehaviour
	{
		protected WidgetManager widgetCollection;
		protected GameObject root;
		public void Awake()
		{
			this.widgetCollection = this.GetComponent<WidgetManager>();
		}
		public virtual void Init(GameObject rootObj)
		{
			this.root = rootObj;
		}
		
		public abstract void Show(System.Action onDone);
		public abstract void Hide(System.Action onDone);

	}
}