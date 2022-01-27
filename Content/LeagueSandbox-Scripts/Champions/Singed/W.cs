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
    public class MegaAdhesive : ISpellScript
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
                Tickrate = 1000,
                Length = 265f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 5f,
                CanHitSameTargetConsecutively = true
            });
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            if (owner.Team == TeamId.TEAM_BLUE)
            {
                red = AddParticle(owner, null, "MegaAdhesive_red_pool.troy", spellPos, lifetime: 5.0f, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
                green = AddParticle(owner, null, "MegaAdhesive_green_pool.troy", spellPos, lifetime: 5.0f, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
            }
            else
            {
                red = AddParticle(owner, null, "MegaAdhesive_red_pool.troy", spellPos, lifetime: 5.0f, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
                green = AddParticle(owner, null, "MegaAdhesive_green_pool.troy", spellPos, lifetime: 5.0f, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
            }
        }

        private IParticle red;
        private IParticle green;

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            AddBuff("SingedCringe", 1.0f, 1, spell, target, spell.CastInfo.Owner);
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