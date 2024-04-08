using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnRocks : MonoBehaviour
{
    [SerializeField] private GameObject RockPrefab;
    [SerializeField] private int NumberOfRocks;
    [SerializeField] private float SpawnRadius;
    [SerializeField] private Vector3 Center;
    [SerializeField] private float RockDistance;

    private List<Rock> Rocks = new List<Rock>();

    private List<Vector3> rockPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitiateRocks();
        }

        for (int i = Rocks.Count - 1; i >= 0; i--)
        {
            Rock rock = Rocks[i];
            rock.UpdateRock();
            if (rock.IsExpired())
            {
                Destroy(rock.gameObject);
                Rocks.RemoveAt(i);
                rockPositions.RemoveAt(i);
            }
            else if (rock.getElapsedTime() < 4f)
            {
                Vector3 rockPos = rock.transform.position;
                rockPos.y += 0.005f;
                rock.transform.position = rockPos;
            }
            else if (rock.getElapsedTime() >= 4f && !rock.IsKinematicChanged())
            {
                rock.ChangeKinematicState(false);
            }
        }
    }

    private void InitiateRocks()
    {
        for (int i = 0; i < NumberOfRocks; i++)
        {
            Vector3 randomPos = RandomPointInCircle(Center, SpawnRadius);
            if (IsPositionValid(randomPos))
            {
                GameObject newRockObject = Instantiate(RockPrefab, randomPos, Quaternion.identity);
                rockPositions.Add(randomPos);
                Rock newRock = newRockObject.GetComponent<Rock>();
                newRock.InitializeRock();
                Rocks.Add(newRock);
            }
        }
    }

    private Vector3 RandomPointInCircle(Vector3 center, float radius)
    {
        float angle = Random.value * Mathf.PI * 2;
        float x = center.x + Mathf.Sin(angle) * radius;
        float z = center.z + Mathf.Cos(angle) * radius;
        return new Vector3(x, center.y, z);
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 rockPos in rockPositions)
        {
            if (Vector3.Distance(position, rockPos) < RockDistance) 
            {
                return false;
            }
        }
        return true;
    }
}