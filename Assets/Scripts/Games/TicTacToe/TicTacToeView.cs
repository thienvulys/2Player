using DG.Tweening;
using GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class TicTacToeView : MonoBehaviour
    {
        [SerializeField] private GameObject[] turns;
        [SerializeField] private Image background;
        [SerializeField] private Sprite[] sprites;

        [SerializeField] private ScoreUI scoreUI;
        [SerializeField] private CountDownUI countDownUI;
        [SerializeField] private WinUI winUI;
        [SerializeField] private GameObject block;
        [SerializeField] private Transform tfReset;

        public void ChangeTurn(Side side)
        {
            if(side == Side.red)
            {
                turns[0].SetActive(true);
                turns[1].SetActive(false);
                background.sprite = sprites[0];
            }
            else
            {
                turns[0].SetActive(false);
                turns[1].SetActive(true);
                background.sprite = sprites[1];
            }
        }
        public void StartCountDown(int timeCountDown, Action onEndedCountdown)
        {
            countDownUI.StartCountDown(timeCountDown, onEndedCountdown);
        }
        public void OnWin(Side side)
        {
            winUI.gameObject.SetActive(true);
            if (side == Side.red)
            {
                winUI.ShowRedWin();
            }
            else if (side == Side.blue)
            {
                winUI.ShowBlueWin();
            }
            else
            {
                winUI.ShowDraw();
            }
        }
        public void Block(bool active)
        {
            block.SetActive(active);
        }
        public void Reset()
        {
            tfReset.gameObject.SetActive(true);
            tfReset.DOScale(new Vector3(30f, 30f, 30f), 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                tfReset.localScale = Vector3.zero;
                tfReset.gameObject.SetActive(false);
            });
        }
    }
}
