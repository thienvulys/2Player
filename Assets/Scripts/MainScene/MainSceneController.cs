using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Extensions;
using DG.Tweening;

using Parameters;
using Services;

namespace MainScene
{
	public class MainSceneController : MonoBehaviour
	{
		[Header("PANEL PREFERENCE")]
		[SerializeField] private MainSceneView view;
		[SerializeField] private MainSceneModel model;
		[SerializeField] private ScrollViewEx mainScrollView;
		[SerializeField] private Toggle favoriteToggle;
		[SerializeField] private Toggle historyToggle;
		[SerializeField] private Toggle homeToggle;

		[SerializeField] private float tabHeight = 60f;
		[SerializeField] private float tabChangeTime = 0.2f;

		// screen shot
		[SerializeField] private Camera cameraScreenshot;
		private bool takeScreenshotOnNextFrame;

		private const string mainNoContent = "No games";
		private const string historyNoContent = "No played games";
		private const string favoriteNoContent = "No favorite games";

		private bool isReload;
		private DisplayService displayService;
		private PlayerService playerService;
		private InputService inputService;
		private AudioService audioService;

		private List<int> favorites = new List<int>();
		private List<int> histories = new List<int>();
		private GameServices gameServices;


		private void Awake()
		{
			view.ThrowIfNull();
			model.ThrowIfNull();
			mainScrollView.ThrowIfNull();
			favoriteToggle.ThrowIfNull();
			historyToggle.ThrowIfNull();
			homeToggle.ThrowIfNull();


			if (GameObject.FindGameObjectWithTag(Constans.ServicesTag) != null)
			{
				gameServices = GameObject.FindGameObjectWithTag(Constans.ServicesTag).GetComponent<GameServices>();
				displayService = gameServices.GetService<DisplayService>();
				playerService = gameServices.GetService<PlayerService>();
				inputService = gameServices.GetService<InputService>();
				audioService = gameServices.GetService<AudioService>();
			}
			else
			{
				SceneManager.LoadScene(Constans.EntryScreen);
				return;
			}

			//change background theme

			//Load Histories
			histories = playerService.GetHistory();
			if (histories == null)
			{
				histories = new List<int>();
			}
			//Load Favorites
			favorites = playerService.GetFavorite();
			if (favorites == null)
			{
				favorites = new List<int>();
			}
			//Set gameitem favorite
			for (int i = 0; i < model.TempGame.Count; i++)
			{
				if (favorites.Contains(i))
				{
					model.TempGame[i].isFavorite = true;
				}
			}
			view.AvoidCutout(displayService.SafeArea(), displayService.Cutouts());

			int numGameItems = model.TempGame.Count;
		}
		private bool CheckListOutOfRangeGameItem(List<int> listGameWatchToPlay, int lengthGameItems)
		{
			if (listGameWatchToPlay.Count == 0 || listGameWatchToPlay == null)
				return false;
			foreach(var i in  listGameWatchToPlay)
			{
				if (i >= lengthGameItems)
				{
					return false;
				}
			}
			return true;
		}

		private void Start()
		{
			audioService?.PlayMusic();

			view.SetScore(gameServices.GetService<PlayerService>());

			mainScrollView.OnItemClick = OpenGameMode;
			mainScrollView.OnFavoriteChange = OnChangeFavorite;
			mainScrollView.Init(model.GameItems, mainNoContent, displayService.SafeArea(), displayService.WideScreen());
			view.PutMainLayer();
			view.PutBottomLayer();

			//if (!playerService.IsTutorialPlayed()) view.OpenPopup(MainSceneView.UIPopup.Tutorial);
			if (GetLastGamePlayed() > 0)
			{
				mainScrollView.ScrollToGame(GetLastGamePlayed());
				audioService?.PlayButton2();
			}

			ToggleChange(homeToggle);
		}

		public List<MainSceneModel.GameItem> ToGameItemList(List<int> list)
		{
			List<MainSceneModel.GameItem> temp = new List<MainSceneModel.GameItem>();
			for (int i = 0; i < list.Count; i++)
			{
				// list: index in temp Game
				temp.Add(model.TempGame[list[i]]);
			}
			return temp;
		}
		#region SNAP EVENT
		public void FavoriteToggle()
		{
			audioService?.PlayButton1();
		}

		public void ToggleChange(Toggle toggle)
		{
			if (!toggle.isOn) return;
			audioService?.PlayButton1();
			homeToggle.transform.DOLocalMoveY(0f, tabChangeTime);
			historyToggle.transform.DOLocalMoveY(0f, tabChangeTime);
			favoriteToggle.transform.DOLocalMoveY(0f, tabChangeTime);
			view.PutMainLayer();

			if (homeToggle.Equals(toggle))
			{
				homeToggle.transform.DOLocalMoveY(tabHeight, tabChangeTime);
				mainScrollView.Init(model.GameItems, mainNoContent, displayService.SafeArea(), displayService.WideScreen());
			}
			else if (historyToggle.Equals(toggle))
			{
				historyToggle.transform.DOLocalMoveY(tabHeight, tabChangeTime);
				mainScrollView.Init(ToGameItemList(histories), historyNoContent, displayService.SafeArea(), displayService.WideScreen(), true);
			}
			else if (favoriteToggle.Equals(toggle))
			{
				favoriteToggle.transform.DOLocalMoveY(tabHeight, tabChangeTime);
				mainScrollView.Init(ToGameItemList(favorites), favoriteNoContent, displayService.SafeArea(), displayService.WideScreen());
			}

		}

		public void HistoryToggle()
		{
			audioService?.PlayButton1();
		}
		#endregion

		#region HISTORY , FAVORITE
		public void OnChangeFavorite(MainSceneModel.GameItem gameItem, bool isOn)
		{
			audioService?.PlayButton1();
			int index = model.TempGame.IndexOf(gameItem);
			if (isOn && !favorites.Contains(index))
			{
				favorites.Add(index);
			}
			else
			{
				favorites.Remove(index);
			}
			playerService.SaveFavorite(favorites);
			playerService.Save();
			model.TempGame[index].isFavorite = isOn;
		}

		private void AddHistory(MainSceneModel.GameItem gameItem)
		{
			int index = model.TempGame.IndexOf(gameItem);
			if (index >= 0)
			{
				if (histories.Contains(index))
				{
					histories.Remove(index);
				}
				histories.Add(index);

				if (histories.Count > model.LimitHistory)
				{
					histories.RemoveRange(0, histories.Count - model.LimitHistory);
				}
				playerService.SaveHistory(histories);
			}
		}
		#endregion
		#region MODE
		public void SaveMode(GameMode mode)
		{
			audioService?.PlayButton1();
			playerService.SaveLastGameMode(mode);
		}
		#endregion
		#region LASTGAMEPLAYED
		public void SaveLastGamePlayed(int index)
		{
			playerService.SetLastGamePlayed(index);
		}
		public int GetLastGamePlayed()
		{
			return playerService.GetLastGamePlayed();
		}
		#endregion
		#region SETTING
		public void SaveMusicVolume(float volume)
		{
			audioService?.PlayButton1();
			playerService.SetMusicVolume(volume);
		}
		public void SaveSoundVolume(float volume)
		{
			audioService?.PlayButton1();
			playerService.SetSoundVolume(volume);
		}

		public void SaveVibration(bool isOn)
		{
			audioService?.PlayButton1();
			playerService.SetVibrate(isOn);
			if (isOn) audioService.Vibrate();
		}
		public float LoadMusicVolume()
		{
			return playerService.GetMusicVolume();
		}

		public float LoadSoundVolume()
		{
			return playerService.GetSoundVolume();
		}
		public bool LoadVibration()
		{
			return playerService.GetVibrate();
		}
		#endregion
		#region RATE US, THANK YOU

		#endregion
		#region POPUP,GAME
		public void OpenScene(MainSceneModel.GameItem gameItem, GameMode mode)
		{
			if (GameObject.FindGameObjectWithTag(Constans.ParamsTag) == null)
			{
				audioService?.PlayButton1();
				audioService?.FadeMusic(3f);
				//Off music in 3 seconds
				AddHistory(gameItem);
				//
				List<MainSceneModel.GameItem> list = model.GameItems;
				SaveLastGamePlayed(list.IndexOf(gameItem));
				GameObject paramObject = new GameObject(nameof(GameParameters));
				paramObject.tag = Constans.ParamsTag;
				GameParameters gameParameters = paramObject.AddComponent<GameParameters>();
				gameParameters.Mode = mode;
				gameParameters.GameItem = gameItem;
				playerService.DecreaseTimesPlayRewardedGame(gameItem.priority);
				SceneManager.LoadScene(gameItem.sceneName);
			}
		}

		public void OpenSetting()
		{
			audioService?.PlayButton1();
			view.OpenPopup(MainSceneView.UIPopup.Setting);
			view.SettingsPopup.LoadSetting(LoadMusicVolume(), LoadSoundVolume(), LoadVibration());
		}

		public void OpenGameMode(MainSceneModel.GameItem gameItem, bool isBack)
		{
			if (isBack) audioService?.PlayButton2();
			else audioService?.PlayButton1();

			{
				if (gameItem.watchAdsToPlay == true && playerService.GetTimesPlayRewardedGame(gameItem.priority) == 0)
				{
					//view.PremiumCarePopup.SetGameItem(gameItem);
					view.OpenPopup(MainSceneView.UIPopup.PremiumCare);
				}
				else
				{
					view.OpenPopup(MainSceneView.UIPopup.Info);
					view.GameInfoPopup.SetGameItem(gameItem);
				}
			}

		}

		public void OpenDifficulty(MainSceneModel.GameItem gameItem)
		{
			audioService?.PlayButton1();
			//view.ModePopup.SetMode(playerService.GetLastGameMode());
			view.OpenPopup(MainSceneView.UIPopup.Difficulty);
			view.ModePopup.SetGameItem(gameItem);
		}

		public void OpenMain()
		{
			audioService?.PlayButton2();
			view.OpenPopup(MainSceneView.UIPopup.Main);
		}

		public void OpenGameTutorial(MainSceneModel.GameItem gameItem)
		{
			audioService?.PlayButton1();
			view.GameTutorialPopup.SetGameItem(gameItem);
			view.OpenPopup(MainSceneView.UIPopup.GameTutorial);
		}

		public void OpenRemoveAds()
		{
			audioService?.PlayButton1();
			view.OpenPopup(MainSceneView.UIPopup.RemoveAds);
		}

		public void OpenCredit()
		{
			audioService?.PlayButton1();
			view.OpenPopup(MainSceneView.UIPopup.Credit);
		}

		public void OpenRateUs()
		{
			audioService?.PlayButton1();
			view.OpenPopup(MainSceneView.UIPopup.RateUs);
		}

		public void OpenThankYou(int star)
		{
			audioService?.PlayButton1();
			if (star > 3)
			{
				view.OpenPopup(MainSceneView.UIPopup.ThankYou);
				view.ThankYouPopup.SetImage(true);
			}
			else
			{
				view.OpenPopup(MainSceneView.UIPopup.ThankYou);
				view.ThankYouPopup.SetImage(false);
			}

		}
		public void OpenRestore()
		{
			audioService?.PlayButton1();
		}
		public void OpenPurchase()
		{
			audioService?.PlayButton1();
		}

		private void OnPurchaseComplete(bool success)
		{
			if (success == true)
			{
				view.HideRemoveAdsButton();
				view.OpenPopup(MainSceneView.UIPopup.Main);
			}
			else
			{
				Logger.Debug("Purchase failed.");
			}
		}
		public void OpenTerm()
		{
			audioService?.PlayButton1();
			Application.OpenURL("https://abigames.com.vn/ct-terms-of-service/");
		}

		public void OpenPrivacy()
		{
			audioService?.PlayButton1();
			Application.OpenURL("https://abigames.com.vn/policy/");
		}

		#endregion
		#region PREMIUM CARE
		public void WatchAds(MainSceneModel.GameItem gameItem)
		{
			playerService.SetCurrentGame(gameItem.priority);
		}
		private void OnRewardedComplete()
		{
			playerService.AddTimesPlayRewardedGame(playerService.CurrentGame);
			OpenGameMode(model.GameItems.Single(x => x.priority == playerService.CurrentGame), false);
		}
		private void OnRewardedFailed()
		{
			view.OpenPopup(MainSceneView.UIPopup.Main);
		}
		private void OnRewardedAdsLoad(bool active)
		{
			view.HideWatchAds(active);
		}
		#endregion

		private void OnDestroy()
		{
			DOTween.KillAll();
		}
	}
}

