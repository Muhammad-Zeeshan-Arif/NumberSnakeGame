using UnityEngine;
using TMPro;

public class NumberObstacle : MonoBehaviour
{
    int damageValue = 2;
    private TextMeshPro text;

    private void Awake()
    {
        damageValue = Random.Range(2,6);
        text = GetComponentInChildren<TextMeshPro>();
        if (text != null)
            text.text = damageValue.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSnake player = other.GetComponent<PlayerSnake>();
            if (player != null)
            {
                player.ReduceNumber(damageValue);
                Destroy(gameObject);
            }
        }
    }
}
