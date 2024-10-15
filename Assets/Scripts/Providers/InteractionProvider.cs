using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using Util;

public static class InteractionProvider
{
    public static Interaction FailInteraction = new Interaction()
    {
        InteractionType = InteractionType.Fail,
    };

    public static Interaction VictoryInteraction = new Interaction()
    {
        InteractionType = InteractionType.Victory,
    };
}