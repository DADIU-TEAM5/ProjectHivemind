using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCam;

    public AnimationCurve FOVChange;

    public float LerpDuration = 0.1f;

    private float _initialFOV;

    private Transform _initialFollow;

    private CinemachineTransposer _transposer;

    void Start() {
        _initialFOV = VirtualCam.m_Lens.FieldOfView;
        _initialFollow = VirtualCam.Follow;

        _transposer = VirtualCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    public void ChangeFOV(float delta) {
        StopAllCoroutines();
        StartCoroutine(SlowChangeFOV(delta));
    }

    private IEnumerator SlowChangeFOV(float delta) {
        var desiredFOV = _initialFOV + delta; 
        var startFOV = VirtualCam.m_Lens.FieldOfView;

        var damping = 5f;

        _transposer.m_XDamping = damping;
        _transposer.m_YDamping = damping;
        _transposer.m_ZDamping = damping;
        //VirtualCam.Follow = null;

        for (var time = 0f; time < LerpDuration; time += Time.deltaTime) {
            var animationTime = FOVChange.Evaluate(time / LerpDuration);

            var fov = Mathf.Lerp(startFOV, desiredFOV, animationTime);
            var newDamp = Mathf.Lerp(damping, 0f, animationTime);
            _transposer.m_XDamping = newDamp;
            _transposer.m_YDamping = newDamp;
            _transposer.m_ZDamping = newDamp;   
            VirtualCam.m_Lens.FieldOfView = fov;
            yield return null;
        }

        VirtualCam.m_Lens.FieldOfView = desiredFOV;
        
        
        

        //VirtualCam.Follow = _initialFollow;
    }
}
