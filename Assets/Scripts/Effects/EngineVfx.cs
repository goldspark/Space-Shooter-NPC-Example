using Assets.Scripts.Entities;
using UnityEngine;

public class EngineVfx : MonoBehaviour
{

    

    public float minLifeTime = 0.3f;
    public float maxLifeTime = 0.75f;
    public float minSize = 1f;
    public float maxSize = 1.8f;
    private ParticleSystem _particleSystem;
    private Ship _owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _owner = transform.parent.GetComponent<Ship>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float shipSpeedPercent = _owner.CurrentSpeed / _owner.MaxSpeed;
        var main = _particleSystem.main;
        main.startLifetime = minLifeTime + (shipSpeedPercent * (maxLifeTime - minLifeTime));
        main.startSize = minSize +( shipSpeedPercent * (maxSize - minSize));
    }
}
