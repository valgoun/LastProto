using UnityEngine;
using System.Collections;
using UnityEditor;



namespace LG.Tools.Graph.WorldBuilding
{
    public class CadastreEditor : EditorWindow
    {


	    private Rect ToolWindow_rect = new Rect(0f, 0f, 320f, 480f);


        [MenuItem("Window/LGTool")]
        static void Init()
        {
            CadastreEditor window = (CadastreEditor)EditorWindow.GetWindow(typeof(CadastreEditor));
            window.Show();
        }

        void OnGUI()
        {
            ToolWindow_rect = GUI.Window(0, ToolWindow_rect, WindowFunc, new GUIContent("Cadastre Editor", null, ""));

            // WARMUP button
            if (GUI.Button(new Rect(5f, 20f, 160f, 100f), new GUIContent("Warmup !", null, "Loads everything so your scene stop bullying you with the most annoying warnings")))
            {
                string returnedMsg = CadastreUtils.WarmupScene();
                if ( returnedMsg != "")
                {
                    EditorUtility.DisplayDialog("CadastreWarmupFailed", "warmup failed : " + returnedMsg, "ok");
                }
            }
        }

        void WindowFunc(int winID)
        {
        }
    }
}