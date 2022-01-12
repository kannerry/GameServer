using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs

{
    internal class DariusHemoMarker : IBuffGameScript
    {
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 5;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase owner;
        IAttackableUnit Unit;

        IBuff Thisbuff;

        float damage;
        float timeSinceLastTick = 500f;
        Dictionary<int, List<string>> particles = new Dictionary<int, List<string>>
        {
            {1, new List<string>{{ "darius_Base_hemo_bleed_trail_only1.troy" }, { "darius_Base_hemo_counter_01.troy"} } },
            {2, new List<string>{{ "darius_Base_hemo_bleed_trail_only2.troy" }, { "darius_Base_hemo_counter_02.troy" } } },
            {3, new List<string>{{ "darius_Base_hemo_bleed_trail_only3.troy" }, { "darius_Base_hemo_counter_03.troy" } } },
            {4, new List<string>{{ "darius_Base_hemo_bleed_trail_only4.troy" }, { "darius_Base_hemo_counter_04.troy" } } },
            {5, new List<string>{{ "darius_Base_hemo_bleed_trail_only6.troy" }, { "darius_Base_hemo_counter_05.troy" } } }, 
            

        };
        Dictionary<int, List<IParticle>> currentParticles = new Dictionary<int, List<IParticle>>
        {
            {1, new List<IParticle>()},
            {2, new List<IParticle>()},
            {3, new List<IParticle>()},
            {4, new List<IParticle>()},
            {5, new List<IParticle>()},
        };
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Thisbuff = buff;
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            int stackCount = 1;
            try
            {
                stackCount = unit.GetBuffWithName("DariusHemo").StackCount;
            }
            catch
            {
                LogInfo("deu null aq");
            }

            //Adds new particles
            foreach (var p in particles[stackCount])
            {
                currentParticles[stackCount].Add(AddParticleTarget(owner, unit, p, unit, float.MaxValue));
            }
            //Removes previous particles
            if (stackCount > 1)
            {
                foreach (var p in currentParticles[stackCount - 1])
                {
                    RemoveParticle(p);
                }
            }

            float AD = owner.Stats.AttackDamage.FlatBonus * 0.3f;
            damage = 12 * ownerSpell.CastInfo.SpellLevel + AD;
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            foreach (var stack in currentParticles.Keys)
            {
                foreach (var p in currentParticles[stack])
                {
                    RemoveParticle(p);
                }
            }
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000f && !Unit.IsDead && Unit != null && owner != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0;
            }
        }
    }
}