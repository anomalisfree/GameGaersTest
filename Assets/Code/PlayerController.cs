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

        private readonly List<StatUI> _playerStats = new List<StatUI>();
        private readonly List<StatUI> _playerBuffs = new List<StatUI>();
        private static readonly int AttackAnimationTrigger = Animator.StringToHash("Attack");

        private void Start()
        {
            playerPanelHierarchy.onAttack += Attack;
        }

        public void SetPlayerStats(IEnumerable<Stat> stats)
        {
            ClearStatsUI();

            foreach (var stat in stats)
            {
                var statUI = Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel)
                    .GetComponent<StatUI>();
                statUI.Initialize(stat);
                _playerStats.Add(statUI);
            }
        }

        public void AddBuffs(Buff buff)
        {
            var buffsUI = Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel)
                .GetComponent<StatUI>();
            buffsUI.Initialize(buff);
            _playerBuffs.Add(buffsUI);

            foreach (var playerStat in _playerStats)
            {
                foreach (var stat in buff.stats)
                {
                    if (stat.statId == playerStat.GetStat().id)
                    {
                        playerStat.AddStatValue(stat.value);
                    }
                }
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

            foreach (var stat in _playerStats.Select(playerStat => playerStat.GetStat())
                .Where(stat => stat.id == (int) StatsId.DAMAGE_ID))
            {
                onAttack?.Invoke(stat.value);
            }
        }

        public void GetHit(float damage)
        {
            foreach (var playerStat in _playerStats.Where(
                playerStat => playerStat.GetStat().id == (int) StatsId.LIFE_ID))
            {
                playerStat.GetStat().value -= damage;
                playerStat.UpdateStatValue(playerStat.GetStat().value);
            }
        }
    }
}