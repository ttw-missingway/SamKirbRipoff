using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Combat
{
    public class WinDetector : MonoBehaviour
    {
        [SerializeField] Bell bell;
        [SerializeField] GameClock gameClock;

        [SerializeField] InputReader winner;
        [SerializeField] TMP_Text winnerText;

        public void SetWinningPlayer(InputReader inputReader)
        {
            winner = inputReader;
            winnerText.enabled = true;
            string[] winnerTextArray = new string[2];
            winnerTextArray[0] = "Winner!";
            winnerTextArray[1] = winner.FighterName;

            winnerText.text = string.Concat(winnerTextArray);
        }
    }
}