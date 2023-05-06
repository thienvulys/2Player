using System.Collections;
using System;

using UnityEngine;

using Extensions;
using TMPro;

using Services;
using System.Collections.Generic;

namespace GameUI
{
	public class CountDownUI : MonoBehaviour
	{
		[Header("COUNTDOWN PANEL CONFIG")]
		[SerializeField] private Animator animator;
		[SerializeField] private GameObject targetGroup;
		[SerializeField] private GameObject matchTimeGroup;
		[SerializeField] private TextMeshProUGUI countText;
		[SerializeField] private TextMeshProUGUI matchTimeText;
		[SerializeField] private TextMeshProUGUI targetText;
		[SerializeField] private int timeCountDown = 3;
		[SerializeField] private List<GameObject> countDown;

		private GameServices gameServices;
		private AudioService audioService;

		public void Awake()
		{
			targetGroup.ThrowIfNull();
			matchTimeGroup.ThrowIfNull();
			countText.ThrowIfNull();
			matchTimeText.ThrowIfNull();
			targetText.ThrowIfNull();

			gameServices = GameObject.FindGameObjectWithTag(Constans.ServicesTag).GetComponent<GameServices>();
			audioService = gameServices.GetService<AudioService>();
		}

		[Obsolete("This function need services . Please use StartCountDown(Action onEnded = null) instead.")]
		public void StartCountDown(AudioService audioService, int timeCountDown, Action onEnded = null)
		{
			this.gameObject.SetActive(true);
			this.timeCountDown = timeCountDown;
			audioService.PlayCountDown();
			StartCoroutine(OnCountDown(onEnded));
		}

		public void SetInfo(int winScore , int matchTime)
		{
			matchTimeGroup.SetActive(true);
			targetGroup.SetActive(true);

			targetText.text = winScore.ToString();
			matchTimeText.text = matchTime.ToString();
		}

		public void StartCountDown(int timeCountDown, Action onEnded = null)
		{
			this.gameObject.SetActive(true);
			this.timeCountDown = timeCountDown;
			audioService.PlayCountDown();
			StartCoroutine(OnCountDown(onEnded));
		}

		private IEnumerator OnCountDown(Action onEnded)
		{
			for(int i = timeCountDown ; i > 0 ; i--)
			{
				for(int j = 0; j < countDown.Count; j++)
				{
					if(j != i - 1)
					{
						countDown[j].SetActive(false);
					}
					else
					{
						countDown[j].SetActive(true);
					}
				}
				yield return new WaitForSeconds(1f);
			}
			onEnded?.Invoke();
			gameObject.SetActive(false);
		}
	}
}

