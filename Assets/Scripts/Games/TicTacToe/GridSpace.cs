using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TicTacToe
{
    public class GridSpace : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Image btnImage;
        private TicTacToeController controller;
        public Side side { get; private set; } = Side.none;
        public bool IsInteractable { get; private set; } = true;
        public int index;
        private TicTacToeAudio tttAudio;
        public void SetSpace()
        {
            btnImage.color = new Color(1f, 1f, 1f, 1f);
            btnImage.sprite = sprites[(int)controller.currentSide];
            side = controller.currentSide;
            controller.EndTurn(index);
            button.interactable = false;
            IsInteractable = false;
            tttAudio.Click();
        }
        public void Init(TicTacToeController controller, TicTacToeAudio audio)
        {
            this.controller = controller;
            this.tttAudio = audio;
        }
        public void DisableBtn()
        {
            button.interactable = false;
        }
        public void Reset()
        {
            btnImage.color = new Color(1f, 1f, 1f, 0f);
            btnImage.sprite = null;
            button.interactable = true;
            IsInteractable = true;
            side = Side.none;
        }
    }
}