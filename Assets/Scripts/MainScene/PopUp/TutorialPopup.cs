using UnityEngine;
using UnityEngine.Events;

namespace MainScene
{
	public class TutorialPopup : MonoBehaviour
	{
		[SerializeField] private UnityEvent onClickBackBtn;
		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}
	}
}

