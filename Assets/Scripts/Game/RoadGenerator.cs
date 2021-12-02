using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [Header("Road Generation Data")]
    [SerializeField] private GameObject roadPartPrefab;
    [SerializeField] private float roadPartLength = 10f;
    [SerializeField] private int roadPartAmount = 15;
    [SerializeField] private float replacementDistance = 5f;

    private Queue<GameObject> roadParts = new Queue<GameObject>();


    private void Start()
    {
        // Event subscription
        GameController.Instance.OnLevelReset += ResetRoad;

        // Instantiate road parts in accurate positions to build the road,
        // then add them to the queue
        for (int i = 0; i < roadPartAmount; i++)
        {
            GameObject roadPart = Instantiate(roadPartPrefab, transform.position + Vector3.forward * roadPartLength * (i - 1), Quaternion.identity, transform);
            roadParts.Enqueue(roadPart);
        }
    }

    private void Update()
    {
        // When player rides beyond the road part, 
        // put it to the front of the road
        // and put it at the end of roadPart queue
        if (playerTransform.position.z - roadParts.Peek().transform.position.z > replacementDistance)
        {
            GameObject roadPart = roadParts.Dequeue();
            roadPart.transform.SetPosZ(roadPart.transform.position.z + (roadPartAmount - 1) * roadPartLength);
            roadParts.Enqueue(roadPart);
        }
    }


    // OnLevelReset event
    private void ResetRoad()
    {
        // Set road parts' positions to the start of level
        for (int i = 0; i < roadPartAmount; i++)
        {
            GameObject roadPart = roadParts.Dequeue();
            roadPart.transform.SetPosZ(transform.position.z + roadPartLength * (i - 1));
            roadParts.Enqueue(roadPart);
        }
    }
}
