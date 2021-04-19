using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class Bell : MonoBehaviour
    {
        GameClock gameClock;
        Image bellDisplay;

        [SerializeField] [Range(0f, 10f)] float _bellTimerFloor = 0f;
        [SerializeField] [Range(0f, 10f)] float _bellTimerCeiling = 0f;
        [SerializeField] float _bellTimerActual;
        public float GetBellTimer => _bellTimerActual;
        public event EventHandler OnBell;

        private void Start()
        {
            bellDisplay = GetComponentInChildren<Image>();
            gameClock = GetComponent<GameClock>();
            RandomizeBellTimer();
        }

        private void Update()
        {
            if (!bellDisplay.enabled)
            {
                if (gameClock.GetTime > _bellTimerActual) DisplayBell();
            }
        }

        private void RandomizeBellTimer()
        {
            _bellTimerActual = UnityEngine.Random.Range(_bellTimerFloor, _bellTimerCeiling);
        }

        private void DisplayBell()
        {
            bellDisplay.enabled = true;
            OnBell?.Invoke(this, EventArgs.Empty);
        }
    }
}
