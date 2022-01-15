using GameServerCore;
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
    public class AatroxQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        private ISpell _spell;
        private ISpellSector sectora;
        private ISpellSector sectorb;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnFinishDash.AddListener(this, owner, onDash, false);
        }

        public void onDash(IAttackableUnit owner)
        {

            sectora = _spell.CreateSpellSector(new SectorParameters
            {
                Length = 300f,
                CanHitSameTargetConsecutively = false,
                //SingleTick = true,
                Type = SectorType.Area,
                Lifetime = 0.1f,
                BindObject = owner,
            });

            sectorb = _spell.CreateSpellSector(new SectorParameters
            {
                Length = 100f,
                CanHitSameTargetConsecutively = false,
                //SingleTick = true,
                Type = SectorType.Area,
                Lifetime = 0.1f,
                BindObject = owner,
            });
            //Aatrox_Base_Q_Land.troy
            AddParticle(owner, null, "Aatrox_Base_Q_Land.troy", owner.Position, lifetime: 3.0f, reqVision: false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {

            owner.Stats.CurrentHealth = owner.Stats.CurrentHealth - owner.Stats.CurrentHealth * 0.1f;
            owner.Stats.CurrentMana += owner.Stats.ManaPoints.Total * 0.1f;

            int speed1 = 250;
            int speed2 = 2550;
            var castrange = spell.GetCurrentCastRange();
            var ownerPos = owner.Position;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            if (Extensions.IsVectorWithinRange(ownerPos, spellPos, 600))
            {
                if (Extensions.IsVectorWithinRange(ownerPos, spellPos, 150))
                {
                    LogDebug("close");
                    owner.SetTargetUnit(null);
                    ForceMovement(spell.CastInfo.Owner, "Spell1", spellPos, speed1, 0, 5, 0, GameServerCore.Enums.ForceMovementType.FIRST_WALL_HIT);
                    owner.PlayAnimation("Spell1", 1, 0, 1);
                    AddParticle(owner, null, "Aatrox_Base_Q_Tar_Green.troy", spellPos, lifetime: 3.0f, reqVision: false);
                    return;
                }
                owner.SetTargetUnit(null);
                ForceMovement(spell.CastInfo.Owner, "Spell1", spellPos, speed1, 0, 5, 0, GameServerCore.Enums.ForceMovementType.FIRST_WALL_HIT);
                CreateTimer(0.5f, () => { ForceMovement(spell.CastInfo.Owner, "Spell1", spellPos, speed2, 0, 0, 0, GameServerCore.Enums.ForceMovementType.FIRST_WALL_HIT); });
                AddParticle(owner, null, "Aatrox_Base_Q_Tar_Green.troy", spellPos, lifetime: 3.0f, reqVision: false);
                owner.PlayAnimation("Spell1", 1, 0, 1);
            }
            else
            {
                FaceDirection(spellPos, owner);
                var trueCoords = GetPointFromUnit(spell.CastInfo.Owner, 750);
                owner.SetTargetUnit(null);
                ForceMovement(spell.CastInfo.Owner, "Spell1", trueCoords, speed1, 0, 5, 0, GameServerCore.Enums.ForceMovementType.FIRST_WALL_HIT);
                CreateTimer(0.5f, () => { ForceMovement(spell.CastInfo.Owner, "Spell1", trueCoords, speed2, 0, 0, 0, GameServerCore.Enums.ForceMovementType.FIRST_WALL_HIT); });
                AddParticle(owner, null, "Aatrox_Base_Q_Tar_Green.troy", trueCoords, lifetime: 3.0f, reqVision: false);
                owner.PlayAnimation("Spell1", 1, 0, 1);
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.2f;
            var damage = 65 + 35 * (spell.CastInfo.SpellLevel - 1) + AD;
            if (target.Team != owner.Team)
            {
                if (sector == sectorb)
                {
                    AddBuff("Pulverize", 1.0f, 1, spell, target, owner);
                }
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
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