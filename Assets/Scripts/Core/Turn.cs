using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    [SerializeField] Character character;

    public Turn(Character character)
    {
        this.character = character;
        BattleController.Instance.StartCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine()
    {
        character.pathmover.waitForCommand = false;
        while (!character.MoveFinished())
        {
            yield return null; //Ждем следующего апдейта чтобы продолжить исполнение кода
        }

        if (character.attackTarget != null)
        {
            character.DoAttack(character.attackTarget);
            yield return new WaitForSeconds(1.5f);
            character.attackTarget.DoAttack(character);
            yield return new WaitForSeconds(1.5f);
            character.DoRetreat(character.attackTarget);//почему-то после отступления ход не заканчивается. Нужно еще смотреть  
            //character.attackTarget.DoRetreat(character); // -- но это надо получать противополжные точки. Хотя мб other именно так и работает
            character.attackTarget = null;
        }
    }
}

