using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemSpells
{
    public class ItemSwordOfFeastAndFamine : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var APratio = target.Stats.CurrentHealth * 0.1f;
            var damage = 100f + APratio;

            AddParticleTarget(owner, target, "hextech_gunBlade_tar.troy", target, 1f);
            AddParticle(owner, owner, "hexTech_Gunblade_cas.troy", Vector2.Zero);
            AddBuff("BORKSlow", 3.0f, 1, spell, target, owner);
            AddParticle(owner, target, "volibear_R_chain_lighting_01.troy", target.Position, size:0.75f);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            if (target.IsDead)
            {
                AddParticle(owner, owner, "hextech_gunBlade_tar.troy", target.Position, lifetime: 1f);
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
