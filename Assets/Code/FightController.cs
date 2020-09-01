using System;
using UnityEngine;

namespace Code
{
    public class FightController
    {
        public Action<float> onAttack;
        public Action<StatsId, float> needUpdateStatUi;

        private float _life;
        private float _armor;
        private float _damage;
        private float _lifeSteal;

        private float _maxLife;

        private readonly PlayerPanelHierarchy _playerPanelHierarchy;
        private readonly HealthBar _healthBar;

        private static readonly int AttackAnimationTrigger = Animator.StringToHash("Attack");
        private static readonly int Health = Animator.StringToHash("Health");

        public FightController(PlayerPanelHierarchy playerPanelHierarchy, HealthBar healthBar)
        {
            _playerPanelHierarchy = playerPanelHierarchy;
            _healthBar = healthBar;
            playerPanelHierarchy.attackButton.onClick.AddListener(Attack);
        }

        public float GetParam(StatsId id)
        {
            switch (id)
            {
                case StatsId.LIFE_ID:
                    return _life;
                case StatsId.ARMOR_ID:
                    return _armor;
                case StatsId.DAMAGE_ID:
                    return _damage;
                case StatsId.LIFE_STEAL_ID:
                    return _lifeSteal;
                default:
                    Debug.LogError("There is no such parameter.");
                    return 0;
            }
        }

        public void SetParam(StatsId id, float value)
        {
            switch (id)
            {
                case StatsId.LIFE_ID:
                    _life = value;
                    break;
                case StatsId.ARMOR_ID:
                    _armor = value;
                    break;
                case StatsId.DAMAGE_ID:
                    _damage = value;
                    break;
                case StatsId.LIFE_STEAL_ID:
                    _lifeSteal = value;
                    break;
                default:
                    Debug.LogError("There is no such parameter.");
                    break;
            }

            needUpdateStatUi?.Invoke(id, value);
        }

        private void Attack()
        {
            if (_life > 0)
            {
                _playerPanelHierarchy.character.SetTrigger(AttackAnimationTrigger);
                onAttack?.Invoke(_damage);
            }
        }

        public void GetHit(float damage, out float finishDamage)
        {
            if (_life > 0)
            {
                finishDamage = damage - damage * _armor * 0.01f;
                _life -= finishDamage;
            }
            else
            {
                finishDamage = 0;
            }

            if (_life < 0)
                _life = 0;

            SetParam(StatsId.LIFE_ID, _life);
            SetHealthVisual();
        }

        public void DamageDoneToEnemy(float damage)
        {
            if (_lifeSteal > 0)
            {
                _life += damage * _lifeSteal * 0.01f;

                if (_life > _maxLife)
                    _life = _maxLife;

                SetParam(StatsId.LIFE_ID, _life);
                SetHealthVisual();
            }
        }

        public void ResetMaxLife()
        {
            _maxLife = _life;
            SetHealthVisual(true);
        }

        private void SetHealthVisual(bool isStarting = false)
        {
            _playerPanelHierarchy.character.SetInteger(Health, (int) _life);

            if (isStarting)
                _healthBar.Initialize(_life);
            else
                _healthBar.SetAmount(_life, _maxLife);
        }

        public void SetHealthPosition(Vector3 position)
        {
            _healthBar.SetPose(position);
        }
    }
}