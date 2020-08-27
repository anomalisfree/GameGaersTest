using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        public Action<float> onAttack;

        [SerializeField] private PlayerPanelHierarchy playerPanelHierarchy;

        private float _life;
        private float _armor;
        private float _damage;
        private float _lifeSteal;

        private readonly List<StatUI> _playerStats = new List<StatUI>();
        private readonly List<StatUI> _playerBuffs = new List<StatUI>();
        private static readonly int AttackAnimationTrigger = Animator.StringToHash("Attack");

        private void Start()
        {
            playerPanelHierarchy.onAttack += Attack;
        }

        private float GetParam(StatsId id)
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

        private void SetParam(StatsId id, float value)
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

            UpdateStatUI(id, value);
        }

        private void UpdateStatUI(StatsId id, float value)
        {
            foreach (var playerStat in _playerStats.Where(playerStat => playerStat.GetStat().id == (int) id))
            {
                playerStat.UpdateStatValue(value);
            }
        }

        public void SetPlayerStats(IEnumerable<Stat> stats)
        {
            ClearStatsUI();

            foreach (var stat in stats)
            {
                var statUI = Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel)
                    .GetComponent<StatUI>();
                statUI.Initialize(stat);

                SetParam((StatsId) stat.id, stat.value);
                
                _playerStats.Add(statUI);
            }
        }

        public void AddBuffs(Buff buff)
        {
            var buffsUI = Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel)
                .GetComponent<StatUI>();
            buffsUI.Initialize(buff);
            _playerBuffs.Add(buffsUI);

            foreach (var stat in buff.stats)
            {
                SetParam((StatsId)stat.statId, GetParam((StatsId)stat.statId) + stat.value);
            }
        }

        private void ClearStatsUI()
        {
            foreach (var playerStat in _playerStats)
            {
                Destroy(playerStat.gameObject);
            }

            _playerStats.Clear();

            foreach (var playerBuff in _playerBuffs)
            {
                Destroy(playerBuff.gameObject);
            }

            _playerBuffs.Clear();
        }

        private void Attack()
        {
            playerPanelHierarchy.character.SetTrigger(AttackAnimationTrigger);
            onAttack?.Invoke(_damage);
        }

        public void GetHit(float damage)
        {
            _life -= damage;
            SetParam(StatsId.LIFE_ID, _life);
        }
    }
}