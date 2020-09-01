using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        public Action<float> onAttack;

        [SerializeField] private PlayerPanelHierarchy playerPanelHierarchy = null;
        [SerializeField] private Transform headPivot = null;

        private readonly List<StatUi> _playerStats = new List<StatUi>();
        private readonly List<StatUi> _playerBuffs = new List<StatUi>();

        private FightController _fightController;

        private void Update()
        {
            _fightController.SetHealthPosition(headPivot.position + Vector3.up);
        }

        private void ClearStatsUi()
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

        private void UpdateStatUi(StatsId id, float value)
        {
            foreach (var playerStat in _playerStats.Where(playerStat => playerStat.GetStat().id == (int) id))
            {
                playerStat.UpdateStatValue(value);
            }
        }

        public void SetPlayerStats(IEnumerable<Stat> stats)
        {
            if (_fightController == null)
            {
                var healthBar = Instantiate(playerPanelHierarchy.healthBarPrefab, playerPanelHierarchy.healthBarRoot)
                    .GetComponent<HealthBar>();

                _fightController = new FightController(playerPanelHierarchy, healthBar)
                    {needUpdateStatUi = UpdateStatUi};
                _fightController.onAttack += f => onAttack?.Invoke(f);
            }

            ClearStatsUi();

            foreach (var stat in stats)
            {
                var statUi = Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel)
                    .GetComponent<StatUi>();
                statUi.Initialize(stat);

                _fightController.SetParam((StatsId) stat.id, stat.value);
                _playerStats.Add(statUi);
            }

            _fightController.ResetMaxLife();
        }

        public void AddBuffs(Buff buff)
        {
            var buffsUi = Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel)
                .GetComponent<StatUi>();
            buffsUi.Initialize(buff);
            _playerBuffs.Add(buffsUi);

            foreach (var stat in buff.stats)
            {
                _fightController.SetParam((StatsId) stat.statId,
                    _fightController.GetParam((StatsId) stat.statId) + stat.value);
            }

            _fightController.ResetMaxLife();
        }

        public void DamageDoneToEnemy(float damage)
        {
            _fightController.DamageDoneToEnemy(damage);
        }

        public void GetHit(float damage, out float finishDamage)
        {
            _fightController.GetHit(damage, out finishDamage);
        }
    }
}