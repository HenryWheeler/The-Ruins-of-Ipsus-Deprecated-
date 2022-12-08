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
                    if (slot.item.GetComponent<AttackFunction>() != null && slot.item.GetComponent<AttackFunction>().weaponType == "Melee") { Attack(attacker, target, slot.item, false); } } }
            attacker.GetComponent<TurnFunction>().EndTurn();
        }
        public static void ThrowWeapon(Entity attacker, Entity target, Entity weapon)
        {
            if (TargetReticle.ReturnFire(attacker, target, attacker.GetComponent<Stats>().strength) != null)
            {
                Coordinate targetCoordinate = TargetReticle.ReturnFire(attacker, target, attacker.GetComponent<Stats>().strength);
                InventoryManager.PlaceItem(targetCoordinate, weapon);

                List<Coordinate> coordinates = RangeModels.ReturnLine(attacker.GetComponent<Coordinate>(), weapon.GetComponent<Coordinate>());
                int current = 0;
                Draw itemFrame = weapon.GetComponent<Draw>();
                Draw baseFrame = new Draw("Gray", "Black", '.');
                Draw finalFrame = new Draw("Yellow", "Black", 'X');
                List<SFX> sfx = new List<SFX>();
                foreach(Coordinate coordinate in coordinates)
                {
                    Draw[] frames = new Draw[coordinates.Count()];
                    if (current != coordinates.Count - 1)
                    {
                        for (int i = 0; i < coordinates.Count(); i++)
                        {
                            if (i == current) { frames[i] = itemFrame; }
                            else { frames[i] = baseFrame; }
                        }
                    } else { for (int i = 0; i < coordinates.Count(); i++) { frames[i] = finalFrame; } }
                    SFX sFX;
                    if (current == 0) { sFX = new SFX(coordinate.x, coordinate.y, itemFrame.character, itemFrame.fColor, itemFrame.bColor, false, true); }
                    else { sFX = new SFX(coordinate.x, coordinate.y, '.', "Gray", "Black", false, true); }
                    sFX.AddComponent(new AnimationFunction(frames));
                    sfx.Add(sFX);
                    current++;
                }
                Renderer.StartAnimationThread(sfx, coordinates.Count(), 250/coordinates.Count());

                if (weapon.GetComponent<Throwable>() != null)
                { weapon.GetComponent<Throwable>().Throw(attacker, targetCoordinate); }
                else 
                { target = Map.map[targetCoordinate.x, targetCoordinate.y].actor; Attack(attacker, target, weapon, false); }
                InventoryManager.RemoveFromInventory(attacker, weapon);
                PronounSet pronounSet = attacker.GetComponent<PronounSet>();
                if (pronounSet.present) { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " has thrown " + pronounSet.possesive + " " + weapon.GetComponent<Description>().name + "!"); }
                { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " have thrown " + pronounSet.possesive + " " + weapon.GetComponent<Description>().name + "!"); }
                attacker.GetComponent<TurnFunction>().EndTurn();
            }
            else if (target != null) { attacker.GetComponent<TurnFunction>().EndTurn(); }
        }
        public static void Attack(Entity attacker, Entity target, Entity weapon, bool endTurn = true)
        {
            if (attacker != null && target != null && weapon != null)
            {
                AttackFunction attackFunction;
                if (weapon.GetComponent<AttackFunction>() != null) { attackFunction = weapon.GetComponent<AttackFunction>(); }
                else { attackFunction = new AttackFunction(1, 1, 0, 0, "Blunt", "Melee"); }

                if (CMath.random.Next(0, 20) + attackFunction.toHitModifier >= target.GetComponent<Stats>().ac)
                {
                    int dmg = 0;
                    for (int d = 0; d < attackFunction.die1; d++) { dmg += CMath.random.Next(1, attackFunction.die2); }
                    dmg += attackFunction.dmgModifier;

                    SpecialComponentManager.TriggerOnHit(weapon, attacker, target, dmg, attackFunction.dmgType, true);
                    SpecialComponentManager.TriggerOnHit(attacker, attacker, target, dmg, null, true);
                    target.GetComponent<OnHit>().Hit(dmg, attackFunction.dmgType, weapon.GetComponent<Description>().name, attacker);
                }
                else { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " missed with " + attacker.GetComponent<PronounSet>().possesive
                    + " " + weapon.GetComponent<Description>().name + "."); }
                if (endTurn) { attacker.GetComponent<TurnFunction>().EndTurn(); }
            }
        }
        public static void EffectAttack(Entity attacker, Entity target, AttackFunction attackFunction, string attackName)
        {
            if (attacker != null && target != null && attackFunction != null)
            {
                if (CMath.random.Next(0, 20) + attackFunction.toHitModifier >= target.GetComponent<Stats>().ac)
                {
                    int dmg = 0;
                    for (int d = 0; d < attackFunction.die1; d++) { dmg += CMath.random.Next(1, attackFunction.die2); }
                    dmg += attackFunction.dmgModifier;
                    target.GetComponent<OnHit>().Hit(dmg, attackFunction.dmgType, attackName, attacker);
                }
                else { Log.AddToStoredLog("The " + attackName + " missed its target."); }
            }
        }
    }
}
