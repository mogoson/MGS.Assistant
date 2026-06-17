/*************************************************************************
 *  Copyright © 2021 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AssetFilterEditor.cs
 *  Description  :  Editor to check the name of assets under the target
 *                  directory, filter and display the assets those name
 *                  is mismatch the define specification.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  3/6/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace MGS.AssetFilter.Editor
{
    public class AssetFilterEditor : EditorWindow
    {
        #region Field and Property
        private Thread thread;
        private bool markRepaint;

        private const float LABEL_WIDTH = 65;
        private const float BUTTON_WIDTH = 75;
        private Vector2 scrollPos = Vector2.zero;

        private const string KEY_TARGET_DIRECTORY = "ASSET_FILTER_TARGET_DIRECTORY";
        private string targetDirectory = "Assets";

        private const string SETTINGS_PATH = "Assets/Editor/AssetFilterSettings.asset";
        private AssetFilterSettings patternSettings;

        private List<string> filterAssets = new();
        private int totalCount = 0, doneCount = 0;
        private int pageCount = 0, pageIndex = 0;
        private const int EACH_PAGE_COUNT = 100;
        #endregion

        #region Private Method
        [MenuItem("Tools/Assistant/Asset Filter")]
        public static void Open()
        {
            GetWindow<AssetFilterEditor>("Asset Filter").Show();
        }

        private void OnEnable()
        {
            targetDirectory = EditorPrefs.GetString(KEY_TARGET_DIRECTORY, targetDirectory);
            patternSettings = AssetDatabase.LoadAssetAtPath<AssetFilterSettings>(SETTINGS_PATH);
        }

        private void OnDisable()
        {
            EditorUtility.UnloadUnusedAssetsImmediate(true);
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(string.Empty, "Window");

            #region Select Directory
            GUILayout.BeginHorizontal();
            GUILayout.Label("Directory", GUILayout.Width(LABEL_WIDTH));
            targetDirectory = GUILayout.TextField(targetDirectory);
            if (GUILayout.Button("Browse", GUILayout.Width(BUTTON_WIDTH)))
            {
                SelectDirectory();
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Pattern Settings
            GUILayout.BeginHorizontal();
            GUILayout.Label("Settings", GUILayout.Width(LABEL_WIDTH));
            EditorGUI.BeginChangeCheck();
            patternSettings = EditorGUILayout.ObjectField(patternSettings, typeof(AssetFilterSettings), false) as AssetFilterSettings;
            if (EditorGUI.EndChangeCheck())
            {
                ClearEditorCache();
            }
            if (GUILayout.Button("New", GUILayout.Width(BUTTON_WIDTH)))
            {
                CreateNewSettings();
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Top Tool Bar
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Check", GUILayout.Width(BUTTON_WIDTH)))
            {
                CheckAssetsName();
            }
            if (GUILayout.Button("Clear", GUILayout.Width(BUTTON_WIDTH)))
            {
                ClearEditorCache();
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Mismatch Assets
            GUILayout.BeginHorizontal();
            GUILayout.Label("Naming mismatch assets", GUILayout.Width(160));
            GUILayout.Label(filterAssets.Count.ToString());
            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos, "Box");
            var startIndex = pageIndex * EACH_PAGE_COUNT;
            var limitCount = Math.Min(EACH_PAGE_COUNT, filterAssets.Count - startIndex);
            for (int i = startIndex; i < startIndex + limitCount; i++)
            {
                if (GUILayout.Button(filterAssets[i], "TextField"))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filterAssets[i]);
                }
            }
            GUILayout.EndScrollView();
            #endregion

            #region Bottom Tool Bar
            if (pageCount > 1)
            {
                GUILayout.BeginHorizontal();
                if (pageIndex > 0)
                {
                    if (GUILayout.Button("Previous", GUILayout.Width(BUTTON_WIDTH)))
                    {
                        pageIndex--;
                        scrollPos = Vector2.zero;
                    }
                }
                else
                {
                    GUILayout.Label(string.Empty, GUILayout.Width(BUTTON_WIDTH));
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label(pageIndex + 1 + " / " + pageCount);
                GUILayout.FlexibleSpace();

                if (pageIndex < pageCount - 1)
                {
                    if (GUILayout.Button("Next", GUILayout.Width(BUTTON_WIDTH)))
                    {
                        pageIndex++;
                        scrollPos = Vector2.zero;
                    }
                }
                else
                {
                    GUILayout.Label(string.Empty, GUILayout.Width(BUTTON_WIDTH));
                }
                GUILayout.EndHorizontal();
            }
            #endregion

            GUILayout.EndVertical();
        }

        private void Update()
        {
            if (doneCount < totalCount)
            {
                if (EditorUtility.DisplayCancelableProgressBar(
                    "Check Assets Name",
                    doneCount + " / " + totalCount + " of assets have been checked.",
                    (float)doneCount / totalCount))
                {
                    thread.Abort();
                    doneCount = totalCount;
                    pageCount = GetCurrentPageCount();
                }
            }
            else
            {
                if (totalCount > 0)
                {
                    EditorUtility.ClearProgressBar();
                    doneCount = totalCount = 0;
                }
            }

            if (markRepaint)
            {
                Repaint();
                markRepaint = false;
            }
        }

        private void SelectDirectory()
        {
            var selectDirectory = EditorUtility.OpenFolderPanel("Select Target Directory", targetDirectory, string.Empty);
            if (selectDirectory == string.Empty)
            {
                return;
            }

            ClearEditorCache();
            try
            {
                targetDirectory = selectDirectory.Substring(selectDirectory.IndexOf("Assets"));
                EditorPrefs.SetString(KEY_TARGET_DIRECTORY, targetDirectory);
            }
            catch
            {
                ShowNotification(new GUIContent("Invalid selection directory."));
            }
        }

        private void CreateNewSettings()
        {
            ClearEditorCache();
            RequireDirectory(SETTINGS_PATH);
            patternSettings = CreateInstance<AssetFilterSettings>();
            AssetDatabase.CreateAsset(patternSettings, SETTINGS_PATH);
            Selection.activeObject = patternSettings;
        }

        private void RequireDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private void CheckAssetsName()
        {
            ClearEditorCache();
            if (!Directory.Exists(targetDirectory))
            {
                ShowNotification(new GUIContent("The target directory is not exist."));
                return;
            }
            if (!patternSettings)
            {
                ShowNotification(new GUIContent("The pattern settings can not be null."));
                return;
            }
            try
            {
                thread = new Thread(() =>
                {
                    var searchFiles = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories);
                    totalCount = searchFiles.Length;
                    foreach (var file in searchFiles)
                    {
                        if (CheckMismatchPattern(file))
                        {
                            filterAssets.Add(file);
                        }
                        doneCount++;
                        Thread.Sleep(0);
                    }
                    pageCount = GetCurrentPageCount();
                    markRepaint = true;
                });
                thread.Start();
            }
            catch (Exception e)
            {
                ShowNotification(new GUIContent(e.Message));
            }
        }

        private void ClearEditorCache()
        {
            filterAssets.Clear();
            totalCount = doneCount = 0;
            pageCount = pageIndex = 0;
        }

        private bool CheckMismatchPattern(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            if (extension == ".meta")
            {
                return false;
            }

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            foreach (var pattern in patternSettings.assetPatterns)
            {
                if (Regex.IsMatch(extension, pattern.extensionPattern) && !Regex.IsMatch(fileName, pattern.namePattern))
                {
                    return true;
                }
            }
            return false;
        }

        private int GetCurrentPageCount()
        {
            return filterAssets.Count / EACH_PAGE_COUNT + (filterAssets.Count % EACH_PAGE_COUNT == 0 ? 0 : 1);
        }
        #endregion
    }
}