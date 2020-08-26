using UnityEngine;

namespace Code
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        
        private void Awake()
        {
            var settings = new GameSettingsLoader().Settings;
            
            cameraController.Initialize(settings.cameraSettings);
        }
    }
}
