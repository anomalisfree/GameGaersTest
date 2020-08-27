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
                var buffCount = 0;
                
                foreach (var buff in _defaultSettings.buffs)
                {
                    if (buffCount < _defaultSettings.settings.buffCountMax && Random.Range(0, 2) == 0)
                    {
                        player.AddBuffs(buff);
                        buffCount++;
                    }
                }
            }
        }

        private void PlayerAttack(float damage, int playerNum)
        {
            for (var i = 0; i < _players.Length; i++)
            {
                if (i -  playerNum != 0)
                {
                    _players[i].GetHit(damage);
                }
            }
        }
        
    }
}
