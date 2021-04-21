using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class AttackDrawer : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        InputReader inputReader;
        [SerializeField] AttackState _attackState;
        [SerializeField] Bell bell;
        [SerializeField] bool _inputFreeze = false;
        public bool GetInputFreeze => _inputFreeze;
        [SerializeField] [Range(0f, 1f)] float _attackStartUpTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _attackActiveTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _attackRecoverTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _feintStartUpTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _feintActiveTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _feintRecoverTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _parryStartUpTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _parryActiveTime = 1f;
        [SerializeField] [Range(0f, 1f)] float _parryRecoverTime = 1f;

        private enum AttackType
        {
            Attack,
            Parry,
            Feint
        }

        public enum AttackState
        {
            Neutral,
            AttackStartUp,
            Attacking,
            Feinting,
            ParryStartUp,
            Parrying,
            Recovery
        }

        public AttackState GetAttackState => _attackState;

        private void Start()
        {
            _attackState = AttackState.Neutral;
            bell.OnBell += ForceEndAnimations;
            spriteRenderer = GetComponent<SpriteRenderer>();
            inputReader = GetComponent<InputReader>();
        }

        private void ForceEndAnimations(object sender, EventArgs e)
        {
            StopAllCoroutines();
            ResetAttackState();
        }

        private void StartAttackFrames(Ability ability)
        {
            IEnumerator coroutine = StartUpFrames(ability);
            StartCoroutine(coroutine);
        }

        private IEnumerator StartUpFrames(Ability ability)
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(ability.GetStartUpFrames);
            PerformAbility(ability);
            StartCoroutine(ActiveFrames(ability));
        }

        private IEnumerator ActiveFrames(Ability ability)
        {
            spriteRenderer.color = ability.GetColor;
            yield return new WaitForSeconds(ability.GetActiveFrames);
            EndParry();
            StartCoroutine(RecoveryFrames(ability));
        }

        private IEnumerator RecoveryFrames(Ability ability)
        {
            _attackState = AttackState.Recovery;
            spriteRenderer.color = Color.cyan;
            yield return new WaitForSeconds(ability.GetRecoveryFrames);
            ResetAttackState();
        }

        private void PerformAbility(Ability ability)
        {
            if (ability.GetAttackType == AttackType.Attack)
            {
                inputReader.AttackActive();
                _attackState = AttackState.Attacking;
            }

            if (ability.GetAttackType == AttackType.Parry)
            {
                inputReader.SetParry();
                _attackState = AttackState.Parrying;
            }
        }

        public void EventReset()
        {
            StopAllCoroutines();
            Ability attack = new Ability(AttackType.Attack, _attackStartUpTime, _attackActiveTime, _attackRecoverTime, Color.red);
            StartCoroutine(RecoveryFrames(attack));
        }

        public void DoAttack()
        {
            _inputFreeze = true;
            Ability attack = new Ability(AttackType.Attack, _attackStartUpTime, _attackActiveTime, _attackRecoverTime, Color.red);
            StartAttackFrames(attack);
            _attackState = AttackState.AttackStartUp;
        }

        public void DoParry()
        {
            _inputFreeze = true;
            Ability parry = new Ability(AttackType.Parry, _parryStartUpTime, _parryActiveTime, _parryRecoverTime, Color.green);
            StartAttackFrames(parry);
            _attackState = AttackState.ParryStartUp;
        }

        public void DoFeint()
        {
            _inputFreeze = true;
            Ability feint = new Ability(AttackType.Feint, _feintStartUpTime, _feintActiveTime, _feintRecoverTime, Color.magenta);
            StartAttackFrames(feint);
            _attackState = AttackState.Feinting;
        }

        private void ResetAttackState()
        {
            _inputFreeze = false;
            spriteRenderer.color = Color.white;
            _attackState = AttackState.Neutral;
        }

        private void EndParry()
        {
            inputReader.ParryEnd();
        }

        private class Ability
        {
            AttackType _type;
            float _startupFrames;
            float _activeFrames;
            float _recoveryFrames;
            Color _color;

            public Ability(AttackType type, float startupFrames, float activeFrames, float recoveryFrames, Color color)
            {
                _type = type;
                _startupFrames = startupFrames;
                _activeFrames = activeFrames;
                _recoveryFrames = recoveryFrames;
                _color = color;
            }

            public AttackType GetAttackType => _type;
            public float GetStartUpFrames => _startupFrames;
            public float GetActiveFrames => _activeFrames;
            public float GetRecoveryFrames => _recoveryFrames;
            public Color GetColor => _color;
        }
    }
}