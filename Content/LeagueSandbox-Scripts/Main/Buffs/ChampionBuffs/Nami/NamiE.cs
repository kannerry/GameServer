using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    internal class NamiE : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;
        private IObjAiBase own;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, unit as IObjAiBase, TargetExecute, false);
            var owner = ownerSpell.CastInfo.Owner;
            own = owner;
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            var ap = own.Stats.AbilityPower.Total;
            float damage = (float)(ap * 0.2 + own.GetSpell(1).CastInfo.SpellLevel * 15);
            StatsModifier.MoveSpeed.BaseBonus -= 30;
            unit.AddStatModifier(StatsModifier);
            unit.TakeDamage(own, damage * 2, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}