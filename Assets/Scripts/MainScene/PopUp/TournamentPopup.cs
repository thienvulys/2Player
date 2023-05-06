using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Extensions;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections;

namespace MainScene
{
	public enum Side
	{
		Red,
		Blue,
		None
	}
	public class TournamentPopup : MonoBehaviour
	{
		// TOP
		[SerializeField] private GameItem[] scoreGame;
		[SerializeField] private TextMeshProUGUI scoreRedText;
		[SerializeField] private TextMeshProUGUI scoreBlueText;
		// BOT
		[SerializeField] private Image gameIcon;
		[SerializeField] private TextMeshProUGUI nameGameText;
		[SerializeField] private TextMeshProUGUI tutorialText;
		// Button
		[SerializeField] private UnityEvent onClickPlayBtn;
		[SerializeField] private UnityEvent onClickBackBtn;
		// Anim
		[SerializeField] private Animator animRed;
		[SerializeField] private Animator animBlue;

		[SerializeField] private GameObject botPanel;
		private string animZoomText = "onZoom";
		private void Awake()
		{
			scoreRedText.ThrowIfNull();
			scoreBlueText.ThrowIfNull();
			gameIcon.ThrowIfNull();
			nameGameText.ThrowIfNull();
			tutorialText.ThrowIfNull();
			botPanel.SetActive(true);
		}
		public void ShowTournament(int[] scoreTournament, int gameOfTournament, MainSceneModel.GameItem gameItem)
		{

			if(gameOfTournament == 0)
			{
				int scoreRed = scoreTournament.Count(s => s == (int)Side.Red);
				int scoreBlue = scoreTournament.Count(s => s == (int)Side.Blue);
				scoreRedText.text = scoreRed.ToString();
				scoreBlueText.text = scoreBlue.ToString();
			}
			else
			{
				if(gameOfTournament > 1)
				{
					int[] temp = new int[gameOfTournament - 1];
					Array.Copy(scoreTournament, 0, temp, 0, gameOfTournament - 1);
					int scoreRed = temp.Count(s => s == (int)Side.Red);
					int scoreBlue = temp.Count(s => s == (int)Side.Blue);
					scoreRedText.text = scoreRed.ToString();
					scoreBlueText.text = scoreBlue.ToString();
				}
				if(gameOfTournament > 0)
				{
					int scoreRed = scoreTournament.Count(s => s == (int)Side.Red);
					int scoreBlue = scoreTournament.Count(s => s == (int)Side.Blue);

					int lastWin = scoreTournament[gameOfTournament - 1];
					if (lastWin == (int)Side.Red)
					{
						animRed.ResetTrigger(animZoomText);
						animRed.SetTrigger(animZoomText);
						StartCoroutine(DelayForAddScore(lastWin, scoreRed));
					}
					else if (lastWin == (int)Side.Blue)
					{
						animBlue.ResetTrigger(animZoomText);
						animBlue.SetTrigger(animZoomText);
						StartCoroutine(DelayForAddScore(lastWin, scoreBlue));
					}
				}
			}
			for(int i = 0; i < scoreTournament.Length; i++)
			{
				if (scoreTournament[i] == -1)
				{
					scoreGame[i].Reset();
				}
				else if(scoreTournament[i] == (int)Side.Red)
				{
					scoreGame[i].SetDogWin();
				}
				else if (scoreTournament[i] == (int)Side.Blue)
				{
					scoreGame[i].SetCatWin();
				}
				else if (scoreTournament[i] == (int)Side.None)
				{
					scoreGame[i].SetDraw();
				}
			}
			if(gameOfTournament != 0)
			{
				scoreGame[gameOfTournament - 1].RunAnimOnWin();
			}
			gameIcon.sprite = gameItem.sprite;
			nameGameText.text = gameItem.name;
			tutorialText.text = gameItem.tutorial;
		}
		public void PlayBtn()
		{
			onClickPlayBtn?.Invoke();
		}
		public void BackBtn()
		{
			onClickBackBtn?.Invoke();
		}
		private IEnumerator DelayForAddScore(int lastWin, int score)
		{
			yield return new WaitForSeconds(1f);
			if (lastWin == (int)Side.Red)
			{
				scoreRedText.text = score.ToString();
			}
			else if (lastWin == (int)Side.Blue)
			{
				scoreBlueText.text = score.ToString();
			}
		}
		public void BlockForWin()
		{
			botPanel.SetActive(false);
		}
		public void UnBlockForWin()
		{
			botPanel.SetActive(true);
		}
	}
}
