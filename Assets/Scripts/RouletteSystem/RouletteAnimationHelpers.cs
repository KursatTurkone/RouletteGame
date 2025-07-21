using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RouletteAnimationHelpers
{
    public static float GetBallStartAngle(Transform ball, Vector3 orbitCenter)
    {
        Vector3 dir = (ball.position - orbitCenter);
        dir.y = 0;
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    public static float GetBallCurrentAngle(Transform ball, Vector3 orbitCenter)
    {
        Vector3 dir = (ball.position - orbitCenter);
        dir.y = 0;
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    public static float GetSlotAngle(int slotIndex, Transform[] slotAnchors, Vector3 orbitCenter)
    {
        Vector3 dir = (slotAnchors[slotIndex].position - orbitCenter);
        dir.y = 0;
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    public static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
}