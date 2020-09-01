﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class GameController
    {
        private Data _defaultSettings;
        private PlayerController[] _players;
        private MainUI _mainUi;

        public void Initialize(
            Data settings,
            PlayerController[] players,
            MainUI mainUi
        )
        {
            _defaultSettings = settings;
            _players = players;
            _mainUi = mainUi;

            SetDefaultParams();

            _mainUi.playWithoutBuffsButton.onClick.AddListener(PlayWithoutBuffs);
            _mainUi.playWithBuffsButton.onClick.AddListener(PlayWithBuffs);

            for (var i = 0; i < _players.Length; i++)
            {
                var num = i;
                _players[i].onAttack += damage => PlayerAttack(damage, num);
            }
        }

        private void PlayWithoutBuffs()
        {
            SetDefaultParams();
        }

        private void PlayWithBuffs()
        {
            SetDefaultParams();
            AddBuffs();
        }

        private void SetDefaultParams()
        {
            _defaultSettings = GameSettingsLoader.GetSettings();

            foreach (var player in _players)
            {
                player.SetPlayerStats(_defaultSettings.stats);
            }
        }

        private void AddBuffs()
        {
            foreach (var player in _players)
            {
                if (_defaultSettings.settings.allowDuplicateBuffs)
                {
                    for (var i = 0;
                        i < Random.Range(_defaultSettings.settings.buffCountMin,
                            _defaultSettings.settings.buffCountMax + 1);
                        i++)
                    {
                        player.AddBuffs(_defaultSettings.buffs[Random.Range(0, _defaultSettings.buffs.Length)]);
                    }
                }
                else
                {
                    var selectedBuffs = new List<Buff>();

                    for (var i = 0;
                        i < Random.Range(_defaultSettings.settings.buffCountMin,
                            _defaultSettings.settings.buffCountMax + 1);
                        i++)
                    {
                        var currentBuff = _defaultSettings.buffs[Random.Range(0, _defaultSettings.buffs.Length)];
                        var hasDuplicate = false;

                        foreach (var _ in selectedBuffs.Where(selectedBuff => selectedBuff == currentBuff))
                        {
                            hasDuplicate = true;
                        }

                        if (hasDuplicate)
                        {
                            i--;
                        }
                        else
                        {
                            player.AddBuffs(currentBuff);
                            selectedBuffs.Add(currentBuff);
                        }
                    }
                }
            }
        }

        private void PlayerAttack(float damage, int playerNum)
        {
            var damageSum = 0f;

            for (var i = 0; i < _players.Length; i++)
            {
                if (i - playerNum != 0)
                {
                    _players[i].GetHit(damage, out var finishDamage);
                    damageSum += finishDamage;
                }
            }

            _players[playerNum].DamageDoneToEnemy(damageSum);
        }
    }
}