/*************************************************************************
 *  Copyright © 2021 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  CubemapRendererEditor.cs
 *  Description  :  Render a scene into a static Cubemap asset.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  3/7/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEditor;
using UnityEngine;

namespace MGS.CubemapRenderer.Editor
{
    public class CubemapRendererEditor : ScriptableWizard
    {
        #region Field and Property
        [Tooltip("Source camera to render Cubemap.")]
        public Camera camera;

        [Tooltip("Width and height of a cube face in pixels.")]
        public int size = 128;

        [Tooltip("Pixel data format to be used for the Cubemap.")]
        public TextureFormat format = TextureFormat.ARGB32;

        [Tooltip("Should mipmaps be created?")]
        public bool mipmap = false;
        #endregion

        #region Private Method
        [MenuItem("Tools/Assistant/Cubemap Renderer")]
        public static void Open()
        {
            DisplayWizard<CubemapRendererEditor>("Cubemap Renderer", "Render");
        }

        private void OnEnable()
        {
            camera = Camera.main;
        }

        private void OnWizardUpdate()
        {
            if (camera && size > 0)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
        }

        private void OnWizardCreate()
        {
            var newCubemapSavePath = EditorUtility.SaveFilePanelInProject(
                "Save New Render Cubemap",
                "NewRenderCubemap",
                "cubemap",
                "Enter a file name to save the new render cubemap.");

            if (string.IsNullOrEmpty(newCubemapSavePath))
            {
                return;
            }

            Selection.activeObject = RenderToCubemap(camera, size, format, mipmap, newCubemapSavePath);
        }

        private Cubemap RenderToCubemap(Camera camera, int size, TextureFormat format, bool mipmap, string savePath)
        {
            var cubemap = new Cubemap(size, format, mipmap);
            camera.RenderToCubemap(cubemap);

            AssetDatabase.CreateAsset(cubemap, savePath);
            AssetDatabase.Refresh();
            return cubemap;
        }
        #endregion
    }
}