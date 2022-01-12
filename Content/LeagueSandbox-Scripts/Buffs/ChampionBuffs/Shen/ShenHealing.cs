using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs

{
    internal class ShenHealing : IBuffGameScript
    {
        public BuffType BuffType => BuffType.HEAL;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase _shen;
        IAttackableUnit _unit;
        ISpell _spell;
        float timeSinceLastTick = 500f;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _shen = ownerSpell.CastInfo.Owner;
            _unit = unit;
            _spell = ownerSpell;
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            float ap = (owner.Stats.HealthPoints.Total * 1.5f) / 100;
            float healthGain = 2 + (spell.CastInfo.SpellLevel * 4) + ap;
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
            AddParticle(_shen, _unit, "shen_vorpalStar_lifetap_tar_02.troy", _unit.Position);
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000f)
            {
                PerformHeal(_shen, _spell, _unit);
                LogDebug("proc");
                timeSinceLastTick = 0;
            }
        }
    }
}