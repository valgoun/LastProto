using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoudiniEngineUnity;

#if UNITY_EDITOR

namespace LG.Tools.Graph.WorldBuilding
{
    
    /*
    // custom Scriptable object
    [CreateAssetMenu(fileName = "HDAToLoadList", menuName = "GraphPipeline")]
    public class HDAToLoad : ScriptableObject
    {
        public string pathToHDAs;
        public string[] HDAList;
    }
    */

    public class CadastreUtils
    {
        public static string WarmupScene()
        {
            // Get Session

            var session = HEU_SessionManager.GetOrCreateDefaultSession();

            if(!session.IsSessionValid())
            {
                return "houdini session not valid.\nPossible causes : Houdini version not up to date compared to Unity plugin version.";
            }

            //HEU_SessionManager.

            // Loads HDA in correct order
            //      Get assets scriptable Asset:
            //var info = new DirectoryInfo()

            string[] HDAs =
            {
                "Assets/Plugins/HoudiniEngineUnity/HDAs/UtilHDA/PathDeformer.hdanc",
                "Assets/Graph/Enviro/Building_Features/Destruction/DestructionTool.hda",
                "Assets/Graph/Enviro/Building_Features/Cadastre/splinemeshhousetool.hdanc"
            };

            foreach (var name in HDAs)
            {
                int libID;
                string[] AssetNames;
                HEU_HAPIUtility.LoadHDAFile( session, name, out libID, out AssetNames);
            }
            

            return "";
        }
    }
}

#endif