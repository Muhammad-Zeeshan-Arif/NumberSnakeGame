using UnityEngine;
using TMPro;

public class SnakeSegment : MonoBehaviour
{
    private TextMeshPro text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }

    public void SetNumber(int number)
    {
        text.text = number.ToString();
    }
}
