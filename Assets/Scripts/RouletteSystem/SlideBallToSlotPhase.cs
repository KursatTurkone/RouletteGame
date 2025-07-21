using System.Collections;
using UnityEngine;

public class SlideBallToSlotPhase : IRouletteAnimationPhase
{
    private RouletteConfig _config;
    private OrbitConfig _orbitConfig;
    private RouletteSceneReferenceConfig _sceneReferences;
    public void Initialize(RouletteConfig config, OrbitConfig orbitConfig, RouletteSceneReferenceConfig sceneReferences)
    {
        _config = config;
        _orbitConfig = orbitConfig;
        _sceneReferences = sceneReferences;
    }

    public IEnumerator Play(int targetIndex)
    {
        float currentAngle = RouletteAnimationHelpers.NormalizeAngle(RouletteAnimationHelpers.GetBallCurrentAngle(_sceneReferences.ball, _orbitConfig.orbitCenter));
        float targetAngle = RouletteAnimationHelpers.NormalizeAngle(RouletteAnimationHelpers.GetSlotAngle(targetIndex, _sceneReferences.slotAnchors, _orbitConfig.orbitCenter));
        float angleDiff = targetAngle - currentAngle;
        if (angleDiff < 0) angleDiff += 360f;

        float maxSlideSpeed = _config.slideAngularSpeed;
        float duration = angleDiff / maxSlideSpeed;
        if (duration < 0.25f) duration = 0.25f;

        float slideElapsed = 0f;
        while (slideElapsed < duration)
        {
            float t = slideElapsed / duration;
            float lerpT = t;
            float interpAngle = currentAngle + angleDiff * lerpT;
            float rad = interpAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _orbitConfig.spinOrbitRadius;
            _sceneReferences.ball.position = _orbitConfig.orbitCenter + offset + Vector3.up * _orbitConfig.ballHeight;

            slideElapsed += Time.deltaTime;
            yield return null;
        }

        float finalRad = targetAngle * Mathf.Deg2Rad;
        Vector3 finalOffset = new Vector3(Mathf.Cos(finalRad), 0, Mathf.Sin(finalRad)) * _orbitConfig.spinOrbitRadius;
        _sceneReferences.ball.position = _orbitConfig.orbitCenter + finalOffset + Vector3.up * _orbitConfig.ballHeight;
    }
}