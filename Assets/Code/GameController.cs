namespace Code
{
    public class GameController
    {
        private Data _defaultSettings;
        private PlayerController[] _players;
        
        public void Initialize(Data settings, PlayerController[] players)
        {
            _defaultSettings = settings;
            _players = players;

            SetDefaultParams();
        }

        private void SetDefaultParams()
        {
            foreach (var player in _players)
            {
                player.SetPlayerStats(_defaultSettings.stats);
            }
        }
        
    }
}
