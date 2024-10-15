using System;
using System.Collections.Generic;
using GridBase;
using JetBrains.Annotations;
using Managers;
using UnityEngine;

namespace Interactions
{
    public class InteractableItem : MonoBehaviour
    {
        [SerializeField] protected List<Interaction> interactions;
        [CanBeNull] [SerializeField] protected AudioClip interactionSound;
        [CanBeNull] [SerializeField] protected List<GameObject> particleEffectPrefabList;

        public GridCell CurrentCell { get; set; }

        public virtual List<Interaction> OnNextInteraction(Snake.Snake interactingSnake)
        {
            PlaySfxAndParticles();
            return interactions;
        }

        public virtual List<Interaction> OnCurrentInteraction(Snake.Snake interactingSnake)
        {
            return new List<Interaction>();
        }

        private void PlaySfxAndParticles()
        {
            if (interactionSound != null)
            {
                AudioManager.Instance.PlaySfx(interactionSound);
            }

            if (particleEffectPrefabList != null)
            {
                foreach (var particleEffectPrefab in particleEffectPrefabList)
                {
                    ParticleManager.Instance.PlayParticle(particleEffectPrefab, gameObject.transform);
                }
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}