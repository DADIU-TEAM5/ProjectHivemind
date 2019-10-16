using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstantiater : MonoBehaviour
{
    public static AudioInstantiater Instance;

    public GameObject[] AudioObjects;

    void Awake() {
        if (Instance) {
            DestroyImmediate(this);
            return;
        }

        DontDestroyOnLoad(this);

        for (int i = 0; i < AudioObjects.Length; i++) {
            var ao =  Instantiate(AudioObjects[i]);
            DontDestroyOnLoad(ao);

            AudioObjects[i] = ao;
        }

        Instance = this;
    }
}
