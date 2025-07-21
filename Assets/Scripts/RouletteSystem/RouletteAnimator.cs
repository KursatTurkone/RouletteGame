using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteAnimator : MonoBehaviour, IRouletteAnimator
{
    [SerializeField] private RouletteSceneReferenceConfig sceneReferences;
    private readonly OrbitConfig _orbitConfig = new OrbitConfig();

    private List<IRouletteAnimationPhase> _phases;

    [HideInInspector] public int[] numberOrder =
    {
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18,
        29, 7, 28, 12, 35, 3, 26
    };

    private void Start()
    {
        _orbitConfig.orbitCenter = transform.position;
        _orbitConfig.spinOrbitRadius = Vector3.Distance(_orbitConfig.orbitCenter, sceneReferences.ball.position);
        _orbitConfig.ballHeight = sceneReferences.ball.position.y - _orbitConfig.orbitCenter.y;
        GameManager.Instance.rouletteAnimator = this;

        _phases = new List<IRouletteAnimationPhase>
        {
            new SpinWheelAndBallPhase(),
            new SlideBallToSlotPhase(),
            new JumpBallToSlotPhase()
        };
        _phases.ForEach(phase => phase.Initialize(sceneReferences.rouletteConfig, _orbitConfig, sceneReferences));
    }

    public void AnimateSpinToNumber(int number, Action<int> onComplete)
    {
        int targetIndex = Array.IndexOf(numberOrder, number);
        if (targetIndex == -1)
        {
            onComplete?.Invoke(-1);
            return;
        }

        StartCoroutine(SpinRoutine(targetIndex, onComplete));
    }

    private IEnumerator SpinRoutine(int targetIndex, Action<int> onComplete)
    {
        sceneReferences.ballAudio.PlayRollSound();

        foreach (var phase in _phases)
            yield return phase.Play(targetIndex);

        sceneReferences.ballAudio.StopRollSound();
        onComplete?.Invoke(numberOrder[targetIndex]);
    }
}

[System.Serializable]
public class RouletteSceneReferenceConfig
{
    public Transform wheel;
    public Transform ball;
    public RouletteBallAudio ballAudio;
    public Transform[] slotAnchors;
    public RouletteConfig rouletteConfig;
}

public class OrbitConfig
{
    public Vector3 orbitCenter;
    public float spinOrbitRadius;
    public float ballHeight;
}