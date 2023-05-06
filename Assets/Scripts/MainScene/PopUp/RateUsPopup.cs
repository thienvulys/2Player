using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Extensions;

namespace MainScene
{
	public class RateUsPopup : MonoBehaviour
	{
		[SerializeField] private Toggle[] starToggles;
		[SerializeField] private GameObject defaultImage;
		[SerializeField] private GameObject happyImage;
		[SerializeField] private GameObject sadImage;
		[SerializeField] private GameObject rateButton;
		[SerializeField] private UnityEvent onClickBackBtn;
		[SerializeField] private UnityEvent<int> onClickRateBtn;

		private int star = 0;

		private void Awake()
		{
			happyImage.ThrowIfNull();
			defaultImage.ThrowIfNull();
			sadImage.ThrowIfNull();
			rateButton.ThrowIfNull();

			defaultImage.SetActive(true);
			happyImage.SetActive(false);
			sadImage.SetActive(false);
			rateButton.SetActive(false);

			for (int i = 0; i < starToggles.Length; i++)
			{
				starToggles[i].isOn = false;
			}
		}


		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}

		public void RateUsButton()
		{
			onClickRateBtn?.Invoke(star);
		}

		public void OnClickStar(int index)
		{
			rateButton.SetActive(true);
			star = index;
			for(int i = 0; i < starToggles.Length; i++)
			{
				starToggles[i].isOn = i < index;
			}

			defaultImage.SetActive(false);
			happyImage.SetActive(false);
			sadImage.SetActive(false);
			switch (index)
			{
				case 1:
				case 2:
				case 3:
					sadImage.SetActive(true);
					break;
				case 4:
				case 5:
					happyImage.SetActive(true);
					break;
			}
		}
	}
}

