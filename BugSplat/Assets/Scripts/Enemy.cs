using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameLoop
{
    public GameObjectList EnemyList;
    public GameObjectVariable LockedTarget;
    public GameObjectVariable TargetGraphic;

    public BoolVariable NoVisibleEnemies;

    private void OnEnable()
    {
        Debug.Log(name + " spawned");
        EnemyList.Add(gameObject);
    }

    public abstract bool IsVisible();

    public abstract void TakeDamage(float damage);


    public void RemoveFromLockedTargetIfNotVisible()
    {
        
        if (LockedTarget.Value == gameObject)
        {
            if(TargetGraphic.Value == null)
            {
                TargetGraphic.Value = GameObject.CreatePrimitive(PrimitiveType.Quad);
                TargetGraphic.Value.name = "Target Graphic";
                Destroy(TargetGraphic.Value.GetComponent<MeshCollider>());
                TargetGraphic.Value.transform.rotation = Quaternion.Euler(90, 0, 0);
                TargetGraphic.Value.GetComponent<Renderer>().material.color = Color.red;
                

            }
            else
            {
                if (TargetGraphic.Value.activeSelf == false)
                    TargetGraphic.Value.SetActive(true);

                TargetGraphic.Value.transform.position = transform.position;
            }

            if (IsVisible() == false)
            {
                TargetGraphic.Value.SetActive(false);
                LockedTarget.Value = null;
            }
        }

        if(LockedTarget.Value == null)
        {
            if(TargetGraphic.Value != null)
            TargetGraphic.Value.SetActive(false);
        }
    }
}
