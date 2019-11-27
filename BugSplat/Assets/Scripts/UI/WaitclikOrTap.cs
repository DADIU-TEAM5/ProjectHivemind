using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitclikOrTap : MonoBehaviour
{
    public GameObject Scenehandle;
	void Update()
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
}
