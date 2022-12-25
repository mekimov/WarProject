using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<Unit> characters; //список всех персонажей игрока
    public List<Unit> AllCharacters => characters;
    [SerializeField] Unit selectedCharacter;

    [SerializeField] private Color _color;
    public Color Color => _color;

    public Unit SelectedCharacter => selectedCharacter;

    private void Start()
    {
        foreach (var unit in characters)
        {
            unit.owner = this; //this -- обращение к текущему экземпляру класса
        }
    }
    public void BeginFight()
    {
        var turn = new Turn(BattleController.Instance.SwitchActivePlayer);
        foreach (var c in characters)
        {
            turn.AddUnit(c);
        }
        turn.Begin();
    }
    void OnCharacterSelected(Unit characterTarget)
    {
        if (selectedCharacter != null)
            selectedCharacter.OnUnselect();
        selectedCharacter = characterTarget;
        selectedCharacter.OnSelect();
    }
    void OnCharacterUnselected()
    {
        if (selectedCharacter != null)
            selectedCharacter.OnUnselect();
    }

    public void TrySelectCharacter(Unit target)
    {
        foreach (var c in characters)
        {
            if (c == target)
               {
                OnCharacterSelected(c);
                return;
            }
            
        }
    }
    public void OnBeginTurn()
    {
        OnCharacterSelected(characters[0]);
        foreach (var c in characters)
        {
            c.OnBeginTurn();
        }
    }
    public void OnEndTurn()
    {
        OnCharacterUnselected();
    }
    public bool AllCharactersFinishTurn()
    {
       foreach (var c in characters)
        {
            if (!c.MoveFinished())
                return false;
        }
        return (true);
    }
}
