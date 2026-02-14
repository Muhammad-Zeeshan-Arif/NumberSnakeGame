//using UnityEngine;

//public class FinishLine : MonoBehaviour
//{
//    private void OnTriggerEnter(Collider other)
//    {

//        if (other.CompareTag("Player"))
//        {
//            PlayerSnake player = other.GetComponent<PlayerSnake>();
//            if (player != null)
//            {
//                player.SetLevelStatus(true);
//            }
//        }
//    }
//}


using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float drainStepDelay = 0.02f; // speed of snake draining
    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            PlayerSnake player = other.GetComponent<PlayerSnake>();
            if (player != null)
            {
                finished = true;
                player.Started = false;                 // stop movement
                player.DrainToOneFast(drainStepDelay);  // start drain effect

                // Wait until drain is done, then end level
                StartCoroutine(EndLevelAfterDrain(player));
            }
        }
    }

    private IEnumerator EndLevelAfterDrain(PlayerSnake player)
    {
        // Wait until the head reaches 1
        while (player != null && player.headValue > 1)
            yield return null;

        if (player != null)
            player.SetLevelStatus(true); // Level completes after drain
    }
}
