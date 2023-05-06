using UnityEngine;

using Services;
using Audio;

namespace HappyBirds
{
	public class GamePlayAudio : MonoBehaviour
	{
		[SerializeField] private Sounds sounds;
		public void Initialized(AudioService audioService)
		{
			sounds.Initialized(audioService);
		}
		public void PlayCrash()
		{
			sounds.PlaySound("crash");
		}
		public void PlayJump()
		{
			sounds.PlaySound("jump");
		}
		public void PlayWall()
		{
			sounds.PlaySound("wall");
		}

		public void StopAllSound()
		{
			sounds.StopSound("crash");
			sounds.StopSound("jump");
			sounds.StopSound("wall");
		}
	}
}

