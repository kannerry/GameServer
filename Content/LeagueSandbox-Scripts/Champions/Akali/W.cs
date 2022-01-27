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
    public class AkaliSmokeBomb : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, SwagExecute, false);
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, RemoveInvis, false);
        }

        public void RemoveInvis(ISpell spell)
        {
            if(SwagSector != null)
            {
                SwagSector.SetToRemove();
                smokeBomb.SetToRemove();
                smokeBombBorder.SetToRemove();
            }
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
        static internal ISpellSector SwagSector;
        static internal IParticle smokeBomb;
        static internal IParticle smokeBombBorder;
        public void OnSpellPostCast(ISpell spell)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var owner = spell.CastInfo.Owner;
            smokeBomb = AddParticle(owner, null, "akali_smoke_bomb_tar.troy", spellPos, lifetime: 8.0f);
            smokeBombBorder = AddParticle(owner, null, "akali_smoke_bomb_tar_team_green.troy", spellPos, lifetime: 8.0f);
            SwagSector = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = smokeBomb.BindObject,
                OverrideFlags = SpellDataFlags.AffectHeroes | SpellDataFlags.AffectFriends,
                Length = 370f,
                Tickrate = 10,
                CanHitSameTargetConsecutively = true,
                Type = SectorType.Area
            });
            CreateTimer(8.0f, () => { if (SwagSector != null) { SwagSector.SetToRemove(); } });
        }

        public void SwagExecute(ISpell ownerSpell, IAttackableUnit target, ISpellMissile swag, ISpellSector sector)
        {
            if (target.Equals(ownerSpell.CastInfo.Owner))
            {
                AddBuff("AkaliShroudBuff", 0.2f, 1, ownerSpell, ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner);
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