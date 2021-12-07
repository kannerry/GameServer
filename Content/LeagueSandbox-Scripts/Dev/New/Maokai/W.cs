using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class MaokaiUnstableGrowth : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public static IAttackableUnit _target = null;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnFinishDash.AddListener(this, owner, AlistarPush, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private IObjAiBase _owner;
        private ISpell _spell;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var to = Vector2.Normalize(target.Position - owner.Position);
            owner.StopMovement();
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell1", new Vector2(target.Position.X - to.X * 100f, target.Position.Y - to.Y * 100f), 5000, 0, 0, 0);
            _target = target;
            _owner = owner;
            _spell = spell;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void AlistarPush(IAttackableUnit unit)
        {
            AddBuff("LuxQ", 1f, 1, _spell, _target, _owner);
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void ApplyEffects(IObjAiBase owner, IAttackableUnit target, ISpell spell, ISpellMissile missile)
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