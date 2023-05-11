using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private CharacterDatabase CharacterDatabase;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Image characterIconImage;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text characterNameText;

    public void UpdateDisplay(CharacterSelectState state)
    {
        if(state.CharacterId != -1)
        {
            var character = CharacterDatabase.GetCharacterById(state.CharacterId);
            characterIconImage.sprite = character.Icon;
            characterIconImage.enabled = true;
            characterNameText.text = character.DisplayName;
        }
        else{
            characterIconImage.enabled = false;
        }

        playerNameText.text = state.IsLockedIn ? $"Player {state.ClientId}" : $"Player {state.ClientId} (Picking...)";
        visuals.SetActive(true);
    }

    public void DisableDisplay()
    {
        visuals.SetActive(false);
    }
}
