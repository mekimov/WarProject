using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    private BattleController _battleController;
    public BattleController BattleController => _battleController;
    private CameraController _cameraController;
    public CameraController CameraController => _cameraController;
    private Level _level;
    public Level Level => _level;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Debug.LogError("ERROR! Game already instantiated");
            Destroy(this.gameObject);
        }
    }

    public void SetLevel(Level l)
    {
        _level = l; 
    }

    public void SetBattleController(BattleController b)
    {
        _battleController = b;
    }
}
