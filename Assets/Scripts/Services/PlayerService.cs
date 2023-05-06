using System;
using System.Collections.Generic;
using UnityEngine;
using Parameters;
using MainScene;

namespace Services
{
	public class PlayerService
	{
		// Key
		private const string MusicVolumeKey = "mvl";
		private const string SoundVolumeKey = "svl";
		private const string VibrateKey = "vbr";
		private const string LastGamePlayedKey = "lgp";

		private const string HistoryKey = "htr";
		private const string FavoriteKey = "fvr";
		private const string TutorialKey = "ttr";
		private const string GameModeKey = "gm";

		private const string Break = "~";
		// Game Rewarded
		private Dictionary<int, int> listGameRewardedToPlay = new Dictionary<int, int>();
		public int CurrentGame { get; private set; }

		// Tournament
		public bool IsTournament { get; private set; } = false;
		private int[] scoreTournament = new int[7];
		private int gameOfTournament = 0;
		private int[] gameTournament = new int[7];
		private GameMode gameModeTournament;
		private int lastWinTournament = -1;

		public int BlueScore { get; private set; }
		public int RedScore { get; private set; }
		public int LastWin { get; private set; } = -1;
		public Action<float> OnMusicVolumeChange;
		public Action<float> OnSoundVolumeChange;

		public Action<bool> OnVibrateChange;
		public void AddBlueScore()
		{
			BlueScore++;
		}
		public void AddRedScore()
		{
			RedScore++;
		}
		public void SetLastWin(int win)
		{
			LastWin = win;
		}
		// Ads rewarded
		public void AddTimesPlayRewardedGame(int game)
		{
			if (listGameRewardedToPlay.ContainsKey(game) == false)
			{
				listGameRewardedToPlay.Add(game, 3);
			}
			else
			{
				listGameRewardedToPlay[game] = 3;
			}
		}
		public int GetTimesPlayRewardedGame(int game)
		{
			if(listGameRewardedToPlay.ContainsKey(game) == false)
			{
				listGameRewardedToPlay.Add(game, 0);
				return 0;
			}
			else
			{
				return listGameRewardedToPlay[game];
			}
		}
		public void SetCurrentGame(int game)
		{
			CurrentGame = game;
		}
		public void DecreaseTimesPlayRewardedGame(int game)
		{
			if(listGameRewardedToPlay.ContainsKey(game) == true)
			{
				listGameRewardedToPlay[game] -= 1;
			}
		}
		// tournament
		// Set score each game of tournament
		public void SetScoreTournament(MainScene.Side side)
		{
			if(IsTournament)
			{
				scoreTournament[gameOfTournament] = (int)side;
				gameOfTournament++;
			}
		}
		// Get score tournament
		public int[] GetScoreTournament()
		{
			return scoreTournament;
		}
		// Get list 7 games of tournament
		public int[] GetGameTournament()
		{
			return gameTournament;
		}
		// Set list 7 games random tournament
		public void SetGameTournament(int[] gameTournament)
		{
			this.gameTournament = gameTournament;
		}
		// Initialized tournament
		public void StartTournament()
		{
			if(IsTournament == false)
			{
				IsTournament = true;
				for (int i = 0; i < scoreTournament.Length; i++)
				{
					scoreTournament[i] = -1;
				}
			}
		}
		public void ResetScoreTournament()
		{
			if (IsTournament == false)
			{
				for (int i = 0; i < scoreTournament.Length; i++)
				{
					scoreTournament[i] = -1;
				}
				gameOfTournament = 0;
			}
		}
		public void ResetTournament()
		{
			IsTournament = false;
			for (int i = 0; i < scoreTournament.Length; i++)
			{
				scoreTournament[i] = -1;
			}
			gameOfTournament = 0;
		}
		public void EndTournament()
		{
			IsTournament = false;
		}
		public int GetGameOfTournament()
		{
			return gameOfTournament;
		}
		// Get - set game mode
		public void SetGameModeTournament(GameMode mode)
		{
			gameModeTournament = mode;
		}
		public GameMode GetGameModeTournament()
		{
			return gameModeTournament;
		}
		// Set last win tournament
		public void SetLastWinTournament(MainScene.Side side)
		{
			lastWinTournament = (int)side;
		}
		public MainScene.Side GetLastWinTournament()
		{
			return (MainScene.Side)lastWinTournament;
		}
		public void ResetLastWinTournament()
		{
			lastWinTournament = -1;
		}
		// end tournament
		public float GetMusicVolume()
		{
			return PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
		}
		public void SetMusicVolume(float volume)
		{
			PlayerPrefs.SetFloat(MusicVolumeKey, volume);
			OnMusicVolumeChange?.Invoke(volume);
		}
		public float GetSoundVolume()
		{
			return PlayerPrefs.GetFloat(SoundVolumeKey, 1.0f);
		}
		public void SetSoundVolume(float volume)
		{
			PlayerPrefs.SetFloat(SoundVolumeKey, volume);
			OnSoundVolumeChange?.Invoke(volume);
		}
		public bool GetVibrate()
		{
			return PlayerPrefs.GetInt(VibrateKey, 1) == 0 ? false : true;
		}
		public void SetVibrate(bool isVibrate)
		{
			OnVibrateChange?.Invoke(isVibrate);
			if (isVibrate == true)
			{
				PlayerPrefs.SetInt(VibrateKey, 1);
			}
			else
			{
				PlayerPrefs.SetInt(VibrateKey, 0);
			}
		}
		public int GetLastGamePlayed()
		{
			return PlayerPrefs.GetInt(LastGamePlayedKey, 0);
		}
		public void SetLastGamePlayed(int lastGamePlayed)
		{
			PlayerPrefs.SetInt(LastGamePlayedKey, lastGamePlayed);
		}
		private void SaveList<T>(string key, List<T> value)
		{
			if (value == null)
			{
				Logger.Warning("Input list null");
				value = new List<T>();
			}
			if (value.Count == 0)
			{
				PlayerPrefs.SetString(key, string.Empty);
				return;
			}
			if (typeof(T) == typeof(string))
			{
				foreach (var item in value)
				{
					string tempCompare = item.ToString();
					if (tempCompare.Contains(Break))
					{
						throw new Exception("Invalid input. Input contain '~'.");
					}
				}
			}
			PlayerPrefs.SetString(key, string.Join(Break, value));
		}
		private List<T> GetList<T>(string key, List<T> defaultValue)
		{
			if (PlayerPrefs.HasKey(key) == false)
			{
				return defaultValue;
			}
			if (PlayerPrefs.GetString(key) == string.Empty)
			{
				return new List<T>();
			}
			string temp = PlayerPrefs.GetString(key);
			string[] listTemp = temp.Split(Break);
			List<T> list = new List<T>();

			foreach (string s in listTemp)
			{
				list.Add((T)Convert.ChangeType(s, typeof(T)));
			}
			return list;
		}
		public void SaveHistory(List<int> history)
		{
			SaveList(HistoryKey, history);
		}
		public List<int> GetHistory(List<int> defaultValue = null)
		{
			return GetList<int>(HistoryKey, defaultValue);
		}
		public void SaveFavorite(List<int> favorite)
		{
			SaveList(FavoriteKey, favorite);
		}
		public List<int> GetFavorite(List<int> defaultValue = null)
		{
			return GetList<int>(FavoriteKey, defaultValue);
		}
		public bool IsTutorialPlayed()
		{
			int first = PlayerPrefs.GetInt(TutorialKey, 0);
			if (first == 0)
			{
				PlayerPrefs.SetInt(TutorialKey, 1);
				return false;
			}
			else
			{
				return true;
			}
		}
		public GameMode GetLastGameMode()
		{
			int temp = PlayerPrefs.GetInt(GameModeKey, 1);
			return (GameMode)temp;
		}
		public void SaveLastGameMode(GameMode mode)
		{
			PlayerPrefs.SetInt(GameModeKey, (int)mode);
		}
		public void Save()
		{
			PlayerPrefs.Save();
		}
	}
}
