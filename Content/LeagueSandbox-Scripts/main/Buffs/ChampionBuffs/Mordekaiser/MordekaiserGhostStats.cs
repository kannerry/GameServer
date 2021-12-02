using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace MordekaiserChildrenOfTheGraveGhost
{
    internal class MordekaiserChildrenOfTheGraveGhost : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 1;
        public bool IsHidden => true;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float timeSinceLastTick;
        private IAttackableUnit Unit;
        private IObjAiBase Owner;
        private int limiter;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            Owner = owner;
            limiter = 0;
            //TODO: Set the Ghost stats here
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 30000.0)
            {
                Unit.TakeDamage(Unit, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            }
        }
    }
}

//TODO: Make healing for POST MITIGATION damage