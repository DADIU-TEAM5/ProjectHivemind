using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeMaterialChange : MonoBehaviour
{
    public SkinnedMeshRenderer[] Meshes;

    private Material[] _defaultMaterials;

    // Start is called before the first frame update
    void Awake()
    {
        _defaultMaterials = new Material[Meshes.Length];
        for (var i = 0; i < Meshes.Length; i++) {
            _defaultMaterials[i] = Meshes[i].material;
        }
    }

    public void SetMaterial(Material material) {
       for (var i = 0; i < Meshes.Length; i++) {
           Meshes[i].material = material;
       } 
    }

    public void SetDefault() {
        for (var i = 0; i < Meshes.Length; i++) {
           Meshes[i].material = _defaultMaterials[i];
       }
    }
}
