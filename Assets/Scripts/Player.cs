using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<Character> characters; //список всех персонажей игрока
    [SerializeField] Character selectedCharacter;
    public Character SelectedCharacter => selectedCharacter;
    void OnCharacterSelected(Character characterTarget)
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
            if (!c.TurnFinished())
                return false;
        }
        return (true);
    }
}
