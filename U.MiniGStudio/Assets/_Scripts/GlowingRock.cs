using MiniGStudio;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class GlowingRock : Rock
{
    public override void OnCollisionEnter(Collision collider)
    {
        if (!Thrown) return;
        base.OnCollisionEnter(collider);
        InstantiateBomb();
        Destroy(gameObject);
    }

    private void InstantiateBomb() {
        BombManager bomb = Instantiate(_desc.BombPrefab, transform.position + Vector3.up * 3f, Quaternion.identity);
    }
}