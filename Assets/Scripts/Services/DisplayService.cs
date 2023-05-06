using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Services
{
	public class DisplayService
	{
#if UNITY_EDITOR

		public bool IsFake { get; set; }
#endif
		public DisplayService()
		{
			Application.targetFrameRate = 60;
		}
		public bool WideScreen()
		{
			if ((float)Screen.width / (float)Screen.height > 0.58f)
			{
				return true;
			}
			return false;
		}
		public Rect SafeArea()
		{
#if UNITY_EDITOR
			if (IsFake)
			{
				return new Rect(0f, 90f, Screen.width, Screen.height - 90f);
			}
			else
#endif
			{
				return Screen.safeArea;
			}
		}
		public Rect[] Cutouts()
		{
			return Screen.cutouts;
		}
	}
}
