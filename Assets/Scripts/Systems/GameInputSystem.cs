using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using LikeSoulKnight;

public interface IGameInputSystem : ISystem
{
    public GameInput GetGameInput();
}

public class GameInputSystem : AbstractSystem, IGameInputSystem
{
    private GameInput mGameInput = new GameInput();
    protected override void OnInit()
    {
        
        mGameInput.Enable();
    }

    public GameInput GetGameInput()
    {
        return mGameInput;
    }

    ~GameInputSystem()
    {
        mGameInput.Disable();
    }
}
