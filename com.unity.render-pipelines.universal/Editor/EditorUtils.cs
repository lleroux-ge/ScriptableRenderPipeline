using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal.Internal
{
    /// <summary>
    /// Contains a database of built-in resource GUIds. These are used to load built-in resource files.
    /// </summary>
    public static class ResourceGuid
    {
        /// <summary>
        /// GUId for the <c>ScriptableRendererFeature</c> template file.
        /// </summary>
        public static readonly string rendererTemplate = "51493ed8d97d3c24b94c6cffe834630b";
    }
}

namespace UnityEditor.Rendering.Universal
{
    static class EditorUtils
    {
        // Each group is separate in the menu by a menu bar
        public const int lwrpAssetCreateMenuPriorityGroup1 = CoreUtils.assetCreateMenuPriority1;
        public const int lwrpAssetCreateMenuPriorityGroup2 = CoreUtils.assetCreateMenuPriority1 + 50;
        public const int lwrpAssetCreateMenuPriorityGroup3 = lwrpAssetCreateMenuPriorityGroup2 + 50;

        private const string SettingsFilePath = "/ProjectSettings/UniversalRP_Settings.txt";

        internal class Styles
        {
            //Measurements
            public static float defaultLineSpace = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            public static float defaultIndentWidth = 12;
        }

        public static void DrawCascadeSplitGUI<T>(ref SerializedProperty shadowCascadeSplit)
        {
            float[] cascadePartitionSizes = null;
            Type type = typeof(T);
            if (type == typeof(float))
            {
                cascadePartitionSizes = new float[] { shadowCascadeSplit.floatValue };
            }
            else if (type == typeof(Vector3))
            {
                Vector3 splits = shadowCascadeSplit.vector3Value;
                cascadePartitionSizes = new float[]
                {
                    Mathf.Clamp(splits[0], 0.0f, 1.0f),
                    Mathf.Clamp(splits[1] - splits[0], 0.0f, 1.0f),
                    Mathf.Clamp(splits[2] - splits[1], 0.0f, 1.0f)
                };
            }
            if (cascadePartitionSizes != null)
            {
                EditorGUI.BeginChangeCheck();
                ShadowCascadeSplitGUI.HandleCascadeSliderGUI(ref cascadePartitionSizes);
                if (EditorGUI.EndChangeCheck())
                {
                    if (type == typeof(float))
                        shadowCascadeSplit.floatValue = cascadePartitionSizes[0];
                    else
                    {
                        Vector3 updatedValue = new Vector3();
                        updatedValue[0] = cascadePartitionSizes[0];
                        updatedValue[1] = updatedValue[0] + cascadePartitionSizes[1];
                        updatedValue[2] = updatedValue[1] + cascadePartitionSizes[2];
                        shadowCascadeSplit.vector3Value = updatedValue;
                    }
                }
            }
        }

        internal static void SetProjectValue(string key, string value)
        {
            var dict = ReadSettings();

            if (dict != null && dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
            WriteSettings(dict);
        }

        internal static string GetProjectValue(string key)
        {
            var dict = ReadSettings();
            return (dict != null && dict.ContainsKey(key)) ? dict[key] : "null";
        }

        private static SerializedDictionary<string, string> ReadSettings()
        {
            if (!File.Exists(SettingsPath()))
            {
                File.WriteAllText(SettingsPath(), "");
            }
            var rawData = File.ReadAllText(SettingsPath());
            if(rawData.Length == 0)
                return new SerializedDictionary<string, string>();
            return JsonUtility.FromJson(rawData, typeof(SerializedDictionary<string, string>)) as
                SerializedDictionary<string, string>;
        }

        private static void WriteSettings(SerializedDictionary<string, string> data)
        {
            var outputData = JsonUtility.ToJson(data);
            File.WriteAllText(SettingsPath(), outputData);
        }

        private static string SettingsPath()
        {
            return Application.dataPath.Remove(Application.dataPath.Length - 6) + SettingsFilePath;
        }
    }
}
