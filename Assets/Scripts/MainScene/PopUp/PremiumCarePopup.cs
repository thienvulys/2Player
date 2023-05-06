using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainScene
{
	public class PremiumCarePopup : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[SerializeField] private TextMeshProUGUI nameGameText;
		[SerializeField] private TextMeshProUGUI descriptionText;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem> onClickWatchAdsBtn;
		[SerializeField] private UnityEvent onClickPurchaseRemoveAdsBtn;
		[SerializeField] private UnityEvent onClickBackBtn;
		[SerializeField] private GameObject btnWatchAds;

		private MainSceneModel.GameItem gameItem;
		public void SetGameItem(MainSceneModel.GameItem gameItem)
		{
			this.gameItem = gameItem;
			nameGameText.text = gameItem.name;
			descriptionText.text = gameItem.tutorial;
			icon.sprite = gameItem.sprite;
		}
		public void WatchAdsBtn()
		{
			onClickWatchAdsBtn?.Invoke(gameItem);
		}
		public void PurchaseRemoveAdsBtn()
		{
			onClickPurchaseRemoveAdsBtn?.Invoke();
		}
		public void BackBtn()
		{
			onClickBackBtn?.Invoke();
		}
		public void HideBtnWatchAds(bool active)
		{
			if(btnWatchAds != null)
			{
				btnWatchAds.SetActive(active);
			}
		}
	}
}
