using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : UnitLoader
{
    private void Awake()
    {
        if (unitToLoadOnStart is not Player)
        {
            Debug.LogWarning("PlayerLoader tried to load a non-player unit.");
            return;
        }

        if (Player.instance != null)
        {
            Debug.LogWarning("Tried to load multiple players.");
            return;
        }
        Player.instance = Instantiate((Player) unitToLoadOnStart);
    }

    public override Unit LoadNewUnit(Unit unit)
    {
        loadedUnit = Player.instance;
        return base.LoadNewUnit(unit);
    }
}
