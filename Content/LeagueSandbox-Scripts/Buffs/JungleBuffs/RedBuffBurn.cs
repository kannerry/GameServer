using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;

namespace Buffs

{
    internal class RedBuffBurn : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; }

        private IObjAiBase _owner;
        private float counter = 100;
        private ISpell spell;
        private IBuff Buff;
        private IAttackableUnit Unit;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
            Buff = buff;
            Unit = unit;

            //TODO: FIND PROPER PARTICLE NAMES
            //p = AddParticleTarget(_owner, unit, "NeutralMonster_buf_red_offense_big.troy", unit, buff.Duration);
            //p2 = AddParticleTarget(_owner, unit, "SRU_JungleBuff_Redbuff_Health.troy", unit);

            float slow = 0.08f;
            if (_owner.Stats.Level >= 6 && _owner.Stats.Level < 11)
            {
                slow = 0.16f;
            }
            if (_owner.Stats.Level >= 11)
            {
                slow = 0.24f;
            }
            if (!_owner.IsMelee)
            {
                slow /= 1.6f;
            }
            StatsModifier.MoveSpeed.PercentBonus -= slow;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //RemoveParticle(p);
            //RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
            counter += diff;

            if (counter >= 1000.0f && _owner != null)
            {
                Unit.TakeDamage(_owner, 8 + 2 * _owner.Stats.Level, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            }
        }
    }
}