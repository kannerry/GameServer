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
    public class DariusAxeGrabCone : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
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
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 540f,
                SingleTick = true,
                ConeAngle = 24.76f,
                Type = SectorType.Cone
            });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var t = target as IObjAiBase;
            t.SetTargetUnit(null);

            var to = Vector2.Normalize(target.Position - owner.Position);

            ForceMovement(target, "RUN", GetPointFromUnit(owner, 150), 2000f, 0, 5.0f, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
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