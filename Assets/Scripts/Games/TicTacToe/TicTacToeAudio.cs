using Audio;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TicTacToe
{
    public class TicTacToeAudio : MonoBehaviour
    {
        [SerializeField] private Sounds sounds;
        public void Initialized(AudioService audioService)
        {
            sounds.Initialized(audioService);
        }
        public void Click()
        {
            sounds.PlaySound("click");
        }
    }
}