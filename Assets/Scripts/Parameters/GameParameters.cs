using UnityEngine;

using MainScene;

namespace Parameters
{
	public enum GameMode
	{
		Pvp,
		BotEasy,
		BotNormal,
		BotHard
	}

	public class GameParameters : MonoBehaviour
	{
		public GameMode Mode { get; set; }
		public MainSceneModel.GameItem GameItem { get; set; }

		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
