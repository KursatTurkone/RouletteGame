using System;
using System.Collections;
using UnityEngine;

public class RouletteAnimator : MonoBehaviour, IRouletteAnimator
{
    [Header("References")] [SerializeField]
    private Transform wheel;

    [SerializeField] private Transform ball;
    [SerializeField] private RouletteBallAudio ballAudio;

    [Header("Slot Anchors")] [SerializeField]
    private Transform[] slotAnchors;

    [Header("Config")] [SerializeField] private int[] numberOrder =
    {
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18,
        29, 7, 28, 12, 35, 3, 26
    };

    public int[] NumberOrder => numberOrder;

    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int wheelSpins = 4;
    [SerializeField] private int ballSpins = 8;

    [Header("Transition Animations")] [SerializeField]
    private float slideAngularSpeed = 400f;

    [SerializeField] private float jumpDuration = 0.35f;
    [SerializeField] private float jumpHeight = 0.10f;

    private Vector3 orbitCenter;
    private float spinOrbitRadius;
    private float ballHeight;

    private void Start()
    {
        orbitCenter = transform.position;
        spinOrbitRadius = Vector3.Distance(orbitCenter, ball.position);
        ballHeight = ball.position.y - orbitCenter.y;
    }

    public void AnimateSpinToNumber(int number, Action<int> onComplete)
    {
        int targetIndex = Array.IndexOf(numberOrder, number);
        if (targetIndex == -1)
        {
            Debug.LogError($"Invalid number {number}");
            onComplete?.Invoke(-1);
            return;
        }

        StartCoroutine(SpinRoutine(targetIndex, onComplete));
    }

    private IEnumerator SpinRoutine(int targetIndex, Action<int> onComplete)
    {
        ballAudio.PlayRollSound();
        float elapsed = 0f;
        float wheelStart = wheel.eulerAngles.y;
        float wheelTarget = wheelStart + 360f * wheelSpins + (360f * targetIndex / numberOrder.Length);

        float ballStartAngle = GetBallStartAngle();
        float ballEndAngle = 360f * ballSpins + GetSlotAngle(targetIndex);


        while (elapsed < spinDuration)
        {
            float t = elapsed / spinDuration;
            float easedT = Mathf.SmoothStep(0f, 1f, t);


            wheel.eulerAngles = new Vector3(0, Mathf.Lerp(wheelStart, wheelTarget, easedT), 0);


            float angle = Mathf.Lerp(ballStartAngle, ballEndAngle, t);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * spinOrbitRadius;
            ball.position = orbitCenter + offset + Vector3.up * ballHeight;

            elapsed += Time.deltaTime;
            yield return null;
        }


        float currentAngle = NormalizeAngle(GetBallCurrentAngle());
        float targetAngle = NormalizeAngle(GetSlotAngle(targetIndex));
        float angleDiff = targetAngle - currentAngle;
        if (angleDiff < 0) angleDiff += 360f;

        float duration = angleDiff / slideAngularSpeed;
        if (duration < 0.05f) duration = 0.05f;

        float slideElapsed = 0f;
        while (slideElapsed < duration)
        {
            float t = slideElapsed / duration;
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            float interpAngle = currentAngle + angleDiff * easedT;
            float rad = interpAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * spinOrbitRadius;
            ball.position = orbitCenter + offset + Vector3.up * ballHeight;

            slideElapsed += Time.deltaTime;
            yield return null;
        }

        float finalRad = targetAngle * Mathf.Deg2Rad;
        Vector3 finalOffset = new Vector3(Mathf.Cos(finalRad), 0, Mathf.Sin(finalRad)) * spinOrbitRadius;
        ball.position = orbitCenter + finalOffset + Vector3.up * ballHeight;


        Vector3 jumpStart = ball.position;
        Vector3 jumpEnd = slotAnchors[targetIndex].position;
        float jumpElapsed = 0f;
        while (jumpElapsed < jumpDuration)
        {
            float t = jumpElapsed / jumpDuration;
            float y = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            ball.position = Vector3.Lerp(jumpStart, jumpEnd, t) + Vector3.up * y;
            jumpElapsed += Time.deltaTime;
            yield return null;
        }

        ball.position = jumpEnd;
        ballAudio.StopRollSound();
        onComplete?.Invoke(numberOrder[targetIndex]);
    }

    private float GetBallStartAngle()
    {
        Vector3 dir = (ball.position - orbitCenter);
        dir.y = 0;
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    private float GetBallCurrentAngle()
    {
        Vector3 dir = (ball.position - orbitCenter);
        dir.y = 0;
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    private float GetSlotAngle(int slotIndex)
    {
        Vector3 dir = (slotAnchors[slotIndex].position - orbitCenter);
        dir.y = 0;
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
}