using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class KatarinaR : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase owner;
        float tickTime;
        float trueHeal;
        int spellLevel;
        IParticle buffParticle;

        private void ApplySpinDamage(IObjAiBase owner, ISpell spell)
        {
            var units = GetUnitsInRange(owner.Position, 500, true);
            foreach (var unit in units)
            {
                if (unit.Team != owner.Team)
                {
                    var ap = owner.Stats.AbilityPower.Total * 0.25f;
                    var ad = owner.Stats.AttackDamage.Total * 0.375f;
                    var damage = 150f + 200 * spell.CastInfo.SpellLevel + ap + ad;
                    if (unit is Minion) damage *= 0.75f;
                    unit.TakeDamage(owner, damage / 9, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddParticle(owner, unit, "katarina_deathLotus_tar.troy", unit.Position);
                }
            }
        }

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            buffParticle = AddParticleTarget(unit, unit, "masteryi_base_w_buf.troy", unit, 4.0f, flags: 0);
        }

        public void TakeDamage(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            var unit = unit2;
            AddParticleTarget(unit, unit, "masteryi_base_w_dmg.troy", unit, flags: 0);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.RemoveAllListenersForOwner(this);
        }

        public void OnUpdate(float diff)
        {
            if (tickTime >= 250.0f)
            {
                ApplySpinDamage(owner, owner.GetSpell(3));
                tickTime = 0;
            }

            tickTime += diff;
        }
    }
}
