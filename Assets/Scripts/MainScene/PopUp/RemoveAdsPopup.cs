using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MainScene
{
	public class RemoveAdsPopup : MonoBehaviour
	{
		[SerializeField] private UnityEvent onClickBackBtn;
		[SerializeField] private UnityEvent onClickPurchaseBtn;
		[SerializeField] private TextMeshProUGUI priceText;
		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}
		public void PurchaseButton()
		{
			onClickPurchaseBtn?.Invoke();
		}
		public void ChangePriceText(string price)
		{
			if(price == string.Empty || price == null)
			{
				priceText.text = "49.000 VND";
			}
			else
			{
				priceText.text = price;
			}
		}
	}
}
