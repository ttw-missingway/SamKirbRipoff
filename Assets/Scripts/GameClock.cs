using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Combat
{
    public class GameClock : MonoBehaviour
    {
        TMP_Text textMesh;
        public static GameClock singleton;

        float _internalTime;
        public float GetTime => _internalTime;
        

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
            else
            {
                print("ERROR: more than one singleton in game scene!");
            }

            textMesh = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _internalTime += Time.deltaTime;
            DisplayTime();
        }

        private void DisplayTime()
        {
            textMesh.text = ((Mathf.Round(_internalTime * 100)) / 100.0).ToString();
        }
    }
}
