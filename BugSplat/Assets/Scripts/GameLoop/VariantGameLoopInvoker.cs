using UnityEngine;

[CreateAssetMenu(menuName="GameLoops/VariantHandler")]
public class GameLoopVariant : SimpleGameLoopInvoker 
{
    [Min(1f)]
    [SerializeField]
    private float Frequency = 1;

    private float _tickFrequency;

    private bool _ticked = false;

    protected override void Init() {
        base.Init();
        _tickFrequency = 1f / Frequency;
    }

    public override void UpdateGameLoop(float time)
    {
        var deltaTime = time - _timeLastTrigger;
        if (deltaTime >= _tickFrequency) {
            _deltaTime = deltaTime;
            _timeLastTrigger = time;
            _ticked = true;

            for (var i = 0; i < _gameLoops.Count; i++) {
                var gameLoop = _gameLoops[i];    

                gameLoop.LoopUpdate(_deltaTime);
            }
        } 
    }

    public override void LateUpdateGameLoop(float time)
    {
        if (_ticked) {
            for (var i = 0; i < _gameLoops.Count; i++) {
                _gameLoops[i].LoopLateUpdate(_deltaTime);
            }
        }

        _ticked = false;
    }
}
