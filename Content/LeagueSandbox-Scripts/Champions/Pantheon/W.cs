using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class PantheonW : ISpellScript
    {
        private IAttackableUnit Target;
        private ISpell _spell;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            ApiEventManager.OnFinishDash.AddListener(this, owner, onDash, false);
        }

        public void onDash(IAttackableUnit owner)
        {
            var AP = owner.Stats.AbilityPower.Total;
            float damage = 50 * _spell.CastInfo.SpellLevel + AP;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddBuff("Stun", 1f, 1, _spell, Target, owner as IObjAiBase);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            owner.SetTargetUnit(null);
            var to = Vector2.Normalize(Target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell2", new Vector2(Target.Position.X - to.X * 100f, Target.Position.Y - to.Y * 100f), 1500, 0, 0, 0);
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