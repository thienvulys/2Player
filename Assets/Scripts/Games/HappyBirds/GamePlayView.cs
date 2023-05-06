using System;

using UnityEngine;

using Extensions;

using GameUI;
using Services;
using System.Collections;

namespace HappyBirds
{
	public class GamePlayView : MonoBehaviour
	{
		[SerializeField] private ScoreUI scoreUI;
		[SerializeField] private CountDownUI countDownUI;
		[SerializeField] private WinUI winUI;
		[SerializeField] private float delayTimeWin = 1f;

		public CountDownUI CountDownUI => countDownUI;
		public ScoreUI ScoreUI => scoreUI;
		public WinUI WinUI => winUI;

		private void Awake()
		{
			scoreUI.ThrowIfNull();
			countDownUI.ThrowIfNull();
			winUI.ThrowIfNull();
		}
		public Vector2 GetViewOfCamera()
		{
			float viewY = Camera.main.orthographicSize * 2f;
			float viewX = viewY * Screen.width / Screen.height;
			return new Vector2(viewX, viewY);
		}

		public void StartCountDown(int timeCountDown, Action onEndedCountdown)
		{
			countDownUI.StartCountDown(timeCountDown, onEndedCountdown);
		}

		public void StartMatchTime(Action onEnded = null)
		{
			scoreUI.StartMatchTime(onEnded);
		}

		public void SetGoalAndTimeMatch(int winScore , int timeMatch)
		{
			scoreUI.SetGoal(winScore, true, winScore - 1);
			scoreUI.SetMatchTime(timeMatch);
			countDownUI.SetInfo(winScore ,timeMatch);
		}

		public void CheckWin(Action action)
		{
			if (scoreUI.BlueScore > scoreUI.RedScore)
			{
				action?.Invoke();
				winUI.gameObject.SetActive(true);
				winUI.ShowBlueWin();
			}
			else if (scoreUI.BlueScore < scoreUI.RedScore)
			{
				action?.Invoke();
				winUI.gameObject.SetActive(true);
				winUI.ShowRedWin();
			}
			else
			{
				action?.Invoke();
				winUI.gameObject.SetActive(true);
				winUI.ShowDraw();
			}
		}
		public bool AddBlueScore(int winScore , Action action)
		{
			scoreUI.ShowBlueScore(scoreUI.BlueScore + 1);
			if (winScore <= scoreUI.BlueScore)
			{
				StartCoroutine(OnWin(delayTimeWin, () =>
				{
					action?.Invoke();
					winUI.gameObject.SetActive(true);
					winUI.ShowBlueWin();
				}));
				return true;
			}
			return false;
		}
		public bool AddRedScore(int winScore , Action action)
		{
			scoreUI.ShowRedScore(scoreUI.RedScore + 1);
			if (winScore <= scoreUI.RedScore)
			{
				StartCoroutine(OnWin(delayTimeWin, () =>
				{
					action?.Invoke();
					winUI.gameObject.SetActive(true);
					winUI.ShowRedWin();
				}));
				return true;
			}
			return false;
		}

		private IEnumerator OnWin(float time, Action action)
		{
			yield return new WaitForSeconds(time);
			action?.Invoke();
		}
	}
}

