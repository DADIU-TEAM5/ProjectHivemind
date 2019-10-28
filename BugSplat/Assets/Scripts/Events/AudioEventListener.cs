using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventListener : MonoBehaviour
{
    public EventListener[] EventListeners;

    private void Awake()
    {
        for (var i = 0; i < EventListeners.Length; i++) {
            var eventListener = EventListeners[i];

            var listener = gameObject.AddComponent<GameEventListener>();
           listener.Event = eventListener.Event;
           listener.Response = eventListener.Response;
        }        
    }

    [System.Serializable]
    public class EventListener {
        public GameEvent Event;
        public UnityEvent Response;
    }
}
