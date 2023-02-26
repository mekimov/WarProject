using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    protected override void ProcessInput()
    {
        Game.Instance.PathCreator.ManualUpdate(Input.GetButtonDown("Fire1"), Input.GetButton("Fire1"), Input.GetButtonUp("Fire1"));
    }
}
