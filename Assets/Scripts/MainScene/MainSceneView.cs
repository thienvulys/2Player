using System.Collections;

using UnityEngine;

using Extensions;
using TMPro;

using Services;
using System.Collections.Generic;

namespace MainScene
{
	[System.Serializable]
	public struct Theme
	{
		public List<GameObject> listBackgrounds;
	}
	public class MainSceneView : MonoBehaviour
	{
		public enum UIPopup
		{
			Main,
			Setting,
			Info,
			Difficulty,
			GameTutorial,
			RemoveAds,
			Winner,
			Tournament,
			Tutorial,
			Credit,
			RateUs,
			ThankYou,
			TournamentMode,
			TournamentBotMode,
			TournamentWinner,
			TournamentExit,
			PremiumCare
		}

		[Header("MAIN BODY PREFERENCE")]
		[SerializeField] private TextMeshProUGUI redScoreText;
		[SerializeField] private TextMeshProUGUI blueScoreText;
		[SerializeField] private SettingsPopup settingsPopup;
		[SerializeField] private GameInfoPopup gameInfoPopup;
		[SerializeField] private ModePopup modePopup;
		[SerializeField] private TutorialPopup tutorialPopup;
		[SerializeField] private RateUsPopup rateUsPopup;
		[SerializeField] private ThankYouPopup thankYouPopup;
		[SerializeField] private GameTutorialPopup gameTutorialPopup;

		[SerializeField] private Animator dogAnim;
		[SerializeField] private Animator catAnim;

		[SerializeField] private RectTransform topMainPanel;
		[SerializeField] private RectTransform centerMainPanel;
		[SerializeField] private RectTransform botMainPanel;
		[SerializeField] private RectTransform topInfoPanel;
		[SerializeField] private RectTransform botInfoPanel;
		[SerializeField] private GameObject pawPrefab;
		[SerializeField] private GameObject clawPrefab;


		[SerializeField] private int minPaw = 4;
		[SerializeField] private int maxPaw = 10;

		//Theme
		[SerializeField] private List<Theme> listThemes;
		[SerializeField] private RectTransform snapScrollView;

		public GameInfoPopup GameInfoPopup => gameInfoPopup;
		public SettingsPopup SettingsPopup => settingsPopup;
		public ModePopup ModePopup => modePopup;
		public ThankYouPopup ThankYouPopup => thankYouPopup;
		public GameTutorialPopup GameTutorialPopup => gameTutorialPopup;
		private bool isAvoidBanner = false;
		private void Awake()
		{
			//Stop when any preference is Null
			redScoreText.ThrowIfNull();
			blueScoreText.ThrowIfNull();
			settingsPopup.ThrowIfNull();
			gameInfoPopup.ThrowIfNull();
			tutorialPopup.ThrowIfNull();
			rateUsPopup.ThrowIfNull();
			thankYouPopup.ThrowIfNull();
			gameTutorialPopup.ThrowIfNull();

			dogAnim.ThrowIfNull();
			catAnim.ThrowIfNull();

			topMainPanel.ThrowIfNull();
			centerMainPanel.ThrowIfNull();
			botMainPanel.ThrowIfNull();
			topInfoPanel.ThrowIfNull();
			botInfoPanel.ThrowIfNull();
			pawPrefab.ThrowIfNull();
			clawPrefab.ThrowIfNull();

			snapScrollView.ThrowIfNull();

			dogAnim.enabled = false;
			catAnim.enabled = false;
		}

		public void SetScore(PlayerService playerService)
		{
			redScoreText.text = playerService.RedScore.ToString();
			blueScoreText.text = playerService.BlueScore.ToString();
			switch (playerService.LastWin)
			{
				case 0:
					dogAnim.enabled = true;
					catAnim.enabled = true;
					break;
				case 1:
					dogAnim.enabled = true;
					break;
				case 2:
					catAnim.enabled = true;
					break;
			}
		}

		public void OpenPopup(UIPopup popup)
		{
			settingsPopup.gameObject.SetActive(false);
			if (popup != UIPopup.Difficulty && popup != UIPopup.GameTutorial && popup != UIPopup.PremiumCare) gameInfoPopup.gameObject.SetActive(false);
			modePopup.gameObject.SetActive(false);
			tutorialPopup.gameObject.SetActive(false);
			rateUsPopup.gameObject.SetActive(false);
			thankYouPopup.gameObject.SetActive(false);
			gameTutorialPopup.gameObject.SetActive(false);

			switch (popup)
			{
				case UIPopup.Setting:
					settingsPopup.gameObject.SetActive(true);
					break;
				case UIPopup.Info:
					gameInfoPopup.gameObject.SetActive(true);
					break;
				case UIPopup.GameTutorial:
					gameTutorialPopup.gameObject.SetActive(true);
					gameInfoPopup.gameObject.SetActive(true);
					break;
				case UIPopup.Difficulty:
					gameInfoPopup.gameObject.SetActive(true);
					modePopup.gameObject.SetActive(true);
					break;
				case UIPopup.Tutorial:
					tutorialPopup.gameObject.SetActive(true);
					break;
				case UIPopup.RateUs:
					rateUsPopup.gameObject.SetActive(true);
					break;
				case UIPopup.ThankYou:
					thankYouPopup.gameObject.SetActive(true);
					break;
			}
		}
		public void PutMainLayer()
		{
		}

		public void PutBottomLayer()
		{
			//PutRandomPawAndClaw(bottomLayer);
		}

		public void PutRandomPawAndClaw(RectTransform rectTransform)
		{
			foreach (RectTransform rect in rectTransform)
			{
				SimplePool.Despawn(rect.gameObject);
			}

			int rd = Random.Range(minPaw, maxPaw);

			for (int i = 0; i < rd; i++)
			{
				GameObject prefab = Random.Range(0, 2) == 0 ? pawPrefab : clawPrefab;
				Vector2 pos = new Vector2(Random.Range(0, rectTransform.rect.width), Random.Range(0, rectTransform.rect.height));
				Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 60f));
				RectTransform rect = SimplePool.Spawn(prefab, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
				rect.SetParent(rectTransform);
				rect.anchoredPosition = pos;
				rect.rotation = rotation;
				rect.localScale = Vector3.one * Random.Range(0.6f, 1.1f);
			}
		}
		public void AvoidCutout(Rect safeArea, Rect[] cutouts)
		{
			if (safeArea.y == 0)
			{
				float posY = safeArea.height;
				foreach (Rect rect in cutouts)
				{
					if (posY > rect.y)
					{
						posY = rect.y;
					}
				}
				safeArea.y = Screen.height - posY;
			}
			topMainPanel.anchoredPosition = new Vector2(topMainPanel.anchoredPosition.x, topMainPanel.anchoredPosition.y - safeArea.y);
			topInfoPanel.anchoredPosition = new Vector2(topInfoPanel.anchoredPosition.x, topInfoPanel.anchoredPosition.y - safeArea.y);

			centerMainPanel.sizeDelta = new Vector2(centerMainPanel.sizeDelta.x, centerMainPanel.sizeDelta.y - safeArea.y);
			centerMainPanel.anchoredPosition = new Vector2(centerMainPanel.anchoredPosition.x, centerMainPanel.anchoredPosition.y - safeArea.y / 2);
		}
		public void AvoidBanner(float bannerHeight)
		{
			isAvoidBanner = true;
			centerMainPanel.sizeDelta = new Vector2(centerMainPanel.sizeDelta.x, centerMainPanel.sizeDelta.y - bannerHeight);
			centerMainPanel.anchoredPosition = new Vector2(centerMainPanel.anchoredPosition.x, centerMainPanel.anchoredPosition.y + bannerHeight / 2);
			botMainPanel.anchoredPosition = new Vector2(botMainPanel.anchoredPosition.x, botMainPanel.anchoredPosition.y + bannerHeight);
		}
		public void DeleteBanner(float bannerHeight)
		{
			if(isAvoidBanner)
			{
				centerMainPanel.sizeDelta = new Vector2(centerMainPanel.sizeDelta.x, centerMainPanel.sizeDelta.y + bannerHeight);
				centerMainPanel.anchoredPosition = new Vector2(centerMainPanel.anchoredPosition.x, centerMainPanel.anchoredPosition.y - bannerHeight / 2);
				botMainPanel.anchoredPosition = new Vector2(botMainPanel.anchoredPosition.x, botMainPanel.anchoredPosition.y - bannerHeight);
			}
		}
		public void HideRemoveAdsButton()
		{
		}
		public void HideWatchAds(bool active)
		{
			//premiumCarePopup.HideBtnWatchAds(active);
		}
		public void ChangeBackgroundTheme(int theme)
		{
			// 0: new
			// 1: old
			if (theme >= listThemes.Count)
			{
				return;
			}
			for (int i = 0; i < listThemes.Count; i++)
			{
				if (i != theme)
				{
					foreach (var go in listThemes[i].listBackgrounds)
					{
						go.SetActive(false);
					}
				}
			}
			foreach (var go in listThemes[theme].listBackgrounds)
			{
				go.SetActive(true);
			}
			switch (theme)
			{
				case 0:
					snapScrollView.offsetMax = new Vector2(snapScrollView.offsetMax.x, -302f);
					break;
				case 1:
					snapScrollView.offsetMax = new Vector2(snapScrollView.offsetMax.x, -292f);
					break;
			}
		}
	}
}
