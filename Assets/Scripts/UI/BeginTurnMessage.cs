using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeginTurnMessage : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _playerNameText;
    
    public void ShowBeginTurnMessage(Player player)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowBeginTurnMessageRoutine(player));
    }

    private IEnumerator ShowBeginTurnMessageRoutine(Player player)
    {
        _playerNameText.text = player.name;
        _playerNameText.color = player.Color;
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }
    
}
