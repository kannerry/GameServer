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
    public class NasusE : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
            SpellToggleSlot = 4
        };

        public ISpellSector DamageSector;
        public ISpellSector SlowSector;

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
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var owner = spell.CastInfo.Owner;
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            AddParticlePos(owner, "Nasus_E_Green_Ring.troy", spellPos, spellPos, 5.0f, teamOnly: owner.Team);
            AddParticlePos(owner, "Nasus_Base_E_SpiritFire.troy", spellPos, spellPos, 5.0f);
            if (owner.Team == TeamId.TEAM_BLUE)
            {
                AddParticlePos(owner, "Nasus_E_Red_Ring.troy", spellPos, spellPos, 5.0f, teamOnly: TeamId.TEAM_PURPLE);
            }
            if (owner.Team == TeamId.TEAM_PURPLE)
            {
                AddParticlePos(owner, "Nasus_E_Red_Ring.troy", spellPos, spellPos, 5.0f, teamOnly: TeamId.TEAM_BLUE);
            }
            CreateTimer(5.0f, () => { DamageSector.SetToRemove(); });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.125f;
            var damage = 40 + (20 * (spell.CastInfo.SpellLevel - 1)) + ap;

            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("NasusE", 1.0f, 1, spell, target, spell.CastInfo.Owner);
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