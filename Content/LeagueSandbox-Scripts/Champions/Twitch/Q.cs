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
    public class TwitchHideInShadows : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(owner, spell, SwagExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        // THIS IS THE MOST SCUFFED THING EVER
        // DO NOT USE THIS
        // BE WARNED

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            //owner.SetStatus(StatusFlags.NoRender, true);
            //CreateTimer(4.0f, () => { owner.SetStatus(StatusFlags.NoRender, false); });
            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (IChampion player in Champs)
            {
                CreateTimer(4.5f, () => { owner.SetStatus(StatusFlags.Targetable, true); });
                owner.SetStatus(StatusFlags.Targetable, false);
                if (player.Team.Equals(owner.Team))
                {
                    CreateTimer(4.5f, () => { owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f); });
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0.5f, 0.1f);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    CreateTimer(4.5f, () => { owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f); });
                    CreateTimer(4.5f, () => { owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true); });
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                }
            }
        }

        public void OnSpellCast(ISpell spell)
        {
            IChampion owner = spell.CastInfo.Owner as IChampion;
            CreateTimer(0.1f, () =>
            {
                owner.disableBroadcastStats = true;
                owner.SetStatus(StatusFlags.NoRender, true);
                owner.NotifyPlayerStatsOppositeTeam(owner);
            });
            CreateTimer(0.2f, () =>
            {
                owner.disableBroadcastStats = true;
                owner.SetStatus(StatusFlags.NoRender, true);
                owner.NotifyPlayerStatsOppositeTeam(owner);
            });
            // DOES NOT WORK UNLESS YOU PROC IT TWICE ?? GOD HELP ME
            CreateTimer(4.5f, () =>
            {
                owner.disableBroadcastStats = false;
                owner.SetStatus(StatusFlags.NoRender, false);
            });


        }

        public void OnSpellPostCast(ISpell spell)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var owner = spell.CastInfo.Owner;
            var smokeBombBorder = AddParticle(owner, owner, "Twitch_Base_Q_Bamf.troy", spellPos, lifetime: 3.0f);
            var smokeBombBorder1 = AddParticle(owner, owner, "Twitch_Base_Q_Cas_Invisible.troy", spellPos, lifetime: 3.0f);
            var SwagSector = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                OverrideFlags = SpellDataFlags.AffectHeroes | SpellDataFlags.AffectFriends | SpellDataFlags.AffectEnemies,
                Length = 370f,
                Tickrate = 10,
                CanHitSameTargetConsecutively = true,
                Type = SectorType.Area,
                Lifetime = 4
            });
        }

        public void SwagExecute(ISpell ownerSpell, IAttackableUnit target, ISpellMissile swag, ISpellSector sector)
        {
            IChampion owner = ownerSpell.CastInfo.Owner as IChampion;
            if (target.Team != owner.Team)
            {
                if(target is IChampion)
                {
                    owner.disableBroadcastStats = false;
                    owner.SetStatus(StatusFlags.NoRender, false);
                    AddBuff("TwitchVisible", 0.2f, 1, ownerSpell, owner, owner);
                }
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