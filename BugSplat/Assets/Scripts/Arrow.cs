using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : GameLoop
{

    public GameObjectList EnemyList;

    public BoolVariable NoVisibleEnemies;

    GameObject _arrow;
    LineRenderer _arrowRenderer;

    private void Start()
    {
        _arrow = new GameObject();
        _arrow.transform.position = transform.position;
        _arrow.transform.rotation = transform.rotation;

        _arrow.AddComponent<LineRenderer>();
        _arrowRenderer = _arrow.GetComponent<LineRenderer>();

        //_arrow.SetActive(false);

        DrawArrow();
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        
    }

    public override void LoopUpdate(float deltaTime)
    {

        if (NoVisibleEnemies.Value)
        {

            if (_arrow.activeSelf == false)
                _arrow.SetActive(true);

            _arrow.transform.position = transform.position;

            float distance = float.MaxValue;
            int index = 0;
            for (int i = 0; i < EnemyList.Items.Count; i++)
            {


                float distanceToEnemy = Vector3.Distance(transform.position, EnemyList.Items[i].transform.position);

                if (distanceToEnemy < distance)
                {
                    index = i;
                    distance = distanceToEnemy;
                }
            }

            Vector3 enemyPos = EnemyList.Items[index].transform.position;
            enemyPos.y = _arrow.transform.position.y;
            _arrow.transform.LookAt(enemyPos);



            //_arrow.transform.Rotate(0, 20 * Time.deltaTime, 0);
            DrawArrow();
        }
        else
        {
            if(_arrow.activeSelf == true)
            _arrow.SetActive(false);
        }
      

    }

    

    void DrawArrow()
    {
        Vector3[] pointsForArrow = new Vector3[6];
        _arrowRenderer.positionCount = 6;

        pointsForArrow[0] = _arrow.transform.TransformPoint(Vector3.back);





        pointsForArrow[1] = _arrow.transform.TransformPoint(Vector3.forward *1.3f);

        pointsForArrow[2] = _arrow.transform.TransformPoint(Vector3.forward*.7f + Vector3.right*.5f);

        pointsForArrow[3] = _arrow.transform.TransformPoint(Vector3.forward * 1.3f);

        pointsForArrow[4] = _arrow.transform.TransformPoint(Vector3.forward * .7f + Vector3.left * .5f);

        pointsForArrow[5] = _arrow.transform.TransformPoint(Vector3.forward * 1.3f);





        _arrowRenderer.SetPositions(pointsForArrow);
        _arrowRenderer.widthMultiplier = 0.1f;

        //_coneRenderer.loop = true;
    }

}
