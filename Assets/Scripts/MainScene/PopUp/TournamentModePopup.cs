using Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace MainScene
{
	public class TournamentModePopup : MonoBehaviour
	{
		[SerializeField] private UnityEvent onClickBackBtn;
		[SerializeField] private UnityEvent<GameMode> onClickPvpBtn;
		[SerializeField] private UnityEvent onClickBotBtn;
		public void BackBtn()
		{
			onClickBackBtn?.Invoke();
		}
		public void PvpBtn()
		{
			onClickPvpBtn?.Invoke(GameMode.Pvp);
		}
		public void BotBtn()
		{
			onClickBotBtn?.Invoke();
		}
	}
}
