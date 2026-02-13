using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            PlayerSnake player = other.GetComponent<PlayerSnake>();
            if (player != null)
            {
                player.LevelComplete();
            }
        }
    }
}
