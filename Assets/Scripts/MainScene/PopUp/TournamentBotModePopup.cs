using Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace MainScene
{
	public class TournamentBotModePopup : MonoBehaviour
	{
		[SerializeField] private UnityEvent<GameMode> onClickModeBtn;
		[SerializeField] private UnityEvent onClickBackBtn;
		public void EasyBtn()
		{
			onClickModeBtn?.Invoke(GameMode.BotEasy);
		}
		public void NormalBtn()
		{
			onClickModeBtn?.Invoke(GameMode.BotNormal);
		}
		public void HardBtn()
		{
			onClickModeBtn?.Invoke(GameMode.BotHard);
		}
		public void BackBtn()
		{
			onClickBackBtn?.Invoke();
		}
	}
}
