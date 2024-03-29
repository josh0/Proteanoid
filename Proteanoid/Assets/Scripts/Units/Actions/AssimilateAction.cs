using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Assimilate")]
public class AssimilateAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, List<Unit> targets)
    {
        foreach(Unit target in targets)
            if (target is Enemy enemy)
            {
                Card card = ScriptableObject.CreateInstance<Card>();
                card.actions.Add(Instantiate(enemy.intent));
                card.manaCost = 1;
                Debug.Log("Come fix the mana cost please");
                CardManager.Instance.AddCardToHand(card);
                Player.instance.AddCardToDeck(card);
            }
        yield return new WaitForSeconds(0.2f);
    }
}
