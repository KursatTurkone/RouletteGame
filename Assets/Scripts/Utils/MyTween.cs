using System;
using System.Collections;
using UnityEngine;

public static class MyTween
{
    public static Coroutine MoveTo(MonoBehaviour runner, Transform target, Vector3 endPos, float duration,
        Action onComplete = null, EaseType easeType = EaseType.Linear)
    {
        var easeFunc = Easing.GetEaseFunc(easeType);
        return runner.StartCoroutine(MoveCoroutine(target, endPos, duration, onComplete, easeFunc));
    }

    public static Coroutine ScaleTo(MonoBehaviour runner, Transform target, Vector3 endScale, float duration,
        Action onComplete = null, EaseType easeType = EaseType.Linear)
    {
        var easeFunc = Easing.GetEaseFunc(easeType);
        return runner.StartCoroutine(ScaleCoroutine(target, endScale, duration, onComplete, easeFunc));
    }

    public static Coroutine RotateTo(MonoBehaviour runner, Transform target, Quaternion endRot, float duration,
        Action onComplete = null, EaseType easeType = EaseType.Linear)
    {
        var easeFunc = Easing.GetEaseFunc(easeType);
        return runner.StartCoroutine(RotateCoroutine(target, endRot, duration, onComplete, easeFunc));
    }

    public static Coroutine FadeCanvasGroupTo(MonoBehaviour runner, CanvasGroup cg, float endAlpha, float duration,
        Action onComplete = null, EaseType easeType = EaseType.Linear)
    {
        var easeFunc = Easing.GetEaseFunc(easeType);
        return runner.StartCoroutine(FadeCanvasGroupCoroutine(cg, endAlpha, duration, onComplete, easeFunc));
    }

    public static Coroutine JumpTo(
        MonoBehaviour runner,
        Transform target,
        Vector3 endPos,
        float jumpHeight,
        float duration,
        Action onComplete = null,
        EaseType easeType = EaseType.Linear)
    {
        var easeFunc = Easing.GetEaseFunc(easeType);
        return runner.StartCoroutine(JumpCoroutine(target, endPos, jumpHeight, duration, onComplete, easeFunc));
    }

    private static IEnumerator JumpCoroutine(
        Transform t,
        Vector3 end,
        float jumpHeight,
        float duration,
        Action onComplete,
        Func<float, float> ease)
    {
        Vector3 start = t.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t01 = Mathf.Clamp01(elapsed / duration);
            float eased = ease(t01);
            Vector3 pos = Vector3.Lerp(start, end, eased);
            float parabola = 4f * jumpHeight * (t01 - t01 * t01); // 0→max→0
            pos.y = Mathf.Lerp(start.y, end.y, eased) + parabola;
            t.position = pos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.position = end;
        onComplete?.Invoke();
    }

    private static IEnumerator MoveCoroutine(Transform t, Vector3 end, float duration, Action onComplete,
        Func<float, float> ease)
    {
        Vector3 start = t.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t01 = Mathf.Clamp01(elapsed / duration);
            t.position = Vector3.Lerp(start, end, ease(t01));
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.position = end;
        onComplete?.Invoke();
    }

    private static IEnumerator ScaleCoroutine(Transform t, Vector3 end, float duration, Action onComplete,
        Func<float, float> ease)
    {
        Vector3 start = t.localScale;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t01 = Mathf.Clamp01(elapsed / duration);
            t.localScale = Vector3.Lerp(start, end, ease(t01));
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.localScale = end;
        onComplete?.Invoke();
    }

    private static IEnumerator RotateCoroutine(Transform t, Quaternion end, float duration, Action onComplete,
        Func<float, float> ease)
    {
        Quaternion start = t.rotation;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t01 = Mathf.Clamp01(elapsed / duration);
            t.rotation = Quaternion.Lerp(start, end, ease(t01));
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.rotation = end;
        onComplete?.Invoke();
    }

    private static IEnumerator FadeCanvasGroupCoroutine(CanvasGroup cg, float endAlpha, float duration,
        Action onComplete, Func<float, float> ease)
    {
        float start = cg.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t01 = Mathf.Clamp01(elapsed / duration);
            cg.alpha = Mathf.Lerp(start, endAlpha, ease(t01));
            elapsed += Time.deltaTime;
            yield return null;
        }

        cg.alpha = endAlpha;
        onComplete?.Invoke();
    }
}

public static class Easing
{
    private static float Linear(float t) => t;

    private static float InOutQuad(float t) =>
        t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

    private static float OutCubic(float t) =>
        1f - Mathf.Pow(1f - t, 3f);

    private static float InCubic(float t) =>
        t * t * t;

    private static float InOutSine(float t) =>
        -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;

    public static Func<float, float> GetEaseFunc(EaseType type)
    {
        switch (type)
        {
            case EaseType.InOutQuad: return InOutQuad;
            case EaseType.OutCubic: return OutCubic;
            case EaseType.InCubic: return InCubic;
            case EaseType.InOutSine: return InOutSine;
            case EaseType.Linear:
            default: return Linear;
        }
    }
}

public enum EaseType
{
    Linear,
    InOutQuad,
    OutCubic,
    InCubic,
    InOutSine,
}