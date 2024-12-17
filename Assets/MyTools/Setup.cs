//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEditor;
//using static UnityEditor.AssetDatabase;
//using static System.IO.Directory;
//using static System.IO.Path;
//using static UnityEngine.UIElements.UxmlAttributeDescription;
//using UnityEditor.VersionControl;
//public static class Setup
//{
//    [MenuItem("Tools/Setup/Create Default Folders")]
//    public static void CreateDefaultFolders()
//    {
//        Folders.CreateDefault("_Project", "Animation","Art","Materials","Prefabs","ScriptableObjects","Scripts","Settings");
//        Refresh();
//    }

//    [MenuItem("Tools/Setup/Import My Favorite")]
//    public static void ImportMyFavoriteAssets()
//    {
//        Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");
//    }


//    static class Folders
//    {
//        public static void CreateDefault(string root, params string[] folders)
//        {
//            var fullpath = Combine(Application.dataPath, root);
//            foreach (var folder in folders)
//            {   
//                var path = Combine(fullpath, folder);
//                if (!Exists(path))
//                {
//                    CreateDirectory(path);
//                }
//            }
//        }
//    }

//    public static class Assets
//    {
//        public static void ImportAsset(string asset,string subfolder,string folder = "C:\\Users\\gauth\\AppData\\Roaming\\Unity\\Asset Store-5.x")
//        {
//            AssetDatabase.ImportPackage(Combine(folder, subfolder, asset), false);
//        }
//    }
//}