using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEnemiesLeft : GameLoop
{
    public GameObjectList EnemyListSO;
    public IntVariable EnemiesLeftSO;
    public GameEvent HasWonSO;

    
    // Start is called before the first frame update
    void Start()
    {
        FindGameObjectsWithLayer(8, EnemyListSO.List);
        EnemiesLeftSO.Value = EnemyListSO.List.Count;
    }

    public override void LoopUpdate(float deltaTime)
    {
        if (EnemiesLeftSO.Value == 0)
        {
            HasWonSO.Raise();
        }
    }

    private void FindGameObjectsWithLayer(int layer, List<GameObject> listSO)
    {
        listSO.Clear();

        var gameObjectsInScene = FindObjectsOfType<GameObject>();

        for (int i = 0; i < gameObjectsInScene.Length; i++)
        {
            if (gameObjectsInScene[i].layer == layer)
            {
                listSO.Add(gameObjectsInScene[i]);
            }
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }
}
