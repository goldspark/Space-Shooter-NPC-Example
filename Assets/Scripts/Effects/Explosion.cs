using Assets.Scripts.Entities;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Ship owner;
    private void OnParticleSystemStopped()
    {
        Destroy(owner.gameObject);
    }
}
