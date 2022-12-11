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
        BattleController.Instance.onTurnBegin += ShowTurnMessage;
        foreach (var player in BattleController.Instance.AllPlayers)
        {
            foreach (var character in player.AllCharacters)
            {
                var healthbar = Instantiate(healthBarPrefab, transform);
                healthbar.SetCharacter(character);
            }
        }
    }

    public void ShowTurnMessage(Player player)
    {
        _turnMessage.ShowBeginTurnMessage(player);
    }

    public void BeginFight()
    {
        Debug.LogError("Active player: " + BattleController.Instance.ActivePlayer.SelectedCharacter.gameObject.name);
        BattleController.Instance.ActivePlayer.BeginFight();
        
    }
}
