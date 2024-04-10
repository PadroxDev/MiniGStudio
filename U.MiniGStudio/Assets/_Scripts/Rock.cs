using MiniGStudio;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Rock : MonoBehaviour
{
    private float                   elapsedTime;
    private bool                    kinematicChanged = false;
    private Rigidbody               rb;
    private VisualEffect _vfx;
    private float _duration;
    private Vector3 _start;
    private Vector3 _destination;
    public bool Thrown;

    private GolemRockHowlState _rockHowlState;
    private GolemRockHowlState.Descriptor _desc;
    private Renderer _renderer;
    private Material _mat;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
        _mat = _renderer.material;
        elapsedTime = 0f;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        float variation = Random.Range(-_desc.DurationVariation, _desc.DurationVariation);
        _duration = _desc.ElevationDuration + _desc.ElevationDuration * variation;
        _start = transform.position;
        _destination = transform.position + Vector3.up * (_desc.ElevationHeight + 0.5f);
        _vfx = (VisualEffect)Instantiate(_desc.GroundSmokeVFX, _destination, Quaternion.identity, Helpers.VFXParent);
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
        else if (elapsedTime < _duration)
        {
            float alpha = elapsedTime / _duration;
            transform.position = Vector3.Lerp(_start, _destination, alpha);
        }
        else if (elapsedTime >= _duration && !kinematicChanged)
        {
            ChangeKinematicState(false);
            _vfx.Stop();
            Destroy(_vfx, 3);
        }

        if(Thrown)
        {
            if(rb.velocity == Vector3.zero)
            {
                StartCoroutine(DissolveRock());
            }
        }
    }

    public void BindWithGolem(GolemRockHowlState rockHowlState) {
        _rockHowlState = rockHowlState;
        _desc = rockHowlState.Desc;
    }

    public bool IsExpired()
    {
        return elapsedTime >= _desc.RockLifespan;
    }

    public void ChangeKinematicState(bool state)
    {
        rb.isKinematic = state;
        kinematicChanged = true;
    }

    private float _dissolveTimer = 0;
    private IEnumerator DissolveRock()
    {
        while(_dissolveTimer < _desc.DissolveDuration)
        {
            _dissolveTimer += Time.deltaTime;
            float a = Mathf.Clamp01(_dissolveTimer / _desc.DissolveDuration);
            _mat.SetFloat("DissolveAmount", a);
            yield return null;
        }

        Destroy(gameObject);
    }
}