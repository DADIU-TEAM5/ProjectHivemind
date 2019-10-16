using UnityEngine;

[CreateAssetMenu(menuName="GameLoops/VariantHandler")]
public class GameLoopVariant : SimpleGameLoopInvoker 
{
    [Min(1f)]
    [SerializeField]
    private float Frequency = 1;

    private float _tickFrequency;

    protected override void Init() {
        base.Init();
        _tickFrequency = 1f / Frequency;
    }

    public override bool UpdateGameLoop(float time)
    {
        var deltaTime = time - _timeLastTrigger;
        if (deltaTime >= _tickFrequency) {
            _deltaTime = deltaTime;
            _timeLastTrigger = time;

            for (var i = 0; i < _gameLoops.Count; i++) {
                var gameLoop = _gameLoops[i];    

                gameLoop.LoopUpdate(_deltaTime);
            }

            return true;
        } 

        return false;
    }

    public override void LateUpdateGameLoop()
    {
        for (var i = 0; i < _gameLoops.Count; i++) {
            _gameLoops[i].LoopLateUpdate(_deltaTime);
        }
    }
}
