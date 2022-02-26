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
    // Start is called before the first frame update
    public void SwitchActivePlayer()
    {
        if (activePlayer == red)
        {
            activePlayer.OnEndTurn();
            activePlayer = blue;
            activePlayer.OnBeginTurn();
            Debug.Log("Active player - blue");
        }
        else
        {
            activePlayer.OnEndTurn();
            activePlayer = red;
            activePlayer.OnBeginTurn();
            Debug.Log("Active player - red");
        }
    }
    void Start()
    {
        Instance = this;
        activePlayer = red;
        red.OnBeginTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (activePlayer.AllCharactersFinishTurn())
            SwitchActivePlayer();
    }
}
