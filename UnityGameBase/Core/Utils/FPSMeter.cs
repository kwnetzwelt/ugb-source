using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.Utils
{
	public class FPSMeter : MonoBehaviour {

		float[] mValues = new float[25];
		int mCIdx;
		float mAvg = 0;

		// Time measuring
		float lastTime;

		// Use this for initialization
		void Start() {
			lastTime = Time.realtimeSinceStartup;
		}

		// Update is called once per frame
		void Update()
		{
			// Calculate gone time with real time since startup...works with any time scale value
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;

			mValues[mCIdx] = timeSpan;

			if(mCIdx % mValues.Length == 0)
			{
				mAvg = 0;
				foreach (float v in mValues)
					mAvg += v;

				mAvg = mAvg / mValues.Length;
				mAvg = (float)System.Math.Round(1 / mAvg, 2);
			}

			mCIdx = (mCIdx + 1) % mValues.Length;
		}


		void OnGUI()
		{
			if(!Debug.isDebugBuild)
				return;
			GUI.Label(new Rect(5, 5, 200, 50), "Fps:" + mAvg);
		}
		
	}
}