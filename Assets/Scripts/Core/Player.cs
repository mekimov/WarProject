using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<Character> characters; //список всех персонажей игрока
    public List<Character> AllCharacters => characters;
    [SerializeField] Character selectedCharacter;

    [SerializeField] private Color _color;
    public Color Color => _color;

    public Character SelectedCharacter => selectedCharacter;

    public void BeginFight()
    {
        foreach (var c in characters)
        {
            c.pathmover.waitForCommand = false;
        }
    }
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

    public void TrySelectCharacter(Character target)
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
            if (!c.TurnFinished())
                return false;
        }
        return (true);
    }
}
