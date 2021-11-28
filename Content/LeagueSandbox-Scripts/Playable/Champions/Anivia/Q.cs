using System;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class FlashFrost : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            AddBuff("Stun", 1.0f, 1, spell, target, owner);
            AddParticleTarget(owner, target, "cryo_ice_impact.troy", target, size: 0.5f);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(targetPos, owner);
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        internal static int casted = 1;
        internal static bool popped = false;
        internal static ISpellMissile mis;
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var distance = GetPointFromUnit(owner, 1100f);
            if (casted.Equals(0))
            {
                LogDebug("popped");
                popped = true;
                var SwagSector = spell.CreateSpellSector(new SectorParameters
                {
                    BindObject = mis,
                    Length = 200f,
                    Tickrate = 10,
                    CanHitSameTargetConsecutively = false,
                    Type = SectorType.Area
                });
                CreateTimer(2.0f, () => { popped = false; });
                CreateTimer(1.0f, () => { SwagSector.SetToRemove(); });
            }
            if (casted.Equals(1))
            {
                SpellCast(owner, 0, SpellSlotType.ExtraSlots, distance, distance, false, Vector2.Zero);
                casted = 0;
                CreateTimer(1.5f, () => { casted = 1; });
            }
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
    public class FlashFrostSpell : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            // TODO
        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnLaunchMissile.AddListener(this, new KeyValuePair<IObjAiBase, ISpell>(owner, spell), CastSpell, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }
        ISpellSector SwagSector;
        public void CastSpell(ISpell spell, ISpellMissile missile)
        {
            FlashFrost.mis = missile;
            CreateTimer(1.35f, () =>
            {
                if (FlashFrost.popped != true)
                {
                    SwagSector = spell.CreateSpellSector(new SectorParameters
                    {
                        BindObject = missile,
                        Length = 200f,
                        Tickrate = 10,
                        CanHitSameTargetConsecutively = false,
                        Type = SectorType.Area
                    });
                    missile.SetToRemove();
                };
            }
            );
        }

        public void OnSpellCast(ISpell spell)
        {

        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 0.5f;
            float damage = 60 + (spell.CastInfo.SpellLevel - 1) * 30 + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddBuff("Chilled", 3.0f, 1, spell, target, owner);
            AddParticleTarget(owner, target, "cryo_ice_impact.troy", target, size: 0.5f);

            if (sector == SwagSector)
            {
                AddBuff("Stun", 1.0f, 1, spell, target, owner);
                AddParticleTarget(owner, target, "cryo_ice_impact.troy", target, size: 0.5f);
            }

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
            if (FlashFrost.popped == true)
            {
                CreateTimer(0.1f, () => { FlashFrost.mis.SetToRemove(); });
            }
        }
    }
}
