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
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;


    private void Start()
    {
        _beginFightButton.onClick.AddListener(Game.Instance.BeginFight);
        _beginFightButton.onClick.AddListener(LockBeginFightButton);
        Game.Instance.BattleController.onTurnBegin += ShowTurnMessage;
        Game.Instance.EventBus.onPlayerWin += ShowWinPanel;
        Game.Instance.EventBus.onPlayerLoose += ShowLoosePanel;
        Game.Instance.EventBus.onHumanPlayerTurnBegin += UnlockBeginFightButton;

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

    
    

    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
        _beginFightButton.gameObject.SetActive(false);
    }

    private void ShowLoosePanel()
    {
        loosePanel.SetActive(true);
        _beginFightButton.gameObject.SetActive(false);


    }

    private void LockBeginFightButton()
    {
        _beginFightButton.interactable = false;
    }

    private void UnlockBeginFightButton()
    {
        _beginFightButton.interactable = true;
    }
}
