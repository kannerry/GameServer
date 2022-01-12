using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs

{
    internal class LiandrysBurn : IBuffGameScript
    {
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase owner;
        IAttackableUnit Unit;

        IBuff Thisbuff;

        float damage;
        float timeSinceLastTick = 500f;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Thisbuff = buff;
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            float AD = unit.Stats.HealthPoints.Total * 0.02f;
            damage = 12 * ownerSpell.CastInfo.SpellLevel + AD;
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
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