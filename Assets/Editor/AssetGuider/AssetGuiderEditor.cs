/*************************************************************************
 *  Copyright © 2021 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AssetGuiderEditor.cs
 *  Description  :  Capture the event of open Asset in Unity and call OS
 *                  open the Asset use the default application.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  3/7/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEditor;
using UnityEngine;

namespace MGS.AssetGuider.Editor
{
    public class AssetGuiderEditor : ScriptableWizard
    {
        [MenuItem("Tools/Assistant/Asset Guider")]
        public static void Open()
        {
            DisplayWizard<AssetGuiderEditor>("Asset Guider", "Apple");
        }

        [SerializeField]
        private string extensions;

        private void OnEnable()
        {
            extensions = EditorPrefs.GetString(AssetGuider.KEY_GUIDER_EXTENSIONS);
            if (string.IsNullOrEmpty(extensions))
            {
                extensions = ".txt$|.json$|.shader$";
                AppleEditorSettings();
            }
        }

        private void OnWizardCreate()
        {
            AppleEditorSettings();
        }

        private void AppleEditorSettings()
        {
            EditorPrefs.SetString(AssetGuider.KEY_GUIDER_EXTENSIONS, extensions);
        }
    }
}