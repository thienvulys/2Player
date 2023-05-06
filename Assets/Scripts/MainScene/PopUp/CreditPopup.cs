using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Extensions;

namespace MainScene
{
	public class CreditPopup : MonoBehaviour
	{
		[SerializeField] private Scrollbar scrollbar;
		[SerializeField] private RectTransform content;
		[SerializeField] private ScrollRect scrollRect;
		[SerializeField] private float timeDelay;
		[SerializeField] private float timeScroll;
		[SerializeField] private UnityEvent onClickBackBtn;

		private void Awake()
		{
			scrollbar.ThrowIfNull();
			content.ThrowIfNull();
			scrollRect.ThrowIfNull();
		}

		private void OnEnable()
		{
			StartCoroutine(OnRun());
		}

		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}

		IEnumerator OnRun()
		{
			scrollbar.value = 1;
			scrollRect.vertical = false;
			content.anchoredPosition = new Vector2(content.anchoredPosition.x , 0);
			yield return new WaitForSeconds(1f);
			float temp = timeScroll;
			while(temp >= 0)
			{
				yield return new WaitForSeconds(Time.deltaTime);
				temp -= Time.deltaTime;
				scrollbar.value = temp / timeScroll;
			}
			scrollRect.vertical = true;
		}
	}
}

