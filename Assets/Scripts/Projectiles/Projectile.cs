using Assets.Scripts.Entities;
using AudioSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector]   
        public Transform spawnLocation;
        public GameObject muzzlePrefab, hitPrefab, beam, projectile;
        public TrailRenderer trail;
        [SerializeField] SoundData _soundData;
        [HideInInspector]
        public Ship owner;
        public float speed = 500.0f;
        public float hullDamage = 100.0f;
        public float shieldDamage = 20.0f;

        private float _cT = 0f;

        private bool _isDestroyed = false;

        public float range = 1000; // 1km
        private Vector3 _spawnLocation = Vector3.zero;

        private SphereCollider _collider;

        private void Awake()
        {
            
        }

        private void Start()
        {
            if (owner.IsPlayer)
            {
                _collider = GetComponent<SphereCollider>();
                _collider.radius *= 10f;
                _soundData.priority = 0;
                _soundData.volume = 0.05f;
            }

            SoundManager.Instance.CreateSoundBuilder()
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play(_soundData);

            _spawnLocation = transform.position;
            //From video by Gabriel Aguiar Prod. Unity 2018 - Game VFX - Projectile/Bullet Raycast Tutorial
            if(muzzlePrefab != null )
            {
                var muzzleVfx = Instantiate(muzzlePrefab, transform.position, Quaternion.identity, spawnLocation);
                muzzleVfx.transform.forward = transform.forward;

                var psMuzzle = muzzleVfx.GetComponent<ParticleSystem>();
                if(psMuzzle != null )
                    Destroy(muzzleVfx, psMuzzle.main.duration);
                else
                {
                    var psChild = muzzleVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(muzzleVfx, psChild.main.duration);
                }
            }



        }

        private void Update()
        {

            if(_isDestroyed)
            {
                beam.gameObject.SetActive(false);
                projectile.gameObject.SetActive(false);
                speed = 0f;
                hullDamage = 0f;
                shieldDamage = 0f;
                _cT += Time.deltaTime;
                if (_cT > trail.time)
                    Destroy(gameObject);
                  
                return;
            }


            if ((transform.position - _spawnLocation).sqrMagnitude > range * range)
               _isDestroyed = true;
        }

        private void FixedUpdate()
        {
            transform.position += transform.forward * speed * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Ship ship = collision.collider.gameObject.GetComponent<Ship>();
            if (ship == null)
                return;           

            ContactPoint contact = collision.GetContact(0);
            Vector3 pos = contact.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            if (hitPrefab != null)
            {
                var hitVfx = Instantiate(hitPrefab, pos, rot);
                var psHit = hitVfx.GetComponent<ParticleSystem>();
                if (psHit != null)
                    Destroy(hitVfx, psHit.main.duration);
                else
                {
                    var psChild = hitVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVfx, psChild.main.duration);
                }
            }
            _isDestroyed = true;
            ship.currentHp -= hullDamage;

        }
}


}
