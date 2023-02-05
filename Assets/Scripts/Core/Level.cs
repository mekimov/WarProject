using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] LevelConditions levelConditions;
    // Start is called before the first frame update
    void Start()
    {
        if (levelConditions == null)
        {
            Debug.LogError("ERROR! No level conditions set!");
        }
        Game.Instance.SetLevel(this);
    }

}
