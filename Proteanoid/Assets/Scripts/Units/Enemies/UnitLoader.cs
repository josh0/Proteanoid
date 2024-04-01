using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitLoader : MonoBehaviour
{
    [SerializeField] protected Unit unitToLoadOnStart;
    public Unit loadedUnit { get; protected set; }
    [SerializeField] private EnemyVfx vfx;
    private void Start()
    {
        if (unitToLoadOnStart != null)
            LoadNewUnit(unitToLoadOnStart);
    }

    /// <summary>The script controlling this unit's movement.</summary>
    public UnitMovement movement { get; private set; }

    [field: SerializeField] public HPSlider hpSlider { get; private set; }
    [SerializeField] private ActionDescription intentDescription;
    [SerializeField] private ShakeMovement graphicShaker;
    [SerializeField] private Image graphicRenderer;

    public void UpdateIntentIcon(UnitAction intent)
    {
        intentDescription.SetDescription(intent);
    }

    public virtual Unit LoadNewUnit(Unit unit)
    {
        gameObject.SetActive(true);

        if (this is not PlayerLoader)
        {
            loadedUnit = Instantiate(unit);
            FightManager.enemies.Add((Enemy) loadedUnit);
        }

        vfx.baseUnitClass = loadedUnit;
        loadedUnit.loader = this;

        //hp stuff
        loadedUnit.hp = loadedUnit.maxHp;
        hpSlider.SetMaxHPVal(loadedUnit.maxHp);
        hpSlider.SetHPVal(loadedUnit.hp);
        hpSlider.SetBlockVal(loadedUnit.block);

        //vfx stuff
        graphicRenderer.sprite = loadedUnit.sprite;

        return loadedUnit;
    }
}
