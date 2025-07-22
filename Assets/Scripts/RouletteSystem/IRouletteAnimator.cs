using System;

public interface IRouletteAnimator
{
    void AnimateSpinToNumber(int number, Action<int> onComplete);
}