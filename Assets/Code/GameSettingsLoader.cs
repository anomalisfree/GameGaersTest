using UnityEngine;

namespace Code
{
   internal static class GameSettingsLoader
   {
      private const string JsonFileName = "data";

      public static Data GetSettings()
      {
         var jsonString = Resources.Load<TextAsset>(JsonFileName);
         return JsonUtility.FromJson<Data>(jsonString.text);
      }
   }
}
