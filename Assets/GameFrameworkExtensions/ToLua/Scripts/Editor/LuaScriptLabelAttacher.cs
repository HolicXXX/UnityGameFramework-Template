using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.ToLua
{
    public static class LuaScriptLabelAttacher
    {
        private const string LuaScriptExtension = ".lua";
        public const string LuaScriptLabel = "LuaScript";

        [MenuItem("Game Framework/Extensions/ToLua/Add LuaScript Label", false, 60)]
        public static void AddLuaScriptLabel()
        {
            string[] luaScriptLabels = new string[] { LuaScriptLabel };
            var luaScriptAssetNames = AssetDatabase.GetAllAssetPaths().Where(luaScriptAssetName => luaScriptAssetName.EndsWith(LuaScriptExtension));
            foreach (string luaScriptAssetName in luaScriptAssetNames)
            {
                Object asset = AssetDatabase.LoadAssetAtPath(luaScriptAssetName, typeof(Object));
                AssetDatabase.SetLabels(asset, new HashSet<string>(AssetDatabase.GetLabels(asset)).Union(luaScriptLabels).ToArray());
            }
        }

        [MenuItem("Game Framework/Extensions/ToLua/Remove LuaScript Label", false, 61)]
        public static void RemoveLuaScriptLabel()
        {
            string[] luaScriptLabels = new string[] { LuaScriptLabel };
            var luaScriptAssetNames = AssetDatabase.GetAllAssetPaths().Where(luaScriptAssetName => luaScriptAssetName.EndsWith(LuaScriptExtension));
            foreach (string luaScriptAssetName in luaScriptAssetNames)
            {
                Object asset = AssetDatabase.LoadAssetAtPath(luaScriptAssetName, typeof(Object));
                AssetDatabase.SetLabels(asset, new HashSet<string>(AssetDatabase.GetLabels(asset)).Except(luaScriptLabels).ToArray());
            }
        }
    }
}
