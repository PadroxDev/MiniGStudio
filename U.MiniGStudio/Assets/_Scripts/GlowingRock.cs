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
        Destroy(gameObject);
    }
}