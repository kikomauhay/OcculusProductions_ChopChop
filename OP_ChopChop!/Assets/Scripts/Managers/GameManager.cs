using UnityEngine;
using System;

/// <summary>
/// Oversees all the actions of other managers.
/// Uses a StateMachine to determine the next action in the game.
/// </summary>

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject _player;
    public GameState CurrentState { get; private set; }
    public GameObject Player { get => _player; }

    // I'll learn how events work when I need to
    // public static event Action<GameState> OnBeforeStateChanged;
    // public static event Action<GameState> OnAfterStateChanged;

    protected override void Awake() { base.Awake(); }
    void Start() => ChangeState(GameState.STARTING);

    
    public void ChangeState(GameState state)
    {
        if (state == CurrentState) return;

        // OnBeforeStateChanged?.Invoke(state);
        CurrentState = state;

        switch (state) 
        {
            case GameState.STARTING:
                StartGame();
                break;
            case GameState.WINNING:
                DoWinningLogic();
                break;
            case GameState.LOOSING:
                DoLoosingLogic();
                break;
            case GameState.QUITTING:
                DoQuit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        // OnAfterStateChanged?.Invoke(state);
    }

    void StartGame() {

    }

    void DoWinningLogic() {

    }

    void DoLoosingLogic() {

    }

    void DoQuit() { UIHandler.Instance.Quit(); }

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }
}

// will add more states the more the game gets developed
public enum GameState { STARTING, WINNING, LOOSING, QUITTING }