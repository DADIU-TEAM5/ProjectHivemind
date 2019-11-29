using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonitorStatEvent : GameLoop
{
    public FloatVariable Stat;

    public float Threshhold;

    public GameEvent OverThreshhold, UnderThreshold;

    public UnityEvent OverAction, UnderAction;

    private bool _over = false;

    public void Start() {
        if (Stat.Value > Threshhold) {
            _over = true;
            OverThreshhold?.Raise(gameObject);
        } else {
            _over = false;
            UnderThreshold?.Raise(gameObject);
        }
    }

    public override void LoopLateUpdate(float deltaTime) {}

    public override void LoopUpdate(float deltaTime)
    {
        if (_over) {
            if (Stat.Value <= Threshhold) {
                UnderThreshold?.Raise(gameObject);
                _over = false;
                return;
            }

            OverAction.Invoke();
        } else {
            if (Stat.Value > Threshhold) {
                OverThreshhold?.Raise(gameObject);
                _over = true;
                return;
            }

            UnderAction.Invoke();
        }
    }
}
