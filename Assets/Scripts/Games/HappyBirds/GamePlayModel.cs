using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Parameters;

namespace HappyBirds
{
	public class GamePlayModel : MonoBehaviour
	{
		#region GAMEPLAY CONFIG
		[Space(8.0f)]
		[Header("GAMEPLAY CONFIG")]
		public int TimeMatch = 30;
		public int TimePerSet;
		public int TimeDieDelay;
		public int WinScore;
		public int MinObtacles;
		public int MaxObtacles;
		public float ChangeTime = 1.5f;
		public float StonePosY = 1.6f;
		#endregion

		#region BOT CONFIG
		[Space(8.0f)]
		[Header("BOT CONFIG")]
		public bool IsBotActive;
		[Range(0.0f, 100.0f)]
		public float BotWinRate;
		public float DelayTouch;
		#endregion

		#region BIRD CONFIG
		[Space(8.0f)]
		[Header("BIRD CONFIG")]
		public float Gravity;
		public float FlyForce;
		public float flyDistance;
		#endregion
	}
}

