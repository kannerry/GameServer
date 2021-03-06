using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Deceive : ISpellScript
    {
        private Vector2 teleportTo;

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

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            teleportTo = new Vector2(end.X, end.Y);
            float targetPosDistance = Math.Abs((float)Math.Sqrt(Math.Pow(owner.Position.X - teleportTo.X, 2f) + Math.Pow(owner.Position.Y - teleportTo.Y, 2f)));
            FaceDirection(teleportTo, owner);
            teleportTo = GetPointFromUnit(owner, Math.Min(targetPosDistance, 400f));
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddParticle(owner, null, "JackintheboxPoof2.troy", owner.Position, 2f);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            TeleportTo(spell.CastInfo.Owner, teleportTo.X, teleportTo.Y);
            AddBuff("Deceive", 3.5f, 1, spell, owner, owner);
            CreateTimer(3.5f, () => { owner.SetStatus(StatusFlags.Targetable, true); });
            owner.SetStatus(StatusFlags.Targetable, false);
            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (IChampion player in Champs)
            {
                CreateTimer(3.5f, () => { owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true); });
                if (player.Team.Equals(owner.Team))
                {
                    CreateTimer(3.5f, () => { owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f); });
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0.5f, 0.1f);
                    AddParticleTarget(owner, owner, "Khazix_Base_R_Cas.troy", owner);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    CreateTimer(3.5f, () => { owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f); });
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
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