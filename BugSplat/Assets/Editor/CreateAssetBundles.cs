using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles 
{
    [MenuItem("Assets/ Build AssetBundles")]
    static void BuildAllAssetBundles() {
        var assetBundleDirectory = "Assets/AssetBundles";

        if (!Directory.Exists(assetBundleDirectory)) Directory.CreateDirectory(assetBundleDirectory);

        var manifest = BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);

        if (manifest == null) {
            Debug.LogError("Manifest is null, asset budles big errr");
        }
    }
}
