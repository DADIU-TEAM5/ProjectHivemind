using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : GameLoop
{
    public Rigidbody Body;
    public Vector3Variable PlayerPosition;
    public float PickUpRange;
    public IntVariable BodyParts;
    public IntVariable LevelEarnedMoney;

    public GameEvent PickedUpEvent;

    public float enableTime = 1;
    public float PickUpSpeed;
    public float ExplosionDistance;
    public float ExplosionHeight;

    public int minValue, MaxValue;

    public Collider Collider;

    float _time = 0;

    bool _pickedUp = false;
    
    // Start is called before the first frame update
  
    private void OnEnable()
    {
        //print("BODY PART!");

        Body.AddForce(new Vector3(Random.Range(-ExplosionDistance, ExplosionDistance), Random.Range(ExplosionHeight + Random.Range(-ExplosionHeight, ExplosionHeight), ExplosionHeight + Random.Range(-ExplosionHeight, ExplosionHeight)), Random.Range(-ExplosionDistance, ExplosionDistance)));
    }

    public override void LoopUpdate(float deltaTime)
    {
        //print("useing loop");
        

        _time += Time.deltaTime;
        if (_time >= enableTime)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, PlayerPosition.Value);
            //print(distanceToPlayer);

            if (distanceToPlayer < PickUpRange)
            {
                _pickedUp = true;
                if (Body != null)
                    Destroy(Body);
                if(Collider != null)
                Destroy(Collider);



            }
        }

        if (_pickedUp)
        {
            PickUpSpeed += Time.deltaTime*5;
            float distanceToPlayer = Vector3.Distance(transform.position, PlayerPosition.Value);
            transform.LookAt(PlayerPosition.Value);
            transform.Translate(Vector3.forward * Time.deltaTime * PickUpSpeed);

            if (distanceToPlayer < 0.2f)
            {
                var increase = Random.Range(minValue, MaxValue);
                BodyParts.Value += increase;
                LevelEarnedMoney.Value += increase;


                PickedUpEvent?.Raise(gameObject);
                Destroy(gameObject);
            }
        }
    }
    public override void LoopLateUpdate(float deltaTime)
    {
        
    }

}
