using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Meditate : ISpellScript
    {
        private bool cancelled;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        private Vector2 basepos;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            CreateTimer(4.0f, () => { cancelled = false; });
            basepos = owner.Position;
            for (var i = 0.0f; i < 2.5; i += 0.25f)
            {
                CreateTimer(i, () => { ApplySpinDamage(owner, spell, target); });
            }
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * spell.SpellData.MagicDamageCoefficient;
            float healthGain = 5 + (spell.CastInfo.SpellLevel * 10) + ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        private void ApplySpinDamage(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            if (owner.Position.X != basepos.X)
            {
                cancelled = true;
            }
            if (owner.Position.Y != basepos.Y)
            {
                cancelled = true;
            }
            if (!cancelled)
            {
                PerformHeal(owner, spell, owner);
                PlayAnimation(owner, "Spell2");
            }
            if (cancelled)
            {
                StopAnimation(owner, "Spell2");
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
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