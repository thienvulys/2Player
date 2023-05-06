using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Extensions;

using Services;

namespace GameUI
{
	public class WinUI : MonoBehaviour
	{
		[Header("WIN PANEL CONFIG")]
		[SerializeField] private AudioClip winClip;
		[SerializeField] private RectTransform redPanel;
		[SerializeField] private RectTransform bluePanel;
		[SerializeField] private RectTransform drawPanel;
		[SerializeField] private float timeToExit = 1f;
		[SerializeField] private bool isWin = false;

		private GameServices gameServices;
		private PlayerService playerService;
		private AudioService audioService;
		private void Awake()
		{
			// Check for reference errors
			redPanel.ThrowIfNull();
			bluePanel.ThrowIfNull();
			drawPanel.ThrowIfNull();

			gameServices = GameObject.FindGameObjectWithTag(Constans.ServicesTag).GetComponent<GameServices>();
			playerService = gameServices.GetService<PlayerService>();
			audioService = gameServices.GetService<AudioService>();

			bluePanel.gameObject.SetActive(false);
			redPanel.gameObject.SetActive(false);
			drawPanel.gameObject.SetActive(false);
		}

		[Obsolete("This function need services . Please use ShowBlueWin(Action OnWin = null) instead.")]
		public void ShowBlueWin(AudioService audioService , PlayerService playerService , Action OnWin = null)
		{
			if (isWin) return;
			isWin = true;
			gameObject.SetActive(true);
			//trackingService.TrackingBlueWin();
			audioService.PlayWin();
			OnWin?.Invoke();
			bluePanel.gameObject.SetActive(true);
			playerService.AddBlueScore();
			playerService.SetLastWin(2);
			playerService.SetScoreTournament(MainScene.Side.Blue);
			StartCoroutine(ExitScene());
		}

		public void ShowBlueWin(Action OnWin = null)
		{
			if (isWin) return;
			isWin = true;
			gameObject.SetActive(true);
			//trackingService.TrackingBlueWin();
			audioService.PlayWin();
			OnWin?.Invoke();
			bluePanel.gameObject.SetActive(true);
			playerService.AddBlueScore();
			playerService.SetLastWin(2);
			playerService.SetScoreTournament(MainScene.Side.Blue);
			StartCoroutine(ExitScene());
		}

		[Obsolete("This function need services . Please use ShowRedWin(Action OnWin = null) instead.")]
		public void ShowRedWin(AudioService audioService , PlayerService playerService, Action OnWin = null)
		{
			if (isWin) return;
			isWin = true;
			gameObject.SetActive(true);
			audioService.PlayWin();
			OnWin?.Invoke();
			redPanel.gameObject.SetActive(true);
			playerService.AddRedScore();
			playerService.SetLastWin(1);
			playerService.SetScoreTournament(MainScene.Side.Red);
			StartCoroutine(ExitScene());
		}

		public void ShowRedWin(Action OnWin = null)
		{
			if (isWin) return;
			isWin = true;
			gameObject.SetActive(true);
			audioService.PlayWin();
			OnWin?.Invoke();
			redPanel.gameObject.SetActive(true);
			playerService.AddRedScore();
			playerService.SetLastWin(1);
			playerService.SetScoreTournament(MainScene.Side.Red);
			StartCoroutine(ExitScene());
		}

		public void ShowDraw(Action OnWin = null)
		{
			if (isWin) return;
			isWin = true;
			gameObject.SetActive(true);
			audioService.PlayWin();
			OnWin?.Invoke();
			drawPanel.gameObject.SetActive(true);
			playerService.AddRedScore();
			playerService.AddBlueScore();
			playerService.SetLastWin(0);
			playerService.SetScoreTournament(MainScene.Side.None);
			StartCoroutine(ExitScene());
		}

		private IEnumerator ExitScene()
		{
			yield return new WaitForSeconds(timeToExit);
			//if (iapService.IsRemoveAds() == false)
			//{
			//	adsService.ShowLimitedInterstitialAd();
			//}
			SceneManager.LoadScene(Constans.MainScreen);
		}
	}

}

