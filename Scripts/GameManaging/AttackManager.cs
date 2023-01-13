using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class AttackManager
    {
        public static void MeleeAllStrike(Entity attacker, Entity target)
        {
            if (attacker.GetComponent<BodyPlot>().ReturnSlot("Weapon").item == null) 
            { 
                Attack(attacker, target, new Entity(new List<Component>() 
                { 
                    new AttackFunction("1-1-1-0-0", "Bludgeoning"), 
                    new Description("Fists", "Fists") 
                })); 
            }
            else 
            {
                Attack(attacker, target, attacker.GetComponent<BodyPlot>().ReturnSlot("Weapon").item);
            }
        }
        public static void ThrowWeapon(Entity attacker, Coordinate target, Entity weapon)
        {
            try
            {
                int time = CMath.Distance(attacker.GetComponent<Coordinate>(), target);
                Entity particle2 = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            new Draw("Yellow", "Black", 'X'),
                            new ParticleComponent(time, 2, "None", 0, new Draw[] { new Draw("Yellow", "Black", 'X'), new Draw("Black", "Black", 'X') }),
                        });
                Renderer.AddParticle(target.vector2.x, target.vector2.y, particle2);

                Entity particle = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            weapon.GetComponent<Draw>(),
                            new ParticleComponent(time, 2, "Target", 0, new Draw[] { weapon.GetComponent<Draw>() }, target.vector2, true),
                        });
                Vector2 vector2 = attacker.GetComponent<Coordinate>().vector2;
                Renderer.AddParticle(vector2.x, vector2.y, particle);

                //while (Renderer.playingAnimation)
                {
                    
                }

                if (weapon.GetComponent<Equippable>() != null && weapon.GetComponent<Equippable>().equipped)
                {
                    InventoryManager.UnequipItem(attacker, weapon);
                }
                InventoryManager.PlaceItem(target, weapon);

                if (weapon.GetComponent<Throwable>() != null)
                {
                    weapon.GetComponent<Throwable>().Throw(attacker, target);
                }
                if (World.GetTraversable(target.vector2).actorLayer != null)
                {
                    Attack(attacker, World.GetTraversable(target.vector2).actorLayer, weapon, false);
                }

                InventoryManager.RemoveFromInventory(attacker, weapon);
                PronounSet pronounSet = attacker.GetComponent<PronounSet>();
                if (pronounSet.present)
                {
                    Log.AddToStoredLog(attacker.GetComponent<Description>().name + " has thrown " + pronounSet.possesive + " " + weapon.GetComponent<Description>().name + "!");
                }
                else
                {
                    Log.AddToStoredLog(attacker.GetComponent<Description>().name + " have thrown " + pronounSet.possesive + " " + weapon.GetComponent<Description>().name + "!");
                }
                attacker.GetComponent<TurnFunction>().EndTurn();
            }
            catch (ArgumentNullException e)
            {
                Log.Add($"Cannot throw because {e.Message} is null");
                attacker.GetComponent<TurnFunction>().EndTurn();
            }
        }
        public static void Attack(Entity attacker, Entity target, Entity weapon, bool endTurn = true)
        {
            if (attacker != null && target != null && weapon != null && weapon.GetComponent<AttackFunction>() != null)
            {
                AttackFunction attackFunction = weapon.GetComponent<AttackFunction>();
                string[] parts = attackFunction.details.Split('-');
                int numberOfAttacks = int.Parse(parts[0]);

                for (int i = 0; i < numberOfAttacks; i++)
                {
                    if (World.random.Next(0, 20) + int.Parse(parts[4]) + attacker.GetComponent<Stats>().strength >= target.GetComponent<Stats>().ac)
                    {
                        int dmg = 0;
                        for (int d = 0; d < int.Parse(parts[1]); d++)
                        {
                            dmg += World.random.Next(1, int.Parse(parts[2]));
                        }
                        dmg += int.Parse(parts[3]);
                        SpecialComponentManager.TriggerOnHit(weapon, attacker, target, dmg, attackFunction.dmgType, true);
                        SpecialComponentManager.TriggerOnHit(attacker, attacker, target, dmg, null, true);
                        target.GetComponent<OnHit>().Hit(dmg, attackFunction.dmgType, weapon.GetComponent<Description>().name, attacker);
                    }
                    else
                    {
                        Log.Add($"{attacker.GetComponent<Description>().name} missed with {attacker.GetComponent<PronounSet>().possesive} {weapon.GetComponent<Description>().name}.");
                    }
                }
                if (endTurn)
                { 
                    attacker.GetComponent<TurnFunction>().EndTurn(); 
                }
            }
            else
            {
                                    attacker.GetComponent<TurnFunction>().EndTurn(); 
            }
        }
        public static void Attack(Entity attacker, Entity target, AttackFunction attackFunction, string attackName)
        {
            try
            {
                if (attacker != null && target != null && attackFunction != null)
                {
                    string[] parts = attackFunction.details.Split('-');
                    int numberOfAttacks = int.Parse(parts[0]);

                    for (int i = 0; i < numberOfAttacks; i++)
                    {
                        int dmg = 0;
                        for (int d = 0; d < int.Parse(parts[1]); d++)
                        {
                            dmg += World.random.Next(1, int.Parse(parts[2]));
                        }
                        dmg += int.Parse(parts[3]);
                        target.GetComponent<OnHit>().Hit(dmg, attackFunction.dmgType, attackName, attacker);
                    }
                }
                else
                {
                    if (attacker == null)
                    {
                        throw new ArgumentNullException(paramName: nameof(attacker), message: "Attacker cannot be null");
                    }
                    else if (target == null)
                    {
                        throw new ArgumentNullException(paramName: nameof(target), message: "Target cannot be null");
                    }
                    else if (attackFunction == null)
                    {
                        throw new ArgumentNullException(paramName: nameof(attackFunction), message: "AttackFunction cannot be null");
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Log.Add($"Cannot attack because {e.Message} is null");
                attacker.GetComponent<TurnFunction>().EndTurn();
            }
        }
    }
}
