using GameFramework;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class LocalizationExtension
    {
        public static void LoadDictionary(this LocalizationComponent localizationComponent, string dictionaryName, object userData = null)
        {
            if (string.IsNullOrEmpty(dictionaryName))
            {
                Log.Warning("Dictionary name is invalid.");
                return;
            }

            localizationComponent.LoadDictionary(dictionaryName, AssetUtility.GetDictionaryAsset(dictionaryName), userData);
        }
    }
}
