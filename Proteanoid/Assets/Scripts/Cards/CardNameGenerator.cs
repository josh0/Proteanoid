using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardNameGenerator
{
    private static Dictionary<(int, int), string> actionNameMap = new()
    {
        //Attack only
        {(1,0), "Slash" },
        {(2,0), "Assault" },
        {(2,1), "Assault" },
        {(3,0), "Blitz" },
        {(3,1), "Blitz" },
        {(4,0), "Onslaught"},

        //Block only
        {(0,1), "Defend"},
        {(0,2), "Barricade" },
        {(1,2), "Barricade" },
        {(0,3), "Shelter" },
        {(1,3), "Shelter" },
        {(0,4), "Fortress" },

        //Mixtures
        {(1,1), "Riposte" },
        {(2,2), "Repulse" },
    };

    public static string GetName(List<UnitAction> actions)
    {
        if (actions.OfType<AssimilateAction>().ToList().Count > 0)
            return "Assimilate";

        actionNameMap.TryGetValue((actions.OfType<AttackAction>().Count(), actions.OfType<BlockAction>().Count()), out string noun);

        return GetAdjective(actions) + " " + noun;

    }

    private static string GetAdjective(List<UnitAction> actions)
    {
        StatusEffect effectWithMostStacks = null;
        int effectStacks = 0;
        foreach (UnitAction action in actions)
            if (action.appliedEffectStacks > effectStacks)
            {
                effectWithMostStacks = action.appliedEffect;
                effectStacks = action.appliedEffectStacks;
            }

        if (effectWithMostStacks == null)
            return null;
        else
            return effectWithMostStacks.adjective;
    }
}
