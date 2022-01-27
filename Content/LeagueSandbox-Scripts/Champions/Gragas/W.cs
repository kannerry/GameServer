using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class GragasW : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellCast.AddListener(this, spell, PassiveHeal);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        ISpell _spell;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _spell = spell;
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, true);
            AddBuff("GragasW", 2.5f, 1, spell, target, owner);
        }

        //ApiEventManager.OnSpellCast.AddListener(this, spell, PassiveHeal);
        public void PassiveHeal(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;

            if (owner.HasBuff("GragasCanPassive"))
            {
                owner.RemoveBuffsWithName("GragasCanPassive");
                LogDebug("yo1");
                AddBuff("GragasPassiveCooldown", 8.0f, 1, spell, owner, owner);
                PerformHeal(owner, spell, owner);
            }

        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * spell.SpellData.MagicDamageCoefficient;
            float healthGain = 15 + (spell.CastInfo.SpellLevel * 45) + ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        private IObjAiBase _owner;
        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            var dmg = 20 + 30 * _spell.CastInfo.SpellLevel - 1;
            var ap = _owner.Stats.AbilityPower.Total * 0.3f;
            var hp = Unit.Stats.HealthPoints.Total * 0.08f;
            Unit.TakeDamage(_owner, dmg + ap + hp, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}