using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Extensions;

namespace MainScene
{
	public class SettingsPopup : MonoBehaviour
	{
		[SerializeField] private Slider musicSlider;
		[SerializeField] private Slider soundSlider;
		[SerializeField] private Slider vibrationSlider;
		//[SerializeField] private Animator musicAnim;
		[SerializeField] private UnityEvent onClickBackBtn;
		[SerializeField] private UnityEvent<float> onSoundChange;
		[SerializeField] private UnityEvent<float> onMusicChange;
		[SerializeField] private UnityEvent<bool> onVibrationChange;
		[SerializeField] private UnityEvent onClickRestoreBtn;
		[SerializeField] private UnityEvent onTermsClick;
		[SerializeField] private UnityEvent onPrivacyClick;
		[SerializeField] private UnityEvent onCreditClick;

		private bool isVibrationOn = false;

		private void Awake()
		{
			vibrationSlider.ThrowIfNull();
			musicSlider.ThrowIfNull();
			vibrationSlider.ThrowIfNull();
			//musicAnim.ThrowIfNull();

			//soundAnim.enabled = false;
			//musicAnim.enabled = false;
		}

		public void LoadSetting(float music , float sound , bool vibration)
		{
			musicSlider.value = music;
			soundSlider.value = sound;
			isVibrationOn = vibration;

			if (vibration)
			{
				StopAllCoroutines();
				StartCoroutine(OnValueChange(vibrationSlider));
			}
		}

		public void BackButton()
		{
			onClickBackBtn?.Invoke();
		}

		public void TermsButton()
		{
			onTermsClick?.Invoke();
		}

		public void PrivacyButton()
		{
			onPrivacyClick?.Invoke();
		}

		public void CreditButton()
		{
			onCreditClick?.Invoke();
		}

		public void SoundSliderPointDown()
		{
			//soundAnim.enabled = true;
		}
		public void SoundSlider(Slider slider)
		{
			//soundAnim.enabled = false;
			onSoundChange?.Invoke(slider.value);
		}

		public void MusicSliderPointDown()
		{
			//musicAnim.enabled = true;
		}

		public void MusicSlider(Slider slider)
		{
			//musicAnim.enabled = false;
			onMusicChange?.Invoke(slider.value);
		}

		public void VibrationToggle(Toggle toggle)
		{
			onVibrationChange?.Invoke(toggle.isOn);
		}

		public void VibrationSlider(Slider slider)
		{
			isVibrationOn = !isVibrationOn;
			onVibrationChange?.Invoke(isVibrationOn);
			StopAllCoroutines();
			StartCoroutine(OnValueChange(slider));
		}
		public void RestoreBtn()
		{
			onClickRestoreBtn?.Invoke();
		}

		IEnumerator OnValueChange(Slider slider)
		{
			float time = 0.1f;
			float timePerStep = 0.02f;
			for(int i = 0; i < time / timePerStep; i++)
			{
				yield return new WaitForSeconds(timePerStep);
				if (isVibrationOn) slider.value += 1 / (time / timePerStep);
				else slider.value -= 1 / (time / timePerStep);
				slider.value = Mathf.Clamp(slider.value , 0f , 1f);
			}
		}
	}
}

