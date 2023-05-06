using Parameters;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace TicTacToe
{
    public enum Side
    {
        red,
        blue,
        none
    }
    public class TicTacToeController : MonoBehaviour
    {
        private const char player = 'X';
        private const char computer = 'O';
        private const char gachngang = '-';
        [SerializeField] private TicTacToeView view;
        [SerializeField] private List<GridSpace> gridSpaces;
        [SerializeField] private TicTacToeAudio audioTTT;
        public Side currentSide { get; private set; } = Side.red;

        private GameServices gameServices;
        private GameMode mode;
        private InputService inputService;
        private AudioService audioService;

        private int moveCount = 0;
        private bool isWin = false;
        private char[] board = new char[9] { '-', '-', '-', '-', '-', '-', '-', '-', '-' };
        private Side firstSide = Side.red;
        private void Awake()
        {
            //Load Services
            if (GameObject.FindGameObjectWithTag(Constans.ServicesTag) == null)
            {
                SceneManager.LoadScene(Constans.EntryScreen);
            }
            else
            {
                gameServices = GameObject.FindGameObjectWithTag(Constans.ServicesTag).GetComponent<GameServices>();
                inputService = gameServices.GetService<InputService>();
                audioService = gameServices.GetService<AudioService>();
                audioTTT.Initialized(audioService);
            }
            //Get Param
            if (GameObject.FindGameObjectWithTag(Constans.ParamsTag) != null)
            {
                var paramsGO = GameObject.FindGameObjectWithTag(Constans.ParamsTag);
                var gameParams = paramsGO.GetComponent<GameParameters>();
                mode = gameParams.Mode;
                Destroy(paramsGO);
            }
            else
            {
                mode = GameMode.Pvp;
            }

            for (int i = 0; i < gridSpaces.Count; i++)
            {
                gridSpaces[i].Init(this, audioTTT);
                gridSpaces[i].index = i;
            }
        }
        private void Start()
        {
            view.StartCountDown(3, StartGame);
        }
        private void StartGame()
        {
            view.ChangeTurn(currentSide);
        }
        public void EndTurn(int ind)
        {
            if (currentSide == Side.red)
            {
                MakeMove(ind, player);
                //board[ind] = player;
            }
            moveCount++;
            if (gridSpaces[0].side == currentSide && gridSpaces[1].side == currentSide && gridSpaces[2].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[3].side == currentSide && gridSpaces[4].side == currentSide && gridSpaces[5].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[6].side == currentSide && gridSpaces[7].side == currentSide && gridSpaces[8].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[0].side == currentSide && gridSpaces[3].side == currentSide && gridSpaces[6].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[1].side == currentSide && gridSpaces[4].side == currentSide && gridSpaces[7].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[2].side == currentSide && gridSpaces[5].side == currentSide && gridSpaces[8].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[0].side == currentSide && gridSpaces[4].side == currentSide && gridSpaces[8].side == currentSide)
            {
                GameOver();
            }
            if (gridSpaces[2].side == currentSide && gridSpaces[4].side == currentSide && gridSpaces[6].side == currentSide)
            {
                GameOver();
            }

            if (moveCount >= 9)
            {
                StartCoroutine(ResetGame());
            }

            ChangeTurn();
        }
        private IEnumerator ResetGame()
        {
            view.Block(true);
            moveCount = 0;
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i < board.Length; i++)
            {
                board[i] = '-';
            }
            foreach(var i in gridSpaces)
            {
                i.Reset();
            }
            if(firstSide == Side.red)
            {
                firstSide = Side.blue;
            }
            else
            {
                firstSide = Side.red;
            }
            view.Reset();
        }
        private void ChangeTurn()
        {
            currentSide = currentSide == Side.red ? Side.blue : Side.red;
            if(mode != GameMode.Pvp)
            {
                if(currentSide == Side.red)
                {
                    view.Block(false);
                }
                else
                {
                    view.Block(true);
                }
                if (currentSide == Side.blue)
                {
                    StartBotTurn();
                }
            }

            if (isWin == false)
            {
                view.ChangeTurn(currentSide);
            }
        }
        private void GameOver()
        {
            isWin = true;
            StartCoroutine(DelayForWin(currentSide));
            foreach (var space in gridSpaces)
            {
                space.DisableBtn();
            }
        }
        private IEnumerator DelayForWin(Side side)
        {
            yield return new WaitForSeconds(1);
            view.OnWin(side);
        }
        private void StartBotTurn()
        {
            if (isWin == true)
            {
                return;
            }
            switch (mode)
            {
                case GameMode.BotEasy:
                    StartCoroutine(BotGoHard());
                    break;
                case GameMode.BotNormal:
                    StartCoroutine(BotGoHard());
                    break;
                case GameMode.BotHard:
                    StartCoroutine(BotGoHard());
                    break;
                case GameMode.Pvp:
                    break;
            }
        }
        private IEnumerator BotGoHard()
        {
            yield return new WaitForSeconds(1.5f);
            int move = GetBestMove();
            //board[move] = 1;
            MakeMove(move, computer);
            gridSpaces[move].SetSpace();
        }
        public bool IsGameOver()
        {
            if (GetWinner() != '-')
            {
                return true;
            }
            if (IsBoardFull())
            {
                return true;
            }
            return false;
        }
        private int Minimax(int depth, bool isMaximizingPlayer)
        {
            int maxDepth = 0;
            switch (mode)
            {
                case GameMode.BotEasy:
                    maxDepth = 1;
                    break;
                case GameMode.BotNormal:
                    maxDepth = 2;
                    break;
                case GameMode.BotHard:
                    maxDepth = 9;
                    break;
            }
            // Check if the game is over or the depth limit has been reached
            if (IsGameOver() || depth == maxDepth)
            {
                return EvaluateBoard();
            }

            // Initialize the best score based on whether we're maximizing or minimizing
            int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

            // Iterate over all possible moves
            for (int i = 0; i < board.Length; i++)
            {
                // Check if the cell is empty
                if (board[i] == '-')
                {
                    // Make the move
                    board[i] = isMaximizingPlayer ? computer : player;

                    // Recursively call Minimax to evaluate the board after the move
                    int score = Minimax(depth + 1, !isMaximizingPlayer);

                    // Undo the move
                    board[i] = '-';

                    // Update the best score based on whether we're maximizing or minimizing
                    if (isMaximizingPlayer)
                    {
                        bestScore = Math.Max(bestScore, score);
                    }
                    else
                    {
                        bestScore = Math.Min(bestScore, score);
                    }
                }
            }

            return bestScore;
        }
        private int EvaluateBoard()
        {
            // Evaluate the board based on the number of possible winning lines for each player
            int computerLines = CountLines(computer);
            int humanLines = CountLines(player);
            return computerLines - humanLines;
        }
        private int CountLines(char player)
        {
            int count = 0;

            // Check rows
            for (int i = 0; i < 9; i += 3)
            {
                if (board[i] == player && board[i + 1] == player && board[i + 2] == player)
                {
                    count++;
                }
            }

            // Check columns
            for (int i = 0; i < 3; i++)
            {
                if (board[i] == player && board[i + 3] == player && board[i + 6] == player)
                {
                    count++;
                }
            }

            // Check diagonals
            if (board[0] == player && board[4] == player && board[8] == player)
            {
                count++;
            }

            if (board[2] == player && board[4] == player && board[6] == player)
            {
                count++;
            }

            return count;
        }
        public bool IsBoardFull()
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == '-')
                {
                    return false;
                }
            }
            return true;
        }
        public bool MakeMove(int index, char player)
        {
            if (board[index] == '-')
            {
                board[index] = player;
                return true;
            }
            return false;
        }
        public char GetWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                int row = i * 3;
                if (board[row] != '-' && board[row] == board[row + 1] && board[row] == board[row + 2])
                {
                    return board[row];
                }
                if (board[i] != '-' && board[i] == board[i + 3] && board[i] == board[i + 6])
                {
                    return board[i];
                }
            }
            if (board[0] != '-' && board[0] == board[4] && board[0] == board[8])
            {
                return board[0];
            }
            if (board[2] != '-' && board[2] == board[4] && board[2] == board[6])
            {
                return board[2];
            }
            return '-';
        }
        public int GetBestMove()
        {
            int bestMove = -1;
            int bestScore = int.MinValue;

            for (int i = 0; i < board.Length; i++)
            {
                // Check if the cell is empty
                if (board[i] == '-')
                {
                    // Make the move
                    board[i] = computer;

                    // Evaluate the board after the move
                    int score = Minimax(0, false);

                    // Undo the move
                    board[i] = '-';

                    // Update the best score and move
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }

            return bestMove;
        }

    }
}