using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }
    [SerializeField] Player red;
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
    void Awake()
    {
        Instance = this;
        activePlayer = red;
        red.OnBeginTurn();
    }

    private void Start()
    {
        onTurnBegin?.Invoke(activePlayer);
    }

    // Update is called once per frame

}
