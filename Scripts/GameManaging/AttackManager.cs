using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class AttackManager
    {
        public static void MeleeAllStrike(Entity attacker, Entity target)
        {
            foreach (EquipmentSlot slot in attacker.GetComponent<BodyPlot>().bodyPlot) { if (slot.item != null) { 
                    if (slot.item.GetComponent<AttackFunction>() != null && slot.item.GetComponent<AttackFunction>().weaponType == "Melee") { Attack(attacker, target, slot.item); } } }
        }
        public static void Attack(Entity attacker, Entity target, Entity weapon)
        {
            if (attacker != null && target != null && weapon != null)
            {
                AttackFunction attackFunction = weapon.GetComponent<AttackFunction>();

                if (CMath.random.Next(0, 20) + attackFunction.toHitModifier >= target.GetComponent<Stats>().ac)
                {
                    int dmg = 0;
                    for (int d = 0; d < attackFunction.die1; d++) { dmg += CMath.random.Next(1, attackFunction.die2); }
                    dmg += attackFunction.dmgModifier;

                    SpecialComponentManager.TriggerOnHit(weapon, attacker, target, dmg, attackFunction.dmgType, true);
                    SpecialComponentManager.TriggerOnHit(attacker, attacker, target, dmg, null, true);
                    target.GetComponent<OnHit>().Hit(dmg, attackFunction.dmgType, weapon.GetComponent<Description>().name, attacker);
                }
                else { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " missed with the " + weapon.GetComponent<Description>().name + "."); }
                attacker.GetComponent<TurnFunction>().EndTurn();
            }
        }
    }
}
