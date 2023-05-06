using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Extensions;
using Parameters;
using TMPro;

namespace MainScene
{
	public class GameInfoPopup : MonoBehaviour
	{
		[Header("MODE PREFERENCE")]
		[SerializeField] private TextMeshProUGUI tutorialText;
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private Button backBtn;
		[SerializeField] private Button vsBotBtn;
		[SerializeField] private Button vsPlayerBtn;
		[SerializeField] private Button tutorialBtn;
		[Header("BUTTON EVENT")]
		[SerializeField] private UnityEvent onClickBackBtn;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem, GameMode> onClickPvpBtn;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem> onClickBotBtn;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem> onClickTutoritalBtn;

		private MainSceneModel.GameItem gameItem;
		private void Awake()
		{
			tutorialText.ThrowIfNull();
			nameText.ThrowIfNull();
			backBtn.ThrowIfNull();
			vsBotBtn.ThrowIfNull();
			vsPlayerBtn.ThrowIfNull();
			tutorialBtn.ThrowIfNull();
		}

		public void SetGameItem(MainSceneModel.GameItem gameItem)
		{
			this.gameItem = gameItem;
			nameText.text = gameItem.name;
			tutorialText.text = gameItem.tutorial;
			vsBotBtn.gameObject.SetActive(gameItem.isVSBot);
			vsPlayerBtn.gameObject.SetActive(gameItem.isVSPlayer);
		}

		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}

		public void VSBotButton()
		{
			onClickBotBtn?.Invoke(gameItem);
		}

		public void VSPlayerButton()
		{
			onClickPvpBtn?.Invoke(gameItem,GameMode.Pvp);
		}

		public void TutorialButton()
		{
			onClickTutoritalBtn?.Invoke(gameItem);
		}
	}
}

