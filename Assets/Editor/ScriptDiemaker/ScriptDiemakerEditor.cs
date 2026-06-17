/*************************************************************************
 *  Copyright © 2025 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ScriptDiemakerEditor.cs
 *  Description  :  Editor for script diemaker.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/17
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEditor;
using UnityEngine;

namespace MGS.ScriptDiemaker.Editor
{
    public sealed class ScriptDiemakerEditor : ScriptableWizard
    {
        [MenuItem("Tools/Assistant/Diemaker Editor")]
        public static void Open()
        {
            DisplayWizard<ScriptDiemakerEditor>("Diemaker Editor", "Apply");
        }

        [SerializeField]
        string company;

        [SerializeField]
        string author;

        private void OnEnable()
        {
            company = EditorPrefs.GetString(ScriptDiemaker.USER_COMPANY);
            author = EditorPrefs.GetString(ScriptDiemaker.USER_AUTHOR);
        }

        private void OnWizardCreate()
        {
            EditorPrefs.SetString(ScriptDiemaker.USER_COMPANY, company);
            EditorPrefs.SetString(ScriptDiemaker.USER_AUTHOR, author);
        }
    }
}