public class ScoreKeeper
{
    public int Score { get; private set; }

    public void IncreaseScore(int num)
    {
        Score += num;
    }
}
