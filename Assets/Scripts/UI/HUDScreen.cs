using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : MonoBehaviour
{
    [SerializeField] private Button _beginFightButton;
    [SerializeField] private BeginTurnMessage _turnMessage;
    [SerializeField] private HealthBar healthBarPrefab;

    private void Start()
    {
        _beginFightButton.onClick.AddListener(BeginFight);
        Game.Instance.BattleController.onTurnBegin += ShowTurnMessage;
        foreach (var player in Game.Instance.BattleController.AllPlayers)
        {
            foreach (var unit in player.AllUnits)
            {
                var healthbar = Instantiate(healthBarPrefab, transform);
                healthbar.SetUnit(unit);
            }
        }
    }

    public void ShowTurnMessage(Player player)
    {
        _turnMessage.ShowBeginTurnMessage(player);
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
    }
}
