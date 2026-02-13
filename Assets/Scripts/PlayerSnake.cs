using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class PlayerSnake : MonoBehaviour
{
    public int headValue = 1;

    public GameObject segmentPrefab;
    public float segmentSpacing = 0.3f;

    private List<Transform> segments = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();
    public TextMeshPro headText;


    private void Start()
    {
        segments.Add(transform);
        headText.text = headValue.ToString();
    }


    private void Update()
    {
        RecordPosition();
        MoveSegments();
    }

    void RecordPosition()
    {
        positionHistory.Insert(0, transform.position);

        if (positionHistory.Count > 1000)
            positionHistory.RemoveAt(positionHistory.Count - 1);
    }

    

    void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            int index = Mathf.Min(Mathf.RoundToInt(i * segmentSpacing * 50f), positionHistory.Count - 1);

            // Smoothly move segment towards that position
            segments[i].position = Vector3.Lerp(
                segments[i].position,
                positionHistory[index],
                Time.deltaTime * 20f // speed of follow
            );

            // Optional: face the direction of movement
            Vector3 direction = positionHistory[index] - segments[i].position;
            if (direction.sqrMagnitude > 0.001f)
                segments[i].rotation = Quaternion.LookRotation(direction);
        }
    }




    public void AddNumber(int value)
    {
        headValue += value;
        headText.text = headValue.ToString();

        RebuildSnake();
        StartCoroutine(PopAnimation(transform));

        Handheld.Vibrate();
    }


    public void ReduceNumber(int value)
    {
        headValue -= value;

        // Check for level fail
        if (headValue < 1)
        {
            headValue = 0; // optional, show 0
            headText.text = headValue.ToString();
            StartCoroutine(PopAnimation(transform));

            for (int i = segments.Count - 1; i > 0; i--) // skip head at index 0
            {
                Destroy(segments[i].gameObject);
            }
            segments.Clear();
            segments.Add(transform); // keep head


            Handheld.Vibrate();
            LevelFailed();
            return; // stop further processing
        }

        headText.text = headValue.ToString();
        RebuildSnake();
        StartCoroutine(PopAnimation(transform));
        Handheld.Vibrate();
    }


    void RebuildSnake()
    {
        // Remove old body except head
        for (int i = segments.Count - 1; i > 0; i--)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        // Build new chain from headValue down to 1
        for (int i = headValue - 1; i >= 1; i--)
        {
            GameObject newSeg = Instantiate(segmentPrefab, transform.position, Quaternion.identity);
            newSeg.GetComponent<SnakeSegment>().SetNumber(i);
            segments.Add(newSeg.transform);
        }
    }

    private void LevelFailed()
    {
        Debug.Log("Level Failed!");

       
        this.enabled = false;
        this.GetComponent<PlayerMovement>().enabled = false;

       
    }

    public void LevelComplete()
    {
        Debug.Log("Level Completed!");


        // this.enabled = false;
        // this.GetComponent<PlayerController>().enabled = false;
        FindObjectOfType<CameraFollow>().enabled = false;

    }


    private IEnumerator PopAnimation(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * 1.3f; // scale up by 30%
        float duration = 0.1f; // scale up duration

        // Scale up
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            yield return null;
        }

        // Scale back
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
            yield return null;
        }

        target.localScale = originalScale;
    }

}
