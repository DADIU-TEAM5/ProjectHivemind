using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitclikOrTap : GameLoop
{
    public GameObject Scenehandle;
    public override void LoopUpdate(float deltaTime)
    {
		if (Input.GetMouseButtonUp(0) || Input.touchCount > 0)
		{
			Scenehandle.SetActive(true);
		}
	}
    void OnEnable()
    {
		Scenehandle.SetActive(false);
	}
    public override void LoopLateUpdate(float deltaTime)
    {

    }
}
