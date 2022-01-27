using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class UFSlash : ISpellScript
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

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _owner = owner;
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            //var to = Vector2.Normalize(target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell4", spellpos, 2200, 0, 0, 0);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void AlistarPush(IAttackableUnit unit)
        {
            var ap = _owner.Stats.AbilityPower.Total;
            var damage = 200 * _owner.GetSpell(3).CastInfo.SpellLevel + ap;

            LogDebug("yo");

            AddParticle(_owner, null, "Malphite_Base_UnstoppableForce_stun.troy", _owner.Position);
            AddParticle(_owner, null, "Malphite_Base_UnstoppableForce_tar.troy", _owner.Position);

            var x = GetUnitsInRange(_owner.Position, 325, true);
            foreach (var unitx in x)
            {
                if (unitx.Team != _owner.Team)
                {
                    var randOffset = (float)new Random().NextDouble();
                    var randPoint = new Vector2(unit.Position.X + 25.0f, unit.Position.Y + 25.0f);
                    var xy = unitx as IObjAiBase;

                    AddParticleTarget(_owner, unitx, "Malphite_Base_UnstoppableForce_tar.troy", unitx, 1f);

                    xy.SetTargetUnit(null);
                    ForceMovement(unitx, "", randPoint, 90.0f, 80.0f, 20.0f, 0.0f, ForceMovementType.FURTHEST_WITHIN_RANGE, ForceMovementOrdersType.CANCEL_ORDER, ForceMovementOrdersFacing.KEEP_CURRENT_FACING);

                    unitx.TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
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