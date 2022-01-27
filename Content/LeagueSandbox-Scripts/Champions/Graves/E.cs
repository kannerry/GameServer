using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class GravesMove : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };
        ISpell _spell;
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, lowerCD, false);
            ApiEventManager.OnFinishDash.AddListener(this, owner, resetAtk, false);
        }

        public void resetAtk(IAttackableUnit owner)
        {
            if(resetatk == true)
            {
                if(atkunit != null)
                {
                    _owner.CancelAutoAttack(true);
                    _owner.SetTargetUnit(atkunit);
                }
                resetatk = false;
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void lowerCD(ISpell spell)
        {
            _spell.LowerCooldown(1.0f);
            AddBuff("GravesPassiveGrit", 3.0f, 1, _spell, _spell.CastInfo.Owner, _spell.CastInfo.Owner);
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        bool resetatk = false;
        IAttackableUnit atkunit;
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var to = Vector2.Normalize(spellPos - current);
            var range = to * 425;
            var maxCoords = current + range;
            var mincoords = GetPointFromUnit(owner, 175);
            atkunit = spell.CastInfo.Owner.TargetUnit;
            spell.CastInfo.Owner.SetTargetUnit(null);
            AddBuff("Quickdraw", 4.0f, 1, spell, owner, owner);

            resetatk = true;

            if (Extensions.IsVectorWithinRange(current, spellPos, 175))
            {
                ForceMovement(owner, "Spell3", mincoords, 1200, 0, 0, 0);
                return;
            }
            else if (Extensions.IsVectorWithinRange(current, spellPos, 375))
            {
                ForceMovement(owner, "Spell3", spellPos, 1200, 0, 0, 0);
            }
            else
            {
                ForceMovement(owner, "Spell3", maxCoords, 1200, 0, 0, 0);
            }


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