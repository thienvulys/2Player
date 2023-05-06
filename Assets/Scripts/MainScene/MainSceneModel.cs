using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MainScene
{
	public class MainSceneModel : MonoBehaviour
	{
		#region MAIN SCREEN MODEL
		[SerializeField] private int limitHistory = 8;
		[SerializeField] private List<GameItem> gameItems;
		#endregion

		public int LimitHistory => limitHistory;
		public List<GameItem> GameItems => gameItems.FindAll((item) => item.isDisplayOnScreen).OrderBy((item) => item.priority).ToList();
		public List<GameItem> TempGame => gameItems.ToList();

		[System.Serializable]
		public class GameItem
		{
			public string name;
			public string sceneName;
			public string tutorial;
			public Sprite[] tutorialSprites;
			public bool isFavorite = false;
			public bool isVSBot;
			public bool isVSPlayer;
			public Sprite sprite;
			public bool isDisplayOnScreen = true;
			public bool watchAdsToPlay = false;
			public int priority = 0;
			public int matchTime = 30;
			public int WinScore = 0;
		}
	}
}
