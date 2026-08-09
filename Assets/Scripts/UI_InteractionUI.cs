using UnityEngine;
using TMPro;

public class UI_InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;

    public void ShowText(string text)
    {
        interactionText.text = text;
        interactionText.gameObject.SetActive(true);
    }

    public void HideText() => interactionText.gameObject.SetActive(false);
}