using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerPanelHierarchy playerPanelHierarchy;
        
        private readonly float[] _playerStats = new float[4];

        public void SetPlayerStats(IEnumerable<Stat> stats)
        {
            foreach (var stat in stats)
            {
                _playerStats[stat.id] = stat.value;
                Instantiate(playerPanelHierarchy.statPrefab, playerPanelHierarchy.statsPanel);
            }
        }
    }
}
