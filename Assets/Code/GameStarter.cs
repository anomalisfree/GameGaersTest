using UnityEngine;

namespace Code
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController = null;
        [SerializeField] private PlayerController[] players = null;
        [SerializeField] private MainUI mainUi = null;
        
        private void Awake()
        {
            var settings = GameSettingsLoader.GetSettings();
            
            cameraController.Initialize(settings.cameraSettings);
            
            var gameController = new GameController();
            gameController.Initialize(settings, players, mainUi);
        }
    }
}
