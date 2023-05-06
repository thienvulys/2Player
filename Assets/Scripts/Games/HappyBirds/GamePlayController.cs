using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Extensions;

using Parameters;
using Services;

namespace HappyBirds
{
	public enum GameSound
	{
		Crash, Jump, Wall
	}
	public class GamePlayController : MonoBehaviour
	{
		[SerializeField] private GamePlayModel model;
		[SerializeField] private GamePlayView view;
		[SerializeField] private GamePlayAudio audios;
		[SerializeField] private Ship dogShip;
		[SerializeField] private Ship catShip;
		[SerializeField] private Bot bot;
		[SerializeField] private Environment environment;

		private GameServices gameServices;
		private GameMode mode;
		private InputService inputService;
		private AudioService audioService;
		private bool isEnd;

		private Vector3 touchPos;

		private void Awake()
		{
			model.ThrowIfNull();
			view.ThrowIfNull();
			dogShip.ThrowIfNull();
			catShip.ThrowIfNull();
			bot.ThrowIfNull();
			environment.ThrowIfNull();
			//Load Services
			if (GameObject.FindGameObjectWithTag(Constans.ServicesTag) == null)
			{
				SceneManager.LoadScene(Constans.EntryScreen);
			}
			else
			{
				gameServices = GameObject.FindGameObjectWithTag(Constans.ServicesTag).GetComponent<GameServices>();
				inputService = gameServices.GetService<InputService>();
				audioService = gameServices.GetService<AudioService>();
				audios.Initialized(audioService);
			}
			//Get Param
			if (GameObject.FindGameObjectWithTag(Constans.ParamsTag) != null)
			{
				var paramsGO = GameObject.FindGameObjectWithTag(Constans.ParamsTag);
				var gameParams = paramsGO.GetComponent<GameParameters>();
				mode = gameParams.Mode;
				model.WinScore = gameParams.GameItem.WinScore;
				model.TimeMatch = gameParams.GameItem.matchTime;
				Destroy(paramsGO);
			}
			else
			{
				mode = GameMode.Pvp;
			}
			// Set mode
			switch (mode)
			{
				case GameMode.Pvp:
					model.IsBotActive = false;
					break;
				case GameMode.BotEasy:
					model.IsBotActive = true;
					model.BotWinRate = 30f;
					break;
				case GameMode.BotNormal:
					model.IsBotActive = true;
					model.BotWinRate = 50f;
					break;
				case GameMode.BotHard:
					model.IsBotActive = true;
					model.BotWinRate = 80f;
					break;
			}
		}

		private void Start()
		{
			//view.SetGoalAndTimeMatch(model.WinScore, model.TimeMatch);
			// Get parameters from config
			dogShip.Initialize(model.Gravity, model.FlyForce, model.flyDistance, StartChangeThorns, EndMatch , PlayGameSound);
			catShip.Initialize(model.Gravity, model.FlyForce, model.flyDistance, StartChangeThorns, EndMatch, PlayGameSound);
			environment.Initialized(model.MinObtacles, model.MaxObtacles, model.ChangeTime, model.StonePosY ,model.BotWinRate);
			if (model.IsBotActive)
			{
				bot.BotInit(catShip, model.BotWinRate, model.FlyForce, model.flyDistance, model.DelayTouch);
				environment.OnStoneChange = SetDestination;
			}
			//StartCoroutine(OnStart(3f));
			StartCoroutine(Restart(0f));
		}

		private IEnumerator OnStart(float time)
		{
			yield return new WaitForSeconds(time);
			view.StartMatchTime(() =>
			{
				audioService.StopAllSound();
				view.CheckWin(() => audioService.StopAllSound());
				isEnd = true;
			});
		}

		public void StartChangeThorns()
		{
			StartCoroutine(environment.ChangeStones());
		}

		public void SetDestination()
		{
			bot.SetEndPos(environment.RandomMissingStone(catShip.IsLeft));
		}

		public void EndMatch(Ship ship)
		{
			if (isEnd) return;
			if (ship.Equals(dogShip))
			{
				catShip.Stop();
				AddBlueScore();
			}
			else
			{
				dogShip.Stop();
				AddRedScore();
			}
			StartCoroutine(Restart(model.TimeDieDelay));
		}

		private IEnumerator Restart(float time)
		{
			yield return new WaitForSeconds(time);
			catShip.OnInit();
			dogShip.OnInit();
			bot.IsStart = false;
			environment.OnInit();
			if (!isEnd)
			{
				view.StartCountDown(model.TimePerSet, () =>
				{
					catShip.OnStart();
					dogShip.OnStart();
					bot.IsStart = true;
				});
			}
		}

		private void Update()
		{
			if (isEnd)
			{
				audios.StopAllSound();
			}
			for (int i = 0; i < inputService.GetTouchCount(); i++)
			{
				CheckInput(inputService.GetTouch(i));
			}
		}

		public void CheckInput(Services.Touch touch)
		{
			touchPos = Camera.main.ScreenToWorldPoint(touch.Position);
			touchPos.z = 0f;

			if (touch.Phase == Services.TouchPhase.Down)
			{
				if (touchPos.y < 0)
				{
					dogShip.Fly();
				}
				else if (!model.IsBotActive && touchPos.y > 0)
				{
					catShip.Fly();
				}
			}
		}

		public void AddBlueScore()
		{
			view.AddBlueScore(model.WinScore, () => { audios.StopAllSound(); isEnd = true; });
		}
		public void AddRedScore()
		{
			view.AddRedScore(model.WinScore, () => { audios.StopAllSound(); isEnd = true; });
		}

		public void PlayGameSound(GameSound gameSound , bool isVibrate)
		{
			if (isEnd)
			{
				audios.StopAllSound();
				return;
			}
			switch (gameSound)
			{
				case GameSound.Wall:
					audios.PlayWall();
					break;
				case GameSound.Crash:
					audios.PlayCrash();
					break;
				case GameSound.Jump:
					audios.PlayJump();
					break;
			}
			if (isVibrate)
			{
				audioService.Vibrate();
			}
		}
	}
}

