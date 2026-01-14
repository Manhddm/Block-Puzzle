using System;
using CommonScripts;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public Board board;
    public Blocks blocks;

    private void Start()
    {
        board.Init();
    }
}
