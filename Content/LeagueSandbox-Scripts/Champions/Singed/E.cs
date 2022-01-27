using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
namespace Spells
{
    public class Fling : ISpellScript
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

        public static IObjAiBase _target = null;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private IObjAiBase _owner;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _target = target as IObjAiBase;
            _owner = owner;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            FaceDirection(_owner.Position, _target);
            var x = GetPointFromUnit(_target, 550);
            _target.SetTargetUnit(null);
            _owner.SetTargetUnit(null);
            ForceMovement(_target, "run", x, 1500, 0, 0, 0);
            float damage = 40 + 10 * spell.CastInfo.SpellLevel;
            float apratio = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.6f;
            _target.TakeDamage(_owner, damage + apratio, GameServerCore.Enums.DamageType.DAMAGE_TYPE_MAGICAL, GameServerCore.Enums.DamageSource.DAMAGE_SOURCE_SPELL, false);

            CreateTimer(0.5f, () =>
            {
                if (_target.HasBuff("SingedCringe"))
                {
                    LogDebug("Fling Combo");
                    AddBuff("SingedCringeRoot", 1.0f, 1, spell, _target, _owner);
                }

            });
        }

        public void ApplyEffects(IObjAiBase owner, IAttackableUnit target, ISpell spell, ISpellMissile missile)
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