using MiniGStudio;
using UnityEditor.Overlays;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private float                   elapsedTime;
    private bool                    kinematicChanged = false;
    private Rigidbody               rb;

    private GolemRockHowlState _rockHowlState;

    public void Start()
    {
        elapsedTime = 0f;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (IsExpired())
        {
            GameObject.Destroy(gameObject);
            _rockHowlState.Rocks.Remove(this);
            _rockHowlState.rockPositions.Remove(transform.position);
        }
        else if (elapsedTime < _rockHowlState.Desc.ElevationDuration)
        {
            transform.position += Vector3.up * 0.005f;
        }
        else if (elapsedTime >= _rockHowlState.Desc.ElevationDuration && !kinematicChanged)
        {
            ChangeKinematicState(false);
        }
    }

    public void BindWithGolem(GolemRockHowlState rockHowlState) => _rockHowlState = rockHowlState;

    public bool IsExpired()
    {
        return elapsedTime >= _rockHowlState.Desc.RockLifespan;
    }

    public void ChangeKinematicState(bool state)
    {
        rb.isKinematic = state;
        kinematicChanged = true;
    }
}