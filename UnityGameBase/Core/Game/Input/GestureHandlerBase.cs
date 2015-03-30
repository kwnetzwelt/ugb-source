using UnityEngine;
using System.Collections;
namespace UnityGameBase.Core.Input
{
	public abstract class GestureHandlerBase : MonoBehaviour
	{
		protected GameInput InputSystem
		{
			get;
			private set;
		}

		public void Initialize(GameInput inputSystem)
		{
			this.InputSystem = inputSystem;
			
			this.InputSystem.TouchStart += HandleTouchStart;
			this.InputSystem.TouchEnd += HandleTouchEnd;
		}
		
		void OnDestroy()
		{
			if(this.InputSystem != null)
			{
				this.InputSystem.TouchStart -= HandleTouchStart;
				this.InputSystem.TouchEnd -= HandleTouchEnd;
			}
		}
		
		protected abstract void HandleTouchEnd (TouchInformation touchInfo);
		protected abstract void HandleTouchStart (TouchInformation touchInfo);
	}

}