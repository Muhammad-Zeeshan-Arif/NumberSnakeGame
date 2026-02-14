//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using System.Collections;

//public class PlayerSnake : MonoBehaviour
//{


//    [Header("Forward Movement")]
//    [SerializeField] private float forwardSpeed = 8f;

//    [Header("Horizontal Movement")]
//    [SerializeField] private float horizontalSpeed = 10f;
//    [SerializeField] private float horizontalLimit = 4f;

//    [Header("Head Settings")]
//    int headValue = 1;
//    [SerializeField] private TextMeshPro headText;

//    [Header("Segment Settings")]
//    [SerializeField] private GameObject segmentPrefab;
//    [SerializeField] private Transform segmentsParent;
//    [SerializeField] private float segmentSpacing = 0.3f;
//    [SerializeField] private int maxVisibleSegments = 25;

//    private List<Transform> segments = new List<Transform>();
//    private List<Vector3> positionHistory = new List<Vector3>();
//    private List<GameObject> segmentPool = new List<GameObject>();



//    [Header("Collision Particle")]
//    [SerializeField] private GameObject particleEffect;

//    float horizontalInput;
//    float horizontalVelocity = 0f;

//    bool hasStarted = false;
//    public bool Started { get => hasStarted; set => hasStarted = value; }



//    private void Start()
//    {

//#if UNITY_EDITOR
//        horizontalSpeed = 100f;
//#endif

//        segments.Add(transform);
//        headText.text = headValue.ToString();
//    }

//    private void Update()
//    {
//        if (!Started) return;

//        HandleInput();
//        MoveHead();
//        RecordPosition();
//        MoveSegments();
//    }

//    private void HandleInput()
//    {
//        horizontalInput = ControlFreak2.CF2Input.GetAxis("Mouse X") * horizontalSpeed;
//    }

//    private void MoveHead()
//    {
//        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);


//        float targetX = transform.position.x + horizontalInput * 0.01f; 
//        targetX = Mathf.Clamp(targetX, -horizontalLimit, horizontalLimit);


//        float lerpSpeed = 10f; 
//        float newX = Mathf.Lerp(transform.position.x, targetX, lerpSpeed * Time.deltaTime);

//        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
//    }

//    private void RecordPosition()
//    {
//        positionHistory.Insert(0, transform.position);
//        if (positionHistory.Count > 1000)
//            positionHistory.RemoveAt(positionHistory.Count - 1);
//    }

//    private void MoveSegments()
//    {
//        for (int i = 1; i < segments.Count; i++)
//        {
//            float targetDistance = i * segmentSpacing;
//            Vector3 targetPos = GetPositionFromDistance(targetDistance);

//            segments[i].position = Vector3.Lerp(
//                segments[i].position,
//                targetPos,
//                Time.deltaTime * 20f
//            );
//        }
//    }

//    private Vector3 GetPositionFromDistance(float distance)
//    {
//        float accumulatedDistance = 0f;

//        for (int i = 0; i < positionHistory.Count - 1; i++)
//        {
//            float segmentDist = Vector3.Distance(positionHistory[i], positionHistory[i + 1]);
//            accumulatedDistance += segmentDist;

//            if (accumulatedDistance >= distance)
//                return positionHistory[i + 1];
//        }

//        return positionHistory[positionHistory.Count - 1];
//    }


//    GameObject GetSegmentFromPool()
//    {
//        foreach (GameObject seg in segmentPool)
//        {
//            if (!seg.activeInHierarchy)
//            {
//                seg.SetActive(true);
//                return seg;
//            }
//        }

//        GameObject newSeg = Instantiate(segmentPrefab, segmentsParent);
//        segmentPool.Add(newSeg);
//        return newSeg;
//    }

//    void ReturnAllSegmentsToPool()
//    {
//        for (int i = 1; i < segments.Count; i++)
//            segments[i].gameObject.SetActive(false);

//        segments.Clear();
//        segments.Add(transform);
//    }


//    public void AddNumber(int value)
//    {
//        headValue += value;
//        headText.text = headValue.ToString();

//        RebuildSnake();
//        StartCoroutine(PopAnimation(transform));
//        Handheld.Vibrate();
//    }

//    public void ReduceNumber(int value)
//    {
//        headValue -= value;

//        if (headValue < 1)
//        {
//            headValue = 0;
//            headText.text = headValue.ToString();
//            ReturnAllSegmentsToPool();
//            Handheld.Vibrate();
//            SetLevelStatus(false);
//            return;
//        }

//        headText.text = headValue.ToString();
//        RebuildSnake();
//        StartCoroutine(PopAnimation(transform));
//        Handheld.Vibrate();
//    }

//    void RebuildSnake()
//    {
//        ReturnAllSegmentsToPool();

//        int segmentsToShow = Mathf.Min(headValue - 1, maxVisibleSegments);

//        for (int i = 0; i < segmentsToShow; i++)
//        {
//            GameObject seg = GetSegmentFromPool();
//            seg.transform.SetParent(segmentsParent);
//            seg.transform.position = transform.position;
//            seg.transform.rotation = Quaternion.identity;

//            int numberValue = headValue - (i + 1);
//            seg.GetComponent<SnakeSegment>().SetNumber(numberValue);

//            segments.Add(seg.transform);
//        }
//    }


//    public void DrainToOneFast(float stepDelay = 0.02f)
//    {
//        StartCoroutine(DrainToOneCoroutine(stepDelay));
//    }

//    private IEnumerator DrainToOneCoroutine(float stepDelay)
//    {
//        while (headValue > 1)
//        {
//            headValue--;
//            headText.text = headValue.ToString();


//            if (segments.Count > 1)
//            {
//                segments[segments.Count - 1].gameObject.SetActive(false);
//                segments.RemoveAt(segments.Count - 1);
//            }

//            yield return new WaitForSeconds(stepDelay);
//        }

//        headValue = 1;
//        headText.text = "1";
//    }




//    public void SetLevelStatus(bool endValue)
//    {
//        this.enabled = false;
//        FindObjectOfType<GameController>().LevelEnd(endValue);
//    }

//    private IEnumerator PopAnimation(Transform target)
//    {
//        Vector3 originalScale = target.localScale;
//        Vector3 targetScale = originalScale * 1.3f;
//        float duration = 0.1f;

//        float t = 0;
//        particleEffect.SetActive(true);
//        while (t < duration)
//        {
//            t += Time.deltaTime;
//            target.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
//            yield return null;
//        }

//        t = 0;
//        while (t < duration)
//        {
//            t += Time.deltaTime;
//            target.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
//            yield return null;
//        }

//        target.localScale = originalScale;
//    }
//}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSnake : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 8f;

    [Header("Horizontal Movement")]
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float horizontalLimit = 4f;

    [Header("Head Settings")]
    public int headValue = 1;
    [SerializeField] private TextMeshPro headText;

    [Header("Segment Settings")]
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private Transform segmentsParent;
    [SerializeField] private float segmentSpacing = 0.3f;
    [SerializeField] private int maxVisibleSegments = 25;

    [Header("Collision Particle")]
    [SerializeField] private GameObject particleEffect;

    private List<Transform> segments = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();
    private List<GameObject> segmentPool = new List<GameObject>();

    private float horizontalInput;
    private bool hasStarted = false;
    public bool Started { get => hasStarted; set => hasStarted = value; }

    private void Start()
    {
#if UNITY_EDITOR
        horizontalSpeed = 100f;
#endif
        segments.Add(transform);
        headText.text = headValue.ToString();
    }

    private void Update()
    {
        if (!Started) return;

        HandleInput();
        MoveHead();
        RecordPosition();
        MoveSegments();
    }

    private void HandleInput()
    {
        horizontalInput = ControlFreak2.CF2Input.GetAxis("Mouse X") * horizontalSpeed;
    }

    private void MoveHead()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

        float targetX = transform.position.x + horizontalInput * 0.01f;
        targetX = Mathf.Clamp(targetX, -horizontalLimit, horizontalLimit);

        float lerpSpeed = 10f;
        float newX = Mathf.Lerp(transform.position.x, targetX, lerpSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void RecordPosition()
    {
        positionHistory.Insert(0, transform.position);
        if (positionHistory.Count > 1000)
            positionHistory.RemoveAt(positionHistory.Count - 1);
    }

    private void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            float targetDistance = i * segmentSpacing;
            Vector3 targetPos = GetPositionFromDistance(targetDistance);

            segments[i].position = Vector3.Lerp(
                segments[i].position,
                targetPos,
                Time.deltaTime * 20f
            );
        }
    }

    private Vector3 GetPositionFromDistance(float distance)
    {
        float accumulatedDistance = 0f;

        for (int i = 0; i < positionHistory.Count - 1; i++)
        {
            float segmentDist = Vector3.Distance(positionHistory[i], positionHistory[i + 1]);
            accumulatedDistance += segmentDist;

            if (accumulatedDistance >= distance)
                return positionHistory[i + 1];
        }

        return positionHistory[positionHistory.Count - 1];
    }

    GameObject GetSegmentFromPool()
    {
        foreach (GameObject seg in segmentPool)
        {
            if (!seg.activeInHierarchy)
            {
                seg.SetActive(true);
                return seg;
            }
        }

        GameObject newSeg = Instantiate(segmentPrefab, segmentsParent);
        segmentPool.Add(newSeg);
        return newSeg;
    }

    void ReturnAllSegmentsToPool()
    {
        for (int i = 1; i < segments.Count; i++)
            segments[i].gameObject.SetActive(false);

        segments.Clear();
        segments.Add(transform);
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

        if (headValue < 1)
        {
            headValue = 0;
            headText.text = headValue.ToString();
            ReturnAllSegmentsToPool();
            Handheld.Vibrate();
            SetLevelStatus(false);
            return;
        }

        headText.text = headValue.ToString();
        RebuildSnake();
        StartCoroutine(PopAnimation(transform));
        Handheld.Vibrate();
    }

    void RebuildSnake()
    {
        ReturnAllSegmentsToPool();

        int segmentsToShow = Mathf.Min(headValue - 1, maxVisibleSegments);

        for (int i = 0; i < segmentsToShow; i++)
        {
            GameObject seg = GetSegmentFromPool();
            seg.transform.SetParent(segmentsParent);
            seg.transform.position = transform.position;
            seg.transform.rotation = Quaternion.identity;

            int numberValue = headValue - (i + 1);
            seg.GetComponent<SnakeSegment>().SetNumber(numberValue);

            segments.Add(seg.transform);
        }
    }

    public void DrainToOneFast(float stepDelay = 0.02f)
    {
        StartCoroutine(DrainToOneCoroutine(stepDelay));
    }

    private IEnumerator DrainToOneCoroutine(float stepDelay)
    {
        while (headValue > 1)
        {
            headValue--;
            headText.text = headValue.ToString();

            if (segments.Count > 1)
            {
                segments[segments.Count - 1].gameObject.SetActive(false);
                segments.RemoveAt(segments.Count - 1);
            }

            yield return new WaitForSeconds(stepDelay);
        }

        headValue = 1;
        headText.text = "1";
    }

    public void SetLevelStatus(bool endValue)
    {
        this.enabled = endValue;
        FindObjectOfType<GameController>().LevelEnd(endValue);
    }

    private IEnumerator PopAnimation(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * 1.3f;
        float duration = 0.1f;

        if (particleEffect != null) particleEffect.SetActive(true);

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            yield return null;
        }

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
