using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

using Extensions;
using UnityEngine.UI;

namespace MainScene
{
	public class GameTutorialPopup : MonoBehaviour
	{
		[SerializeField] private Image[] images;
		[SerializeField] private Toggle[] toggles;
		[SerializeField] private HorizontalScrollSnap scrollSnap;
		[SerializeField] private RectTransform viewPort;
		[SerializeField] private RectTransform content;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem,bool> onClickBackBtn;
		private MainSceneModel.GameItem gameItem;

		private void Awake()
		{
			viewPort.ThrowIfNull();
			content.ThrowIfNull();
			scrollSnap.ThrowIfNull();
		}

		public void SetGameItem(MainSceneModel.GameItem gameItem)
		{
			this.gameItem = gameItem;
			for(int i = 0; i < images.Length; i++)
			{
				if (i < gameItem.tutorialSprites.Length)
				{
					toggles[i].gameObject.SetActive(true);
					images[i].gameObject.SetActive(true);
					images[i].rectTransform.SetParent(content);
					images[i].sprite = gameItem.tutorialSprites[i];
				}
				else
				{
					toggles[i].gameObject.SetActive(false);
					images[i].gameObject.SetActive(false);
					images[i].rectTransform.SetParent(viewPort);
				}
			}
			scrollSnap._currentPage = 0;
		}

		public void BackButton()
		{
			onClickBackBtn?.Invoke(gameItem,true);
		}
	}
}

