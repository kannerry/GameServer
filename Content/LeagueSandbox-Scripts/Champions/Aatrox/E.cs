﻿using GameServerCore.Domain.GameObjects;
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
    public class AatroxE : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
            FaceDirection(end, owner);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            owner.Stats.CurrentHealth = owner.Stats.CurrentHealth - owner.Stats.CurrentHealth * 0.05f;
            owner.Stats.CurrentMana += owner.Stats.ManaPoints.Total * 0.05f;

            var endPos = GetPointFromUnit(spell.CastInfo.Owner, 1000, 0);
            var offsetPosL = GetPointFromUnit(spell.CastInfo.Owner, 100, -90);
            var offsetPosR = GetPointFromUnit(spell.CastInfo.Owner, 100, 90);
            SpellCast(spell.CastInfo.Owner, 1, SpellSlotType.ExtraSlots, endPos, Vector2.Zero, true, offsetPosL);
            SpellCast(spell.CastInfo.Owner, 1, SpellSlotType.ExtraSlots, endPos, Vector2.Zero, true, offsetPosR);
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

    public class AatroxEConeMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
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
            var owner = spell.CastInfo.Owner;
            var ad = owner.Stats.AbilityPower.Total * 0.65 + spell.CastInfo.Owner.GetSpell(2).CastInfo.SpellLevel * 40 + 40;
            target.TakeDamage(owner, (float)ad, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
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