using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventListener : MonoBehaviour
{
    public List<EventListener> EventListeners;

    private void Awake()
    {
        for (var i = 0; i < EventListeners.Count; i++) {
            var eventListener = EventListeners[i];

            var listener = gameObject.AddComponent<GameEventListener>();
           listener.Event = eventListener.Event;
           listener.Response = eventListener.Response;
        }        
    }

    private void Start() {
        Destroy(this);
    }

    [System.Serializable]
    public class EventListener {
        public GameEvent Event;
        public GameUnityEvent Response;
    }
}
