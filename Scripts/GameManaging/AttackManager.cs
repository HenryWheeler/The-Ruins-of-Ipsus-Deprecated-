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
                if (weapon.GetComponent<Equippable>() != null && weapon.GetComponent<Equippable>().equipped)
                {
                    InventoryManager.UnequipItem(attacker, weapon);
                }
                InventoryManager.PlaceItem(target, weapon);
                {
                    List<Coordinate> coordinates = RangeModels.ReturnLine(attacker.GetComponent<Coordinate>(), weapon.GetComponent<Coordinate>());
                    int current = 0;
                    Draw itemFrame = weapon.GetComponent<Draw>();
                    Draw baseFrame = new Draw("Gray", "Black", '.');
                    Draw finalFrame = new Draw("Yellow", "Black", 'X');
                    List<Entity> sfx = new List<Entity>();
                    foreach (Coordinate coordinate in coordinates)
                    {
                        Draw[] frames = new Draw[coordinates.Count()];
                        if (current != coordinates.Count - 1)
                        {
                            for (int i = 0; i < coordinates.Count(); i++)
                            {
                                if (i == current)
                                {
                                    frames[i] = itemFrame;
                                }
                                else
                                {
                                    frames[i] = baseFrame;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < coordinates.Count(); i++)
                            {
                                frames[i] = finalFrame;
                            }
                        }
                        Entity sFX;
                        if (current == 0)
                        {
                            sFX = new Entity(new List<Component>()
                    {
                        new Coordinate(coordinate.vector2), new Draw(itemFrame)
                    });
                        }
                        else
                        {
                            sFX = new Entity(new List<Component>()
                    {
                        new Coordinate(coordinate.vector2), new Draw("Gray", "Black", '.')
                    });
                        }
                        sFX.AddComponent(new AnimationFunction(frames));
                        sfx.Add(sFX);
                        current++;
                    }
                    Renderer.StartAnimationThread(sfx, coordinates.Count(), 250 / coordinates.Count());
                }
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
            if (attacker != null && target != null && weapon != null && weapon.CheckComponent<AttackFunction>())
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
                        if (World.random.Next(0, 20) + int.Parse(parts[4]) >= target.GetComponent<Stats>().ac)
                        {
                            int dmg = 0;
                            for (int d = 0; d < int.Parse(parts[1]); d++)
                            {
                                dmg += World.random.Next(1, int.Parse(parts[2]));
                            }
                            dmg += int.Parse(parts[3]);
                            target.GetComponent<OnHit>().Hit(dmg, attackFunction.dmgType, attackName, attacker);
                        }
                        else
                        {
                            Log.Add($"The {attackName} missed its target.");
                        }
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
