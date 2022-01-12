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
    public class ItemID_3077 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase ItemOwner;
        private int stacks = 0;
        ISpell _spell;
        IAttackableUnit unitx;
        public void OnActivate(IObjAiBase owner)
        {
            ItemOwner = owner;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, TiamatExecute, false);
            ApiEventManager.OnSpellHit.AddListener(this, _spell, TargetExecute, false);
        }

        private void TiamatExecute(ISpell spell)
        {
            unitx = spell.CastInfo.Targets[0].Unit;
            _spell = spell;
            AddParticle(ItemOwner, null, "TiamatMelee_itm.troy", new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z));
            var DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 15000,
                Length = 350f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1
            });
        }        
        
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ad = owner.Stats.AttackDamage.Total * 1.0f;
            //Graves_SmokeGrenade_Cloud_Team_Green.troy
            //Graves_SmokeGrenade_Cloud_Team_Red.troy
            if(target != owner.TargetUnit)
            {
                target.TakeDamage(spell.CastInfo.Owner, ad * 0.2f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
            ApiEventManager.OnSpellHit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

//        IAttackableUnit unitx;
//ApiEventManager.OnLaunchAttack.AddListener(this, owner, TiamatExecute, false);
//public void TiamatExecute(ISpell spell)
//{
//    unitx = spell.CastInfo.Targets[0].Unit;
//    var owner = spell.CastInfo.Owner;
//    AddParticle(owner, null, "TiamatMelee_itm.troy", new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z));
//    var DamageSector = spell.CreateSpellSector(new SectorParameters
//    {
//        Tickrate = 15000,
//        Length = 350f,
//        OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
//        Type = SectorType.Area,
//        Lifetime = 1
//    });
//}