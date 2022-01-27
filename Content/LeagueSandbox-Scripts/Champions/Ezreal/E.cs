using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class EzrealArcaneShift : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var startPos = owner.Position;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);

            var to = trueCoords - startPos;
            if (to.Length() > 475)
            {
                trueCoords = GetPointFromUnit(owner, 475f);
            }

            var target = GetClosestUnitInRange(owner, 750f, true);

            if (target != null)
            {
                if (!(target is IBaseTurret))
                {
                    FaceDirection(target.Position, owner, true);
                    SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, target, trueCoords);
                }
            }

            AddParticle(owner, null, "Ezreal_arcaneshift_cas.troy", startPos);
            AddParticleTarget(owner, owner, "Ezreal_arcaneshift_flash.troy", owner);

            TeleportTo(owner, trueCoords.X, trueCoords.Y);
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

    public class EzrealArcaneShiftMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            AddBuff("EzrealRisingSpellForce", 6.0f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
            target.TakeDamage(spell.CastInfo.Owner, 75f + ((spell.CastInfo.SpellLevel - 1) * 50f) + spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.75f,
                DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            missile.SetToRemove();
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