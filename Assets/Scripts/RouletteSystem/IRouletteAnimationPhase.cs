using System.Collections;
public interface IRouletteAnimationPhase
{
    void Initialize(RouletteConfig config,OrbitConfig orbitConfig, RouletteSceneReferenceConfig sceneReferences);
    IEnumerator Play(int targetIndex);
  
}