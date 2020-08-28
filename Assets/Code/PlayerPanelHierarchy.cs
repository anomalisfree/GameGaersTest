using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class PlayerPanelHierarchy : MonoBehaviour
    {
        public Button attackButton;
        public Transform statsPanel;
        public Animator character;
        public GameObject statPrefab;
        public Action onAttack;
        public GameObject healthBarPrefab;
        public Transform healthBarRoot;

        private void Start()
        {
            attackButton.onClick.AddListener(() => onAttack?.Invoke());
        }
    }
}
