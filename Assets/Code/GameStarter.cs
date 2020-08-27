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
            var settings = GameSettingsLoader.GetSettings();
            
            cameraController.Initialize(settings.cameraSettings);
            
            var gameController = new GameController();
            gameController.Initialize(settings, players, mainUi);
        }
    }
}
