using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class GameInputs : MonoBehaviour
    {
        public enum Keys
        {
            P1Parry,
            P1Feint,
            P1Attack,
            P2Parry,
            P2Feint,
            P2Attack
        }

        public Dictionary<Keys, KeyCode> keys = new Dictionary<Keys, KeyCode>();

        public static GameInputs singleton;

        private void Awake()
        {
            SingletonCheck();

            keys.Add(Keys.P1Attack, KeyCode.D);
            keys.Add(Keys.P1Feint, KeyCode.S);
            keys.Add(Keys.P1Parry, KeyCode.A);

            keys.Add(Keys.P2Attack, KeyCode.LeftArrow);
            keys.Add(Keys.P2Feint, KeyCode.DownArrow);
            keys.Add(Keys.P2Parry, KeyCode.RightArrow);
        }

        private void SingletonCheck()
        {
            if (singleton == null)
            {
                singleton = this;
            }
            else
            {
                print("ERROR, more than one GameInputs class detected!");
            }
        }
    }
}