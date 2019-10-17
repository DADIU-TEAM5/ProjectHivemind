using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManyCubes : GameLoop
{
    public GameObject Cube;

    public float SpawnIntervalSeconds = 1f;

    private float _localTimer = 0f;

    private int _column = 0;

    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public override void LoopUpdate(float deltaTime)
    {
        _localTimer += deltaTime;

        if (_localTimer >= SpawnIntervalSeconds) {
            var newCube = Instantiate(Cube);
            newCube.transform.position = Vector3.zero + new Vector3(_column, 0, 0);

            _column++;
            _column %= 10;

            _localTimer = 0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
