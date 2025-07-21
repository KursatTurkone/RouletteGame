using System.Collections;
using UnityEngine;

public class JumpBallToSlotPhase : IRouletteAnimationPhase
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
        Vector3 jumpStart = _sceneReferences.ball.position;
        Vector3 jumpEnd = _sceneReferences.slotAnchors[targetIndex].position;
        float jumpElapsed = 0f;
        while (jumpElapsed < _config.jumpDuration)
        {
            float t = jumpElapsed / _config.jumpDuration;
            float y = Mathf.Sin(Mathf.PI * t) * _config.jumpHeight;
            _sceneReferences.ball.position = Vector3.Lerp(jumpStart, jumpEnd, t) + Vector3.up * y;
            jumpElapsed += Time.deltaTime;
            yield return null;
        }

        _sceneReferences.ball.position = jumpEnd;
    }
}