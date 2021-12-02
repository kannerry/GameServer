using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class SummonerDot : IBuffGameScript
    {
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle ignite;
        private IObjAiBase Owner;
        private IAttackableUnit Target;

        private float timeSinceLastTick = 1000.0f;
        private float damage;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            Target = unit;
            damage = 10 + Owner.Stats.Level * 4;
            ignite = AddParticleTarget(Owner, unit, "Global_SS_Ignite.troy", unit, buff.Duration, bone: "C_BUFFBONE_GLB_CHEST_LOC");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = null;
            Target = null;
            ignite.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
            if (Target == null || Owner == null)
            {
                return;
            }

            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f)
            {
                Target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
                timeSinceLastTick = 0;
            }
        }
    }
}