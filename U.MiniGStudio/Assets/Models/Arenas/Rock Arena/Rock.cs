using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float  lifeSpan;
    private float                   elapsedTime;
    private bool                    kinematicChanged = false;
    private Rigidbody               rb;

    public void InitializeRock()
    {
        elapsedTime = 0f;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void UpdateRock()
    {
        elapsedTime += Time.deltaTime;
    }

    public bool IsExpired()
    {
        return elapsedTime >= lifeSpan;
    }

    public bool IsKinematicChanged()
    {
        return kinematicChanged;
    }

    public void ChangeKinematicState(bool state)
    {
        rb.isKinematic = state;
        kinematicChanged = true;
    }

    public float getElapsedTime()
    {
        return elapsedTime;
    }
}