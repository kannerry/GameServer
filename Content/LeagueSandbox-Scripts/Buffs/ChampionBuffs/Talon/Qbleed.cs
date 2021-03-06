using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class TalonBleedDebuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.POISON;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float damage;
        private float timeSinceLastTick = 900f;
        private IAttackableUnit Unit;
        private IObjAiBase owner;
        private IParticle p;
        private IParticle p2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            var ADratio = owner.Stats.AttackDamage.PercentBonus;
            damage = (10 * ownerSpell.CastInfo.SpellLevel + ADratio) / 6f;

            p = AddParticleTarget(owner, unit, "talon_Q_bleed_indicator.troy", unit, 1, (float)1.5);
            //p2 = AddParticle(owner, unit, "talon_Q_bleed.troy", unit.Position, 1, (float)1.5);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0f;
            }
        }
    }
}