using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3025 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase baseOwner;
        private bool Casted = false;

        public void OnActivate(IObjAiBase owner)
        {
            baseOwner = owner;
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[0], SpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[1], SpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[2], SpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[3], SpellCast);
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, TargetExecute, false);
        }

        public void SpellCast(ISpell spell)
        {
            Casted = true;
        }

        ISpell _spell;
        bool timer = false;
        private void TargetExecute(ISpell spell)
        {
            if (Casted == true)
            {
                if(timer == false)
                {
                    _spell = spell;
                    var unit = spell.CastInfo.Targets[0].Unit;
                    unit.TakeDamage(baseOwner, baseOwner.Stats.AttackDamage.BaseValue * 1.25f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                    AddParticle(baseOwner, null, "item_frozen_gauntlet_green.troy", unit.Position);
                    var DamageSector = _spell.CreateSpellSector(new SectorParameters
                    {
                        Tickrate = 15000,
                        Length = 350f,
                        OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                        Type = SectorType.Area,
                        Lifetime = 2.0f
                    });
                    CreateTimer(1.5f, () => { timer = false; });
                    timer = true;
                }
                Casted = false;
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}