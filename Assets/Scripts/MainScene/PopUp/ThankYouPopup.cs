using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Extensions;

namespace MainScene
{
	public class ThankYouPopup : MonoBehaviour
	{
		[SerializeField] private GameObject happyImage;
		[SerializeField] private GameObject sadImage;
		[SerializeField] private UnityEvent onClickBackBtn;

		private void Awake()
		{
			happyImage.ThrowIfNull();
			sadImage.ThrowIfNull();

			happyImage.SetActive(false);
			sadImage.SetActive(false);
		}


		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}

		public void SetImage(bool isHappy)
		{
			happyImage.SetActive(isHappy);
			sadImage.SetActive(!isHappy);
		}
	}
}

