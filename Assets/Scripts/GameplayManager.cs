using CommonScripts;

public class GameplayManager : Singleton<GameplayManager>
{
    public Board board;
    public Blocks blocks;

    private void Start()
    {
        board.Init();
    }
}
