/*************************************************************************
 *  Copyright © 2026 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Finder.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  06/17/2026
 *  Description  :  Initial development version.
 *************************************************************************/

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MGS.Finder.Editor
{
    public class Finder : EditorWindow
    {
        [MenuItem("Tools/Assistant/Finder &F")]
        public static void Open()
        {
            GetWindow<Finder>().Show();
        }

        void OnGUI()
        {
            DrawPathGUI();
            DrawPlayGUI();
            DrawSceneGUI();
        }

        void DrawPlayGUI()
        {
            GUILayout.BeginHorizontal();
            var color = GUI.color;
            GUI.color = EditorApplication.isPlaying ? highlightColor : color;
            if (GUILayout.Button("Play"))
            {
                EditorApplication.isPlaying = !EditorApplication.isPlaying;
            }

            GUI.color = EditorApplication.isPaused ? highlightColor : color;
            if (GUILayout.Button("Pause"))
            {
                EditorApplication.isPaused = !EditorApplication.isPaused;
            }

            GUI.color = color;
            if (GUILayout.Button("Step"))
            {
                EditorApplication.Step();
            }
            GUILayout.EndHorizontal();
        }
        Color highlightColor = Color.cyan;

        void DrawPathGUI()
        {
            if (GUILayout.Button("dataPath"))
            {
                Application.OpenURL(Application.dataPath);
            }
            if (GUILayout.Button("streamingAssetsPath"))
            {
                Application.OpenURL(Application.streamingAssetsPath);
            }
            if (GUILayout.Button("persistentDataPath"))
            {
                Application.OpenURL(Application.persistentDataPath);
            }
            if (GUILayout.Button("temporaryCachePath"))
            {
                Application.OpenURL(Application.temporaryCachePath);
            }
            if (GUILayout.Button("consoleLogPath"))
            {
                Application.OpenURL(Path.GetDirectoryName(Application.consoleLogPath));
            }
        }

        void DrawSceneGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) { continue; }

                if (GUILayout.Button(scene.path))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        Vector2 scrollPosition;
    }
}