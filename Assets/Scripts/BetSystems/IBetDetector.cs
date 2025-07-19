public interface IBetDetector
{
    bool TryDetect(float xPercent, float zPercent, int row, int col, int rows, int cols, out int[] numbers);
}