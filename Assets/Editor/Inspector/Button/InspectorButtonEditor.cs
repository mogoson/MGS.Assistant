/*************************************************************************
 *  Copyright © 2026 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  InspectorButtonEditor.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  03/20/2026
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MGS.Inspector.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class InspectorButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            OnInspectorCustomGUI();
        }

        protected void OnInspectorCustomGUI()
        {
            var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                var btnAttrType = typeof(InspectorButtonAttribute);
                if (Attribute.IsDefined(method, btnAttrType))
                {
                    var btnAttr = Attribute.GetCustomAttribute(method, btnAttrType) as InspectorButtonAttribute;
                    var btnName = string.IsNullOrEmpty(btnAttr.displayName) ? method.Name : btnAttr.displayName;
                    if (GUILayout.Button(btnName))
                    {
                        method.Invoke(target, default);
                    }
                }
            }
        }
    }
}