using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using UnityEngine.SceneManagement;
using TMPro;
using Audio;
using UnityEngine.EventSystems;

namespace Entry
{
	public class EntryController : MonoBehaviour
	{
		private const string soundObjectName = "Sound";

		[SerializeField] private List<Sound> sounds;
		[SerializeField] private Music music;
		[SerializeField] private GameObject musicObject;

		[Space(8.0f)]
		[SerializeField] private bool isFake = false;
		[SerializeField] private Canvas fakeTouchPrefab;
		[SerializeField] private GameObject touchPrefabs;
		[Space(8.0f)]
		[SerializeField] private Transform bannerTransform;

		private Canvas fakeTouch;
		private GameServices gameServices = null;

		void Awake()
		{

			if (GameObject.FindGameObjectWithTag(Constans.ServicesTag) == null)
			{
				GameObject gameServiceObject = new GameObject(nameof(GameServices));
				gameServiceObject.tag = Constans.ServicesTag;
				gameServices = gameServiceObject.AddComponent<GameServices>();

				// Instantie Audio
				DontDestroyOnLoad(musicObject);

				GameObject soundObject = new GameObject(soundObjectName);
				DontDestroyOnLoad(soundObject);

				gameServices.AddService(new DisplayService());
				gameServices.AddService(new AudioService(music, sounds, soundObject));
				gameServices.AddService(new InputService(EventSystem.current));
				gameServices.AddService(new PlayerService());
				//gameServices.AddService(new FirebaseService(OnFetchSuccess));
				//gameServices.AddService(new AppsFlyerService());
				//gameServices.AddService(new AdsService(bannerTransform, gameServices.GetService<AudioService>()));
				//gameServices.AddService(new TrackingService(gameServices.GetService<FirebaseService>()));
				//gameServices.AddService(new GameService());
				//gameServices.AddService(new IAPService());

				// Setting is show app open ad
				//gameServices.GetService<FirebaseService>().OnShowAppOpenAdChange = gameServices.GetService<AdsService>().OnShowAppOpenAdChange;

				// Reset last game played
				gameServices.GetService<PlayerService>().SetLastGamePlayed(-1);

				//var adsServices = gameServices.GetService<AdsService>();
				//adsServices.EnableAppOpenAdStateChange();
				//gameServices.OnDestroyAction += () =>
				//{
				//	adsServices.DisableAppOpenAdStateChange();
				//};

				DisplayService displayService = gameServices.GetService<DisplayService>();
#if UNITY_EDITOR
				displayService.IsFake = isFake;
				// Instantie Fake Touch
				fakeTouch = Instantiate(fakeTouchPrefab);
				DontDestroyOnLoad(fakeTouch);
				if (gameServices.GetService<InputService>() != null)
				{
					gameServices.GetService<InputService>().FakeTouchs = new List<GameObject>();
					gameServices.GetService<InputService>().FakeTouchImages = new List<Image>();

					Vector3 space = new Vector3(0f, 1f, 0f) * 100;
					for (int i = 0; i < 4; i++)
					{
						GameObject touch = Instantiate(touchPrefabs, fakeTouch.transform);
						touch.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
						touch.transform.position = touch.transform.position + space * (i + 1);
						gameServices.GetService<InputService>().FakeTouchs.Add(touch);
						gameServices.GetService<InputService>().FakeTouchImages.Add(touch.GetComponent<Image>());
					}
				}
				fakeTouch.gameObject.SetActive(false);
#endif
				// Change position Logo
				var safeArea = displayService.SafeArea();

				// Set Volume
				var audioService = gameServices.GetService<AudioService>();
				var playerService = gameServices.GetService<PlayerService>();

				playerService.OnMusicVolumeChange = audioService.SetMusicVolume;
				playerService.OnSoundVolumeChange = audioService.SetSoundVolume;

				playerService.OnVibrateChange = audioService.SetVibrate;

				audioService.MusicVolume = playerService.GetMusicVolume();
				audioService.SoundVolume = playerService.GetSoundVolume();

				audioService.VibrateOn = playerService.GetVibrate();

				audioService.MusicOn = true;
				audioService.SoundOn = true;

				audioService.StopMusic();
			}
		}
		private void Start()
		{
			SceneManager.LoadScene(Constans.MainScreen);
		}
	}
}
