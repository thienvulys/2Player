using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Extensions;
using Parameters;
using TMPro;

namespace MainScene
{
	public class ModePopup : MonoBehaviour
	{
		//[SerializeField] private Toggle easyToggle;
		//[SerializeField] private Toggle normalToggle;
		//[SerializeField] private Toggle hardToggle;
		//[SerializeField] private Image easyBG;
		//[SerializeField] private Image normalBG;
		//[SerializeField] private Image hardBG;
		//[SerializeField] private GameObject easyCheck;
		//[SerializeField] private GameObject normalCheck;
		//[SerializeField] private GameObject hardCheck;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem,GameMode> onClickPlayBtn;
		[SerializeField] private UnityEvent<MainSceneModel.GameItem ,bool> onClickBackBtn;
		[SerializeField] private UnityEvent<GameMode> onModeChange;

		private GameMode mode;
		private MainSceneModel.GameItem gameItem;


		private void Awake()
		{
			//easyToggle.ThrowIfNull();
			//normalToggle.ThrowIfNull();
			//hardToggle.ThrowIfNull();
			//easyCheck.ThrowIfNull();
			//normalCheck.ThrowIfNull();
			//hardCheck.ThrowIfNull();
			//easyBG.ThrowIfNull();
			//normalBG.ThrowIfNull();
			//hardBG.ThrowIfNull();
		}

		//public void UpdateStatus()
		//{
		//	easyCheck.SetActive(easyToggle.isOn);
		//	normalCheck.SetActive(normalToggle.isOn);
		//	hardCheck.SetActive(hardToggle.isOn);
		//	easyBG.enabled = !easyToggle.isOn;
		//	normalBG.enabled = !normalToggle.isOn;
		//	hardBG.enabled = !hardToggle.isOn;
		//}

		//public void SetMode(GameMode mode)
		//{
		//	this.mode = mode;
		//	switch (this.mode)
		//	{
		//		case GameMode.BotEasy:
		//			easyToggle.isOn = true;
		//			break;
		//		case GameMode.BotNormal:
		//			normalToggle.isOn = true;
		//			break;
		//		case GameMode.BotHard:
		//			hardToggle.isOn = true;
		//			break;
		//	}
		//	UpdateStatus();
		//}

		public void SetGameItem(MainSceneModel.GameItem gameItem)
		{
			this.gameItem = gameItem;
		}

		//public void OnToggleChange(Toggle toggle)
		//{
		//	UpdateStatus();
		//	if (toggle == easyToggle && easyToggle.isOn)
		//	{
		//		mode = GameMode.BotEasy;
		//		onModeChange.Invoke(mode);
		//	}
		//	else if (toggle == normalToggle && normalToggle.isOn)
		//	{
		//		mode = GameMode.BotNormal;
		//		onModeChange.Invoke(mode);
		//	}
		//	else if (toggle == hardToggle && hardToggle.isOn)
		//	{
		//		mode = GameMode.BotHard;
		//		onModeChange.Invoke(mode);
		//	}
		//}

		public void SelectModeButton(int modeIndex)
		{
			switch (modeIndex)
			{
				case 1:
					mode = GameMode.BotEasy;
					break;
				case 2:
					mode = GameMode.BotNormal;
					break;
				case 3:
					mode = GameMode.BotHard;
					break;
			}
			onClickPlayBtn?.Invoke(gameItem, mode);
		}

		public void PlayButton()
		{
			onClickPlayBtn?.Invoke(gameItem,mode);
		}
		public void BackButton()
		{
			onClickBackBtn?.Invoke(gameItem,true);
		}
	}
}

