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
using System.Numerics;

namespace Buffs
{
    internal class MissFortuneR : IBuffGameScript
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
            for (int arrowCount = -15; arrowCount < 15; arrowCount = arrowCount + 5)
            {
                SpellCast(owner, 2, SpellSlotType.ExtraSlots, GetPointFromUnit(owner, 1000, arrowCount), GetPointFromUnit(owner, 1000, arrowCount), true, Vector2.Zero);
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
