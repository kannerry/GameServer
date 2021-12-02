using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class TalonCutthroat : ISpellScript
    {
        private IAttackableUnit Target;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
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
            AddParticle(owner, owner, "talon_E_cast.troy", owner.Position, lifetime: 1f);

            var to = Vector2.Normalize(Target.Position - owner.Position);

            TeleportTo(owner, Target.Position.X + to.X * 175f, Target.Position.Y + to.Y * 175f);
            AddBuff("TalonESlow", 0.25f, 1, spell, Target, owner);

            AddParticleTarget(owner, Target, "talon_E_tar.troy", Target, 1f);
            AddParticleTarget(owner, Target, "talon_E_tar_dmg.troy", Target, 0.25f);
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