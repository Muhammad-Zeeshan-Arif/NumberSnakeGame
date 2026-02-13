using UnityEngine;
using TMPro;
public class NumberPickup : MonoBehaviour
{
    int value = 0;
    private TextMeshPro text;

    private void Awake()
    {
        value = Random.Range(1, 6);
        text = GetComponentInChildren<TextMeshPro>();
        if (text != null)
            text.text = value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to player
        if (other.CompareTag("Player"))
        {
            PlayerSnake player = other.GetComponent<PlayerSnake>();
            if (player != null)
            {
                player.AddNumber(value);
                Destroy(gameObject); // remove pickup
            }
        }
    }
}
