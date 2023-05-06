using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MainScene
{
	public class TournamentExit : MonoBehaviour
	{
		[SerializeField] private GameItem[] scoreGame;
		// Button
		[SerializeField] private UnityEvent onClickExitBtn;
		public void ResumeBtn()
		{
			Time.timeScale = 1f;
			gameObject.SetActive(false);
		}
		public void ExitBtn()
		{
			Time.timeScale = 1f;
			onClickExitBtn?.Invoke();
		}
		public void ShowTournament(int[] scoreTournament)
		{
			for (int i = 0; i < scoreTournament.Length; i++)
			{
				if (scoreTournament[i] == -1)
				{
					scoreGame[i].Reset();
				}
				else if (scoreTournament[i] == (int)Side.Red)
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
		}
	}
}
