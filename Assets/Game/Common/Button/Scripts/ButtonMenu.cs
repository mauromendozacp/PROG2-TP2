using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    [SerializeField] private AudioEvent clickEvent = null;
    [SerializeField] private RectTransform rectTransform = null;
    [SerializeField] private float normalSize = 0f;
    [SerializeField] private float selectedSize = 0f;

    private Button button = null;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnDisable()
    {
        ToggleSelected(false);
    }

    public void OnClick()
    {
        if (!button.interactable) return;

        GameManager.Instance.AudioManager.PlayAudio(clickEvent);
    }

    public void ToggleSelected(bool status)
    {
        if (!button.interactable) return;

        float size = status ? selectedSize : normalSize;
        rectTransform.localScale = new Vector3(size, size, size);
    }
}
