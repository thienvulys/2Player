using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MainScene
{
	public class TournamentWinnerPopup : MonoBehaviour
	{
		[SerializeField] private UnityEvent onClickCaptureBtn;
		[SerializeField] private UnityEvent onClickShareBtn;
		[SerializeField] private UnityEvent onClickPlayAgainBtn;
		[SerializeField] private UnityEvent onClickMainMenuBtn;

		[SerializeField] private GameObject catWinner;
		[SerializeField] private GameObject dogWinner;
		[SerializeField] private GameObject draw;

		[SerializeField] private GameObject botContent;

		public void CaptureBtn()
		{
			onClickCaptureBtn?.Invoke();
		}
		public void ShareBtn()
		{
			onClickShareBtn?.Invoke();
		}
		public void PlayAgainBtn()
		{
			onClickPlayAgainBtn?.Invoke();
		}
		public void MainMenuBtn()
		{
			onClickMainMenuBtn?.Invoke();
		}
		public void OnWin(Side side)
		{
			if (side == Side.Red)
			{
				dogWinner.SetActive(true);
				catWinner.SetActive(false);
				draw.SetActive(false);
			}
			else if (side == Side.Blue)
			{
				dogWinner.SetActive(false);
				catWinner.SetActive(true);
				draw.SetActive(false);
			}
			else
			{
				dogWinner.SetActive(false);
				catWinner.SetActive(false);
				draw.SetActive(true);
			}
		}
		public void HideForTakeScreenShot()
		{
			botContent.SetActive(false);
		}
		public void UnHideForTakeScreenShot()
		{
			botContent.SetActive(true);
		}
	}
}
