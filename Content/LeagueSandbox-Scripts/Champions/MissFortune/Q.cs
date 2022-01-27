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
    public class MissFortuneRicochetShot : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            IsDamagingSpell = true,
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
        }

        public void OnSpellCast(ISpell spell)
        {
            //TODO:Fix broken SpellCast animation
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }
        bool hitOnce = false;
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.35f;
            var ad = owner.Stats.AttackDamage.Total * 0.85f;
            var damage = 5.0f + (15f * spell.CastInfo.SpellLevel) + ap + ad;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);

            AddParticleTarget(owner, target, "MissFortune_Base_Q_Tar.troy", target, lifetime: 1f); //TODO: Fix particles that for some reason aren't spawning ||||| Test if particles now work
            var dis = Vector2.Distance(owner.Position, target.Position);
            FaceDirection(target.Position, owner);
            var p = GetClosestUnitInRange(target, 500, true);
            if(hitOnce == false)
            {
                if (p.Team != owner.Team)
                {
                    if (p != target)
                    {
                        SpellCast(owner, 0, SpellSlotType.SpellSlots, true, p, target.Position);
                    }
                }
                hitOnce = true;
            }
            CreateTimer(1.0f, () => { hitOnce = false; });
            missile.SetToRemove();
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