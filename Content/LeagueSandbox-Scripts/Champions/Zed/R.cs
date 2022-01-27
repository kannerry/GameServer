using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class ZedUlt : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        static internal bool RCast = false;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnFinishDash.AddListener(this, owner, makeVisible, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        bool ultDash = false;
        public void makeVisible(IAttackableUnit owner)
        {
            if(ultDash == true)
            {
                var objOwner = owner as IObjAiBase;
                owner.SetStatus(StatusFlags.Targetable, true);
                owner.SetStatus(StatusFlags.Ghosted, false);
                var Champs = GetAllChampionsInRange(owner.Position, 50000);
                foreach (IChampion player in Champs)
                {
                    objOwner.SetInvisible((int)player.GetPlayerId(), objOwner, 1f, 0.05f);
                    objOwner.SetHealthbarVisibility((int)player.GetPlayerId(), objOwner, false);
                }
                ultDash = false;
            }
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            RCast = true;
            ultDash = true;
            Vector2 originPos = new Vector2(owner.Position.X + 1, owner.Position.Y + 1);
            var to = Vector2.Normalize(target.Position - owner.Position);
            var Champs = GetAllChampionsInRange(owner.Position, 50000);

            ZedShadowDashMissile._spell = spell;
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, originPos, originPos, true, Vector2.Zero);
            owner.SetStatus(StatusFlags.Ghosted, true);
            owner.DashToLocation(new Vector2(target.Position.X + to.X * 125f, target.Position.Y + to.Y * 125f), 1000, "RUN");
            owner.SetTargetUnit(null);
            owner.SetStatus(GameServerCore.Enums.StatusFlags.Targetable, false);

            AddParticle(spell.CastInfo.Owner, target, "Zed_Ult_TargetMarker_tar.troy", Vector2.Zero);
            AddBuff("ZedMarker", 4.0f, 1, spell, target, owner);

            foreach (IChampion player in Champs)
            {
                owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.05f);
                owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
            }
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

    public class ZedR2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool recast = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var RShadow = ZedShadowDashMissile.RShadow;
            var ownerPos = owner.Position;
            var RShadowPos = RShadow.Position;
            RShadow.TeleportTo(ownerPos.X, ownerPos.Y);
            owner.TeleportTo(RShadowPos.X, RShadowPos.Y);
            owner.SetSpell("ZedUlt", 3, true);

            AddParticleTarget(owner, owner, "zed_base_cloneswap.troy", owner);
            AddParticleTarget(owner, RShadow, "zed_base_cloneswap.troy", RShadow);
            recast = true;

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