using UnityEngine;

namespace Code
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerController[] players;
        [SerializeField] private MainUI mainUi;

        
        private void Awake()
        {
            cameraController.Initialize();
            
            var gameController = new GameController();
            gameController.Initialize(players, mainUi);
        }
    }
}
