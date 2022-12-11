using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private RectTransform rect;
    private Character character;

    public void SetCharacter(Character c)
    {
        character = c;
    }
   
    // Update is called once per frame
    void Update()
    {
        var pos = Camera.main.WorldToScreenPoint(character.transform.position + Vector3.up * 5 ); // Трансформирует точку игрового мира в точку с координатами x,y (0,1)
        pos.x = pos.x/Screen.width * 960f - 480f;
        pos.y = pos.y/Screen.height * 540f - 270f;
        rect.localPosition = pos;

        var scale = health.transform.localScale;
        scale.x = character.stats.CurrentHP / (float)character.stats.MaxHP;
        health.transform.localScale = scale;
    }
}
