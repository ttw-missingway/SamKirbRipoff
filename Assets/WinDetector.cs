using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class WinDetector : MonoBehaviour
    {
        [SerializeField] Bell bell;
        [SerializeField] GameClock gameClock;

        [SerializeField] InputReader winner;

        public void SetWinningPlayer(InputReader inputReader)
        {
            winner = inputReader;
        }
    }
}