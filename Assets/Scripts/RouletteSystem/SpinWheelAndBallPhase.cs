using System.Collections;
using UnityEngine;

public class SpinWheelAndBallPhase : IRouletteAnimationPhase
{
    private RouletteConfig _config;
    private OrbitConfig _orbitConfig;
    private RouletteSceneReferenceConfig _sceneReferences;
    public IEnumerator Play(int targetIndex)
    {
        float elapsed = 0f;
        float wheelStart = _sceneReferences.wheel.eulerAngles.y;
        float wheelTarget = wheelStart + 360f * _sceneReferences.rouletteConfig.wheelSpins + (360f * targetIndex / _sceneReferences.rouletteConfig.numberOrder.Length);
        float ballStartAngle = RouletteAnimationHelpers.GetBallStartAngle(_sceneReferences.ball, _orbitConfig.orbitCenter);
        float ballEndAngle = 360f * _config.ballSpins + RouletteAnimationHelpers.GetSlotAngle(targetIndex, _sceneReferences.slotAnchors, _orbitConfig.orbitCenter);

        while (elapsed < _config.spinDuration)
        {
            float t = elapsed / _config.spinDuration;
            float lerpT = t;

            _sceneReferences.wheel.eulerAngles = new Vector3(0, Mathf.Lerp(wheelStart, wheelTarget, lerpT), 0);

            float angle = Mathf.Lerp(ballStartAngle, ballEndAngle, lerpT);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _orbitConfig.spinOrbitRadius;
            _sceneReferences.ball.position = _orbitConfig.orbitCenter + offset + Vector3.up * _orbitConfig.ballHeight;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void Initialize(RouletteConfig config, OrbitConfig orbitConfig, RouletteSceneReferenceConfig sceneReferences)
    {
        _config = config;
        _orbitConfig = orbitConfig;
        _sceneReferences = sceneReferences;
        
    }

 
    
}