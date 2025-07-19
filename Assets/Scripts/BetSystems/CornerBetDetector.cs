public class CornerBetDetector : IBetDetector
{
    private readonly float threshold;
    public CornerBetDetector(float threshold = 0.18f) { this.threshold = threshold; }

    public bool TryDetect(float xPercent, float zPercent, int row, int col, int rows, int cols, out int[] numbers)
    {
        numbers = null;
        if (xPercent <= threshold && zPercent <= threshold && row > 0 && col > 0)
        {
            numbers = new int[]
            {
                row * cols + col + 1,
                (row - 1) * cols + col + 1,
                row * cols + (col - 1) + 1,
                (row - 1) * cols + (col - 1) + 1
            };
            return true;
        }
        if (xPercent >= 1f - threshold && zPercent <= threshold && row < rows - 1 && col > 0)
        {
            numbers = new int[]
            {
                (row + 1) * cols + col + 1,
                row * cols + col + 1,
                (row + 1) * cols + (col - 1) + 1,
                row * cols + (col - 1) + 1
            };
            return true;
        }
        if (xPercent <= threshold && zPercent >= 1f - threshold && row > 0 && col < cols - 1)
        {
            numbers = new int[]
            {
                row * cols + (col + 1) + 1,
                (row - 1) * cols + (col + 1) + 1,
                row * cols + col + 1,
                (row - 1) * cols + col + 1
            };
            return true;
        }
        if (xPercent >= 1f - threshold && zPercent >= 1f - threshold && row < rows - 1 && col < cols - 1)
        {
            numbers = new int[]
            {
                (row + 1) * cols + (col + 1) + 1,
                row * cols + (col + 1) + 1,
                (row + 1) * cols + col + 1,
                row * cols + col + 1
            };
            return true;
        }
        return false;
    }
}