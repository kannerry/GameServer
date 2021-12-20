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
    public class GravesSmokeGrenade : ISpellScript
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
            var owner = spell.CastInfo.Owner;
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 1,
                Length = 250f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1
            });
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            if (owner.Team == TeamId.TEAM_BLUE)
            {
                red = AddParticle(owner, null, "Graves_SmokeGrenade_Cloud_Team_Red.troy", spellPos, lifetime: 4.0f, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
                green = AddParticle(owner, null, "Graves_SmokeGrenade_Cloud_Team_Green.troy", spellPos, lifetime: 4.0f, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
            }
            else
            {
                red = AddParticle(owner, null, "Graves_SmokeGrenade_Cloud_Team_Red.troy", spellPos, lifetime: 4.0f, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
                green = AddParticle(owner, null, "Graves_SmokeGrenade_Cloud_Team_Green.troy", spellPos, lifetime: 4.0f, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
            }
        }

        private IParticle red;
        private IParticle green;

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 60 + (20 * (spell.CastInfo.SpellLevel - 1)) + ap;
            //Graves_SmokeGrenade_Cloud_Team_Green.troy
            //Graves_SmokeGrenade_Cloud_Team_Red.troy
            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
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