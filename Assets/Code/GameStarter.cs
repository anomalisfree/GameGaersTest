using UnityEngine;

namespace Code
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerController[] players;    
        
        private void Awake()
        {
            var settings = new GameSettingsLoader().Settings;
            
            cameraController.Initialize(settings.cameraSettings);
            
            var gameController = new GameController();
            gameController.Initialize(settings, players);
        }
    }
}
