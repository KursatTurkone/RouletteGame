using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteAnimator : MonoBehaviour, IRouletteAnimator
{
    [SerializeField] private RouletteSceneReferenceConfig sceneReferences;
    private readonly OrbitConfig _orbitConfig = new OrbitConfig();

    private List<IRouletteAnimationPhase> _phases;
    

    private void Start()
    {
        _orbitConfig.orbitCenter = transform.position;
        _orbitConfig.spinOrbitRadius = Vector3.Distance(_orbitConfig.orbitCenter, sceneReferences.ball.position);
        _orbitConfig.ballHeight = sceneReferences.ball.position.y - _orbitConfig.orbitCenter.y;

        _phases = new List<IRouletteAnimationPhase>
        {
            new SpinWheelAndBallPhase(),
            new SlideBallToSlotPhase(),
            new JumpBallToSlotPhase()
        };
        _phases.ForEach(phase => phase.Initialize(sceneReferences.rouletteConfig, _orbitConfig, sceneReferences));
    }

    private void OnEnable()
    {
        GameEvents.OnSpinRequested += OnSpinRequested;
    }

    private void OnDisable()
    {
        GameEvents.OnSpinRequested -= OnSpinRequested;
    }

    private void OnSpinRequested(int number)
    {
        AnimateSpinToNumber(number, GameEvents.SpinCompleted);
    }

    public void AnimateSpinToNumber(int number, Action<int> onComplete)
    {
        int targetIndex = Array.IndexOf(RouletteNumbers.NumberOrder, number);
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
        onComplete?.Invoke(RouletteNumbers.NumberOrder[targetIndex]);
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