using UnityEngine;
using UnityEngine.Events;

namespace MainScene
{
	public class WinnerPopup : MonoBehaviour
	{
		[SerializeField] private GameObject CatWinImage;
		[SerializeField] private GameObject DogWinImage;
		[SerializeField] private UnityEvent onClickCameraBtn;
		[SerializeField] private UnityEvent onClickMainBtn;
		[SerializeField] private UnityEvent onClickShareBtn;
		[SerializeField] private UnityEvent onClickPlayAgainBtn;
		public void MainButton()
		{
			onClickMainBtn?.Invoke();
		}
		public void PlayAgainButton()
		{
			onClickPlayAgainBtn?.Invoke();
		}
		public void CameraButton()
		{
			onClickCameraBtn?.Invoke();
		}
		public void ShareButton()
		{
			onClickShareBtn?.Invoke();
		}
	}
}

