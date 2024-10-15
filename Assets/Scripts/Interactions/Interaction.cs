using System;
using UnityEngine;
using Util;

namespace Interactions
{
    [Serializable]
    public class Interaction
    {
        [SerializeField] private InteractionType interactionType;
        [SerializeField] private float amount;

        public InteractionType InteractionType
        {
            get => interactionType;
            set => interactionType = value;
        }

        public float Amount
        {
            get => amount;
            set => amount = value;
        }
    }
}