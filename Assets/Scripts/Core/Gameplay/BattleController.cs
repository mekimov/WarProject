using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] Player red;
    public Player HumanPlayer => red;
    [SerializeField] Player blue;
    private Player activePlayer;
    public Player ActivePlayer => activePlayer;

    public Action<Player> onTurnBegin;
    public IEnumerable<Player> AllPlayers
    {
        get { yield return red;
            yield return blue;
        }

    }
    // Start is called before the first frame update

    public void OnEndTurn()
    {
        red.OnEndTurn();
        blue.OnEndTurn();
        red.OnBeginTurn();
        onTurnBegin?.Invoke(red);

    }
    public void SwitchActivePlayer()
    {
        if (activePlayer == red)
        {
            activePlayer.OnEndTurn();
            activePlayer = blue;
            activePlayer.OnBeginTurn();
            
            onTurnBegin?.Invoke(activePlayer);
        }
        else
        {
            activePlayer.OnEndTurn();
            activePlayer = red;
            activePlayer.OnBeginTurn();
            
            onTurnBegin?.Invoke(activePlayer);
        }
    }
    public void StopPreparing()
    {
        activePlayer.OnEndTurn();
    }

    private void Start()
    {
        //Game.Instance.SetBattleController(this);
        activePlayer = red;
        red.OnBeginTurn();
        onTurnBegin?.Invoke(activePlayer);
    }

    // Update is called once per frame

}
