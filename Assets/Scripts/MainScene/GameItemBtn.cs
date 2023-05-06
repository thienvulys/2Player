using System;

using UnityEngine;
using UnityEngine.UI;

using Extensions;
using TMPro;

namespace MainScene
{
	public class GameItemBtn : MonoBehaviour
	{
		[SerializeField]private RectTransform buttonTransform;
		[SerializeField]private Image image;
		[SerializeField]private GameObject starOn;
		[SerializeField]private TextMeshProUGUI gameName;
		[SerializeField]private Button favoriteButton;
		[SerializeField]private GameObject premiumCare;

		[SerializeField]private GameObject frame;
		public Action<MainSceneModel.GameItem , bool> OnCLick;
		public Action<MainSceneModel.GameItem, bool> OnToggleChange;
		private MainSceneModel.GameItem gameItem;
		private void Awake()
		{
			buttonTransform.ThrowIfNull();
			image.ThrowIfNull();
			starOn.ThrowIfNull();
			gameName.ThrowIfNull();
			favoriteButton.ThrowIfNull();
		}

		public void FavoriteChange()
		{
			gameItem.isFavorite = !gameItem.isFavorite;
			starOn.SetActive(gameItem.isFavorite);
			OnToggleChange?.Invoke(gameItem, gameItem.isFavorite);
		}

		public void OnClickButton()
		{
			OnCLick?.Invoke(gameItem, false);
		}

		public void SetGameItem(MainSceneModel.GameItem gameItem)
		{
			this.gameItem = gameItem;
			image.sprite = gameItem.sprite;
			gameName.text = gameItem.name;
			starOn.SetActive(gameItem.isFavorite);
			premiumCare.SetActive(gameItem.watchAdsToPlay);
		}

		public void SetScaler(float scaler)
		{
			transform.localScale = new Vector3(scaler, scaler, scaler);
		}

		public void SetParentAndPosition(Transform parent , Vector2 position)
		{
			buttonTransform.SetParent(parent);
			//buttonTransform.localScale = Vector3.one;
			buttonTransform.anchoredPosition = position;
		}
		public void SetTheme(int theme)
		{
			if(theme == 0)
			{
				frame.SetActive(false);
			}
			else
			{
				frame.SetActive(true);
			}
		}
	}
}

