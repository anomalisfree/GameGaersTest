using UnityEngine;

namespace Code
{
   public class GameSettingsLoader
   {
      private const string JsonFileName = "data";
      
      public Data Settings => _settings;
      private Data _settings;

      public GameSettingsLoader()
      {
         var jsonString = Resources.Load<TextAsset>(JsonFileName);
         _settings = JsonUtility.FromJson<Data>(jsonString.text);
      }
   }
}
