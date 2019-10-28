using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform nullform = null;
        NavMeshBuildSettings buildSettings = new NavMeshBuildSettings();

        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
        

        List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(nullform, 0, NavMeshCollectGeometry.PhysicsColliders, 0, markups, buildSources);

        print(buildSources.Count);

        Bounds meshBounds = new Bounds();

        NavMeshBuilder.BuildNavMeshData(buildSettings, buildSources, meshBounds, Vector3.zero, Quaternion.identity);


        print("hello");
    }

}
