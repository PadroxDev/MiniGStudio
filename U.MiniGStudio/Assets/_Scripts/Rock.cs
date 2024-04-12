using MiniGStudio;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Rock : MonoBehaviour
{
    protected float                   elapsedTime;
    protected bool                    kinematicChanged = false;
    protected Rigidbody               rb;
    protected VisualEffect _vfx;
    protected float _duration;
    protected Vector3 _start;
    protected Vector3 _destination;
    protected VisualEffect _debrisVFX;
    public bool Thrown;

    protected GolemRockHowlState _rockHowlState;
    protected GolemRockHowlState.Descriptor _desc;
    protected Renderer _renderer;
    protected Material _mat;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
        _mat = _renderer.material;
        elapsedTime = 0f;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        _debrisVFX = GetComponentInChildren<VisualEffect>();

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
            if (rb.velocity.magnitude > 0.2f) return;
            StartCoroutine(DissolveRock());
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

    private IEnumerator DissolveRock()
    {
        _debrisVFX.Stop();

        float _dissolveTimer = 0;
        while(_dissolveTimer < _desc.DissolveDuration)
        {
            _dissolveTimer += Time.deltaTime;
            float a = Mathf.Clamp01(_dissolveTimer / _desc.DissolveDuration);
            _mat.SetFloat("_DissolveAmount", a);
            yield return null;
        }

        Destroy(gameObject);
    }

    public void EnableDebris() {
        _debrisVFX.Play();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!Thrown) return;
        Character character = collision.collider.GetComponentInChildren<Character>();
        if (!character) return;
        if (rb.velocity.magnitude < _desc.MinSpeedToDamage) return;
        character.Damage(1.0f);
    }

    private void OnDestroy() {
        _rockHowlState.Rocks.Remove(this);
        _rockHowlState.rockPositions.Remove(transform.position);
    }
}