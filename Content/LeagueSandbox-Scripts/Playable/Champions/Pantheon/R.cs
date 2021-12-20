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
    public class PantheonRJump : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            owner.SetStatus(StatusFlags.CanMove, false);
            CreateTimer(1.0f, () => {
                AddParticle(owner, null, "Pantheon_Base_R_jump.troy", owner.Position, lifetime: 3.0f);

                owner.SetStatus(StatusFlags.Targetable, false);

                var Champs = GetChampionsInRange(owner.Position, 50000, true);
                foreach (IChampion player in Champs)
                {
                    if (player.Team.Equals(owner.Team))
                    {
                        owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                    }
                    if (!(player.Team.Equals(owner.Team)))
                    {
                        if (player.IsAttacking)
                        {
                            player.CancelAutoAttack(false);
                        }
                        owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                        owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                    }
                }

                if (owner.Team == TeamId.TEAM_BLUE)
                {
                    AddParticle(owner, null, "Pantheon_Base_R_indicator_green.troy", spellPos, lifetime: 3.0f, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
                    AddParticle(owner, null, "Pantheon_Base_R_indicator_red.troy", spellPos, lifetime: 3.0f, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
                }
                else
                {
                    AddParticle(owner, null, "Pantheon_Base_R_indicator_green.troy", spellPos, lifetime: 3.0f, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
                    AddParticle(owner, null, "Pantheon_Base_R_indicator_red.troy", spellPos, lifetime: 3.0f, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
                }
            });
            CreateTimer(3.0f, () => { 
                TeleportTo(owner, spellPos.X, spellPos.Y);
                var DamageSector = spell.CreateSpellSector(new SectorParameters
                {
                    Tickrate = 15,
                    Length = 250f,
                    OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                    Type = SectorType.Area,
                    Lifetime = 1.0f,
                    CanHitSameTargetConsecutively = false
                });
                AddParticle(owner, null, "Pantheon_Base_R_aoe_explosion.troy", spellPos, lifetime: 3.0f);

                owner.SetStatus(StatusFlags.Targetable, true);
                owner.SetStatus(StatusFlags.CanMove, true);

                var Champs = GetChampionsInRange(owner.Position, 50000, true);
                foreach (IChampion player in Champs)
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true);
                }

            });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 200 + (50 * (spell.CastInfo.SpellLevel - 1)) + ap;
            //Graves_SmokeGrenade_Cloud_Team_Green.troy
            //Graves_SmokeGrenade_Cloud_Team_Red.troy
            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
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