using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] WinDetector winDetector;
        [SerializeField] Bell bell;
        [SerializeField] [Range(1, 2)] int player = 1;
        [SerializeField] InputReader opponent;
        [SerializeField] bool _hasAttacked = false;
        [SerializeField] bool _hasParried = false;

        [SerializeField] private bool _clashDetected = false;

        public string FighterName;

        public bool HasAttacked => _hasAttacked;
        public bool HasParried => _hasParried;

        public void ParryEnd()  { _hasParried = false; }
        public void SetParry()  { _hasParried = true; }
        public void SetAttack() { _hasAttacked = true; }

        AttackDrawer attack;
        GameInputs gameInputs;

        Dictionary<InternalInputs, KeyCode> internalKeys = new Dictionary<InternalInputs, KeyCode>();

        private enum InputState
        {
            PreBell,
            PostBell,
            PostInput
        };

        private enum InternalInputs
        {
            Attack,
            Feint,
            Parry
        };

        [SerializeField] InputState _currentState;

        private void Start()
        {
            gameInputs = GameInputs.singleton;
            attack = GetComponent<AttackDrawer>();

            _currentState = InputState.PreBell;

            bell.OnBell += SetPostBell;

            AssignInternalInputs(player);
        }

        public void ClashResetAttack()
        {
            _hasAttacked = false;
            _currentState = InputState.PostBell;
            _clashDetected = false;
        }

        private void AssignInternalInputs(int player)
        {
            if (player == 1)
            {
                internalKeys.Add(InternalInputs.Attack, gameInputs.keys[GameInputs.Keys.P1Attack]);
                internalKeys.Add(InternalInputs.Feint, gameInputs.keys[GameInputs.Keys.P1Feint]);
                internalKeys.Add(InternalInputs.Parry, gameInputs.keys[GameInputs.Keys.P1Parry]);
            }

            if (player == 2)
            {
                internalKeys.Add(InternalInputs.Attack, gameInputs.keys[GameInputs.Keys.P2Attack]);
                internalKeys.Add(InternalInputs.Feint, gameInputs.keys[GameInputs.Keys.P2Feint]);
                internalKeys.Add(InternalInputs.Parry, gameInputs.keys[GameInputs.Keys.P2Parry]);
            }
        }

        private void SetPostBell(object sender, EventArgs e)
        {
            _currentState = InputState.PostBell;
        }

        private void SetPostInput()
        {
            _currentState = InputState.PostInput;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case InputState.PreBell:
                    PreBellInputs();
                    break;
                case InputState.PostBell:
                    PostBellInputs();
                    break;
                default:
                case InputState.PostInput:
                    break;
            }
        }

        private void PreBellInputs()
        {
            if (Input.GetKeyDown(internalKeys[InternalInputs.Feint]))
            {
                attack.DoFeint();
            }

            if (Input.GetKeyDown(internalKeys[InternalInputs.Parry]))
            {
                attack.DoFeint();
            }

            if (Input.GetKeyDown(internalKeys[InternalInputs.Attack]))
            {
                print("FAULT!");
                bell.DisplayBell();
                attack.EventReset();
            }
        }

        private void PostBellInputs()
        {
            if (attack.GetInputFreeze) return;

            if (Input.GetKeyDown(internalKeys[InternalInputs.Parry]))
            {
                attack.DoParry();
            }

            if (Input.GetKeyDown(internalKeys[InternalInputs.Attack]))
            {
                if (attack.GetAttackState != AttackDrawer.AttackState.Neutral) return;

                attack.DoAttack();

                if (CheckClash())
                {
                    print("Clash!");
                    _clashDetected = true;
                    return;
                }

                SetPostInput();

                if (opponent.HasParried)
                {
                    winDetector.SetWinningPlayer(opponent);
                    attack.EventReset();
                    opponent.SetPostInput();
                }
            }
        }

        public void AttackActive()
        {
            SetAttack();

            if (opponent.HasAttacked) return;

            if (opponent.RequestWinConditionMet())
            {
                winDetector.SetWinningPlayer(this);
                opponent.SetPostInput();
            }     
        }

        public bool RequestWinConditionMet()
        {
            if (_clashDetected)
            {
                ClashResetAttack();
                attack.EventReset();
                opponent.ClashResetAttack();
                opponent.GetComponent<AttackDrawer>().EventReset();
                return false;
            }

            return true;
        }

        private bool CheckClash() => opponent.GetComponent<AttackDrawer>().GetAttackState == AttackDrawer.AttackState.AttackStartUp;
    }
}