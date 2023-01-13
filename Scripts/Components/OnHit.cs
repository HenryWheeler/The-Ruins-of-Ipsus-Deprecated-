using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class OnHit: Component
    {
        public List<string> statusEffects = new List<string>();
        public void Hit(int dmg, string type, string weaponName, Entity attacker) 
        {
            SpecialComponentManager.TriggerOnHit(entity, attacker, entity, dmg, type, false);

            if (CMath.ReturnAI(entity) != null) 
            {
                CMath.ReturnAI(entity).OnHit(attacker); 
            }

            Stats stats = entity.GetComponent<Stats>();
            stats.hp -= dmg;

            if (entity.display) 
            {
                StatManager.UpdateStats(entity); 
            }

            if (stats.hp <= 0) 
            {
                Death();
            }
            else
            {
                PronounSet pronounSet = attacker.GetComponent<PronounSet>();
                if (entity == attacker)
                {
                    Log.Add($"{attacker.GetComponent<Description>().name} hit {pronounSet.reflexive} for {dmg} damage with {pronounSet.possesive} {weaponName}.");
                }
                else if (pronounSet != null && !pronounSet.present)
                {
                    Log.Add($"{attacker.GetComponent<Description>().name} hit {entity.GetComponent<Description>().name} for {dmg} damage with {pronounSet.possesive} {weaponName}."); 
                }
                else if (pronounSet != null) 
                { 
                    Log.Add($"{attacker.GetComponent<Description>().name} hit you for {dmg} damage with {pronounSet.possesive} {weaponName}."); 
                }
                else 
                { 
                    Log.Add($"{attacker.GetComponent<Description>().name} hit you for {dmg} damage with the {weaponName}."); 
                }

                Entity hitParticle = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            new Draw("Red", "Black", (char)3),
                            new ParticleComponent(2, 1, "None", 1, new Draw[] { new Draw("Red", "Black", (char)3) })
                        });
                Vector2 vector2 = entity.GetComponent<Coordinate>().vector2;
                Renderer.AddParticle(vector2.x, vector2.y, hitParticle);
            }
        }
        public void LowerHealth(int dmg)
        {
            Stats stats = entity.GetComponent<Stats>();
            stats.hp -= dmg;
            if (entity.display) 
            { 
                StatManager.UpdateStats(entity);
            }
            if (stats.hp <= 0) 
            {
                Death(); 
            }
        }
        public void Death() 
        {
            Entity hitParticle = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            new Draw("Red", "Black", 'X'),
                            new ParticleComponent(2, 1, "None", 1, new Draw[] { new Draw("Red", "Black", 'X') })
                        });
            Coordinate vector2 = entity.GetComponent<Coordinate>();
            Renderer.AddParticle(vector2.vector2.x, vector2.vector2.y, hitParticle);
            Log.Add($"{entity.GetComponent<Description>().name} has died.");
            entity.GetComponent<TurnFunction>().turnActive = false;

            World.GetTraversable(entity.GetComponent<Coordinate>().vector2).actorLayer = null; 
            TurnManager.RemoveActor(entity.GetComponent<TurnFunction>());
            EntityManager.RemoveEntity(entity);

            entity.ClearEntity();
        }
        public OnHit() { }
    }
}
