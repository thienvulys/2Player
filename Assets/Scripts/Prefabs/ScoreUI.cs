using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Extensions;
using TMPro;
using Services;
using MainScene;
using System.Collections.Generic;

namespace GameUI
{
	public class ScoreUI : MonoBehaviour
	{
		private enum UIPosition
		{
			Center, BottomTop, TopBottom
		}

		[Header("SCORE PANEL CONFIG")]
		[SerializeField] private Animator redAnim;
		[SerializeField] private Animator blueAnim;
		[SerializeField] private Animator goalAnim;
		[SerializeField] private Animator clockAnim;
		[SerializeField] private RectTransform scoreRect;
		[SerializeField] private RectTransform exitRect;
		[SerializeField] private TextMeshProUGUI redScoreText;
		[SerializeField] private TextMeshProUGUI blueScoreText;
		[SerializeField] private TextMeshProUGUI goalText;
		[SerializeField] private TextMeshProUGUI clockText;
		[SerializeField] private UIPosition uIPosition = UIPosition.Center;
		[SerializeField] private float offset = 200f;

		[Header("Tournament")]
		[SerializeField] private TournamentExit tournamentExit;

		//Theme
		[SerializeField] private List<Theme> listThemes;

		private int matchTime = 30;
		private int goal;
		private int scoreWarning;
		private bool isWarning;
		private int redScore;
		private int blueScore;

		private GameServices gameServices;
		//private AdsService adsService;
		//private TrackingService trackingService;
		//private IAPService iapService;
		private PlayerService playerService;
		//private FirebaseService firebaseService;
		public int RedScore
		{
			get => redScore;
			set
			{
				redScore = value;
				redScoreText.text = redScore.ToString();

			}
		}
		public int BlueScore
		{
			get => blueScore;
			set
			{
				blueScore = value;
				blueScoreText.text = blueScore.ToString();
			}
		}

		private void Awake()
		{
			// Check for reference errors
			redAnim.ThrowIfNull();
			blueAnim.ThrowIfNull();
			goalAnim.ThrowIfNull();
			clockAnim.ThrowIfNull();
			scoreRect.ThrowIfNull();
			exitRect.ThrowIfNull();
			redScoreText.ThrowIfNull();
			blueScoreText.ThrowIfNull();
			goalText.ThrowIfNull();
			clockText.ThrowIfNull();

			clockAnim.enabled = false;
			goalAnim.enabled = false;

			gameServices = GameObject.FindGameObjectWithTag(Constans.ServicesTag).GetComponent<GameServices>();
			//adsService = gameServices.GetService<AdsService>();
			//trackingService = gameServices.GetService<TrackingService>();
			//iapService = gameServices.GetService<IAPService>();
			playerService = gameServices.GetService<PlayerService>();
			//firebaseService = gameServices.GetService<FirebaseService>();

			//ChangeBackgroundTheme(firebaseService.GetThemeBackground());
		}
		private void Start()
		{
			if (uIPosition == UIPosition.BottomTop)
			{
				scoreRect.anchoredPosition = new Vector2(scoreRect.anchoredPosition.x, -(Screen.height / 2 - scoreRect.sizeDelta.y / 2 - offset));
				exitRect.anchoredPosition = new Vector2(exitRect.anchoredPosition.x, (Screen.height / 2 - exitRect.sizeDelta.y / 2 - offset));
			}
			else if (uIPosition == UIPosition.TopBottom)
			{
				scoreRect.anchoredPosition = new Vector2(scoreRect.anchoredPosition.x, (Screen.height / 2 - scoreRect.sizeDelta.y / 2 - offset));
				exitRect.anchoredPosition = new Vector2(exitRect.anchoredPosition.x, -(Screen.height / 2 - exitRect.sizeDelta.y / 2 - offset));
			}
		}
		public void SetGoal(int goal, bool isWarning = false, int scoreWarning = 0)
		{
			this.goal = goal;
			this.isWarning = isWarning;
			this.scoreWarning = scoreWarning;

			goalText.text = goal.ToString();
		}

		public void SetMatchTime(int matchTime)
		{
			this.matchTime = matchTime;
			this.clockText.text = matchTime.ToString();
		}

		public void StartMatchTime(int time, Action onEnded = null)
		{
			StartCoroutine(OnPlayTime(time, onEnded));
		}

		public void StartMatchTime(Action onEnded = null)
		{
			StartCoroutine(OnPlayTime(matchTime, onEnded));
		}

		private IEnumerator OnPlayTime(int time, Action onEnded)
		{
			for (int i = time; i >= 0; i--)
			{
				if (i == 5)
				{
					clockAnim.enabled = true;
				}
				clockText.text = i.ToString();
				yield return new WaitForSeconds(1f);
			}
			onEnded?.Invoke();
		}
		//Show score the Red side
		public void ShowRedScore(int score)
		{
			if (RedScore == score) return;
			RedScore = score;
			redScoreText.text = RedScore.ToString();
			ChangAnim(redAnim);
			goalAnim.enabled = isWarning && redScore >= scoreWarning;
		}

		//Show score the Blue side
		public void ShowBlueScore(int score)
		{
			if (BlueScore == score) return;
			BlueScore = score;
			blueScoreText.text = BlueScore.ToString();
			ChangAnim(blueAnim);
			goalAnim.enabled = isWarning && blueScore >= scoreWarning;
		}

		public void ExitButton()
		{
			if(playerService.IsTournament == true)
			{
				tournamentExit.gameObject.SetActive(true);
				tournamentExit.ShowTournament(playerService.GetScoreTournament());
				Time.timeScale = 0f;
			}
			else
			{
				ExitGame();
			}
		}
		public void EndTournament()
		{
			playerService?.ResetTournament();
		}
		public void ExitGame()
		{
			//trackingService.StopGameTracking();
			//if (iapService.IsRemoveAds() == false)
			//{
			//	adsService.ShowLimitedInterstitialAd();
			//}
			SceneManager.LoadScene(Constans.MainScreen);
		}
		private void ChangAnim(Animator animator)
		{
			animator.ResetTrigger("ZoomScore");
			animator.SetTrigger("Idle");
			animator.SetTrigger("ZoomScore");
		}
		public void ChangeBackgroundTheme(int theme)
		{
			if (theme >= listThemes.Count)
			{
				return;
			}
			for (int i = 0; i < listThemes.Count; i++)
			{
				if (i != theme)
				{
					foreach (var go in listThemes[i].listBackgrounds)
					{
						go.SetActive(false);
					}
				}
			}
			foreach (var go in listThemes[theme].listBackgrounds)
			{
				go.SetActive(true);
			}
		}
	}

}

