/*************************************************************************
 *  Copyright © 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  PathHelperEditor.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/21
 *  Description  :  Initial development version.
 *************************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace MGS.QuickAssistant.Editor
{
    public sealed class PathAssistantEditor
    {
        [MenuItem("Tools/Path Assistant/DataPath")]
        public static void DataPath()
        {
            Application.OpenURL(Application.dataPath);
        }

        [MenuItem("Tools/Path Assistant/StreamingAssetsPath")]
        public static void StreamingAssetsPath()
        {
            Application.OpenURL(Application.streamingAssetsPath);
        }

        [MenuItem("Tools/Path Assistant/PersistentDataPath")]
        public static void PersistentDataPath()
        {
            Application.OpenURL(Application.persistentDataPath);
        }

        [MenuItem("Tools/Path Assistant/TemporaryCachePath")]
        public static void TemporaryCachePath()
        {
            Application.OpenURL(Application.temporaryCachePath);
        }

        [MenuItem("Tools/Path Assistant/ConsoleLogPath")]
        public static void ConsoleLogPath()
        {
            var dir = Path.GetDirectoryName(Application.consoleLogPath);
            Application.OpenURL(dir);
        }
    }
}