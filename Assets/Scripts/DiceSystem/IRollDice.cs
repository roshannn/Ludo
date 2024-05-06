using Cysharp.Threading.Tasks;

public interface IRollDice {
    public UniTask<int> RollDice();

    public int GetCurrentRoll();
}
