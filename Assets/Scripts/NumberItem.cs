using UnityEngine;
using TMPro;

public enum NumberEffect { None, Gain, Lose }
public class NumberItem : MonoBehaviour
{
   

    [Header("Item Settings")]
    [SerializeField] private NumberEffect numberEffect = NumberEffect.None;

    [Header("Value Range")]
    [SerializeField] private int minValue = 10;
    [SerializeField] private int maxValue = 60;

    int value;
    TextMeshPro text;

    private void Awake()
    {
        InitItem();
    }

    private void InitItem()
    {
        value = Random.Range(minValue, maxValue);
        text = GetComponentInChildren<TextMeshPro>();

        if (text != null)
            text.text = value.ToString();

        if (numberEffect == NumberEffect.Gain)
            text.color = Color.green;
        else
            text.color = Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerSnake player = other.GetComponent<PlayerSnake>();
        if (player == null) return;

        switch (numberEffect)
        {
            case NumberEffect.Gain:
                player.AddNumber(value);
                break;

            case NumberEffect.Lose:
                player.ReduceNumber(value);
                break;
        }
        gameObject.SetActive(false);
    }
}
