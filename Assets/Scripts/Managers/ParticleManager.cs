using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleSystem = UnityEngine.ParticleSystem;

namespace Managers
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance;

        private readonly Dictionary<GameObject, ParticleSystem> _particleSystemMap =
            new Dictionary<GameObject, ParticleSystem>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayParticle(GameObject particlePrefab, Transform parent)
        {
            if (_particleSystemMap.ContainsKey(particlePrefab) == false)
            {
                CreateInstanceFromParticleSystem(particlePrefab, parent);
            }

            PlayParticleAsync(_particleSystemMap[particlePrefab], parent);
        }

        // Asynchronous method to play a particle system
        private void PlayParticleAsync(ParticleSystem particleSys, Transform parent)
        {
            StartCoroutine(PlayParticleCoroutine(particleSys, parent));
        }

        private IEnumerator PlayParticleCoroutine(ParticleSystem particleSys, Transform parent)
        {
            if (particleSys != null)
            {
                // Activate and play the particle system
                particleSys.gameObject.transform.position = parent.position;
                particleSys.gameObject.SetActive(true);
                particleSys.Play();
                // Wait until the particle system has finished playing
                while (particleSys != null && particleSys.isPlaying)
                {
                    yield return null; // Continue to wait until the next frame
                }

                // Deactivate the particle system after it finishes
                particleSys.gameObject.SetActive(false);
            }
        }

        private void CreateInstanceFromParticleSystem(GameObject particlePrefab, Transform parent)
        {
            var ps = Instantiate(particlePrefab, parent).GetComponent<ParticleSystem>();
            ps.gameObject.SetActive(false);

            _particleSystemMap.Add(particlePrefab, ps);
        }
    }
}