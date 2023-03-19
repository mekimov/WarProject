using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public static Game Instance { get; private set; }
    [SerializeField] private BattleController _battleController;
    public BattleController BattleController => _battleController;
    [SerializeField] private CameraController _cameraController;
    public CameraController CameraController => _cameraController;
    [SerializeField] private PathCreator _pathCreator;
    public PathCreator PathCreator => _pathCreator;
    private Level _level;
    public Level Level => _level;

    private EventBus eventBus = new EventBus();
    public EventBus EventBus => eventBus;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Debug.LogError("ERROR! Game already instantiated");
            Destroy(this.gameObject);
        }
    }

    public void SetLevel(Level l)
    {
        _level = l; 
    }

    public void SetBattleController(BattleController b)
    {
        _battleController = b;
    }

    public void BeginFight()
    {

        if (!Game.Instance.BattleController.ActivePlayer.isFirstPlayer)
        {
            Game.Instance.BattleController.StopPreparing();
            var turn = new Turn(OnEndTurn);
            foreach (var p in Game.Instance.BattleController.AllPlayers)
            {
                foreach (var c in p.AllUnits)
                {
                    turn.AddUnit(c);
                }
            }

            Game.Instance.PathCreator.Stop();
            turn.Begin();
        }
        else
        {
            Game.Instance.BattleController.SwitchActivePlayer();
        }
    }

    private void OnEndTurn()
    {
        Game.Instance.BattleController.OnEndTurn();
        Game.Instance.BattleController.SwitchActivePlayer();
        Game.Instance.PathCreator.Continue();
    }
}
