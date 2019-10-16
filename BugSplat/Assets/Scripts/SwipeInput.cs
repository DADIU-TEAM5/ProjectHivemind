using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeInput : MonoBehaviour
{
    //public static bool swipeEnabled;
    //public PlayerController playerCOntroller;
    //public float minDistance4Swipe = 100f;
    //public IntVariable swipe4Dash;

    //Touch currentTouch;
    //public Text debby;

    //Vector2 startPos;
    //Vector2 endPos;

    //float direction;


    //bool CurrentlyTOuching;
    

    // Start is called before the first frame update
    //void Start()
    //{
    //    swipeEnabled = true;
    //   // debby.text = Input.touchCount + "";
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if (swipeEnabled == true)
    //    {

    //        if (Input.touchCount > 0)
    //        {

    //            currentTouch = Input.GetTouch(0);

    //            if (startPos == Vector2.zero)
    //                startPos = currentTouch.position;

    //            endPos = currentTouch.position;

    //            //debby.text = startPos.x + " - " + endPos.x;
    //            direction = startPos.x - endPos.x;

    //        }
    //        else if (SwipeDistanceCheck())
    //        {
    //            if (direction < 0)
    //                direction = 1;
    //            else if (direction > 0)
    //                direction = -1;

    //            if (startPos != Vector2.zero)
    //                playerCOntroller.Dash(direction);
    //        } else { 

    //            startPos = Vector2.zero;
    //            // debby.text = direction + "";
    //        }
    //    }
    //}

    //private bool SwipeDistanceCheck()
    //{
    //    //return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;

    //    return Mathf.Abs(direction) > minDistance4Swipe;
    //}


}
