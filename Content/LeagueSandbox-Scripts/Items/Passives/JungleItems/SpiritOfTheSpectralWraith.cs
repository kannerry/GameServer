using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerCore.Enums;
//Spirit1
namespace ItemPassives
{
    public class ItemID_3206 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase itemOwner;
        public void OnActivate(IObjAiBase owner)
        {
            StatsModifier.CooldownReduction.PercentBonus = 0.1f;
            owner.AddStatModifier(StatsModifier);
            itemOwner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            ApiEventManager.OnKillUnit.AddListener(this, owner, KillUnit, false);
        }

        private void KillUnit(IDeathData dat)
        {
            if (dat.Unit is IMonster)
            {
                var monster = dat.Unit as IMonster; // KRUG RED CHICKEN WOLF BLUE GROMP DRAGON BARON
                if (monster.MinionSpawnType == MonsterSpawnType.GOLEM || monster.MinionSpawnType == MonsterSpawnType.ELDER_LIZARD
                    || monster.MinionSpawnType == MonsterSpawnType.GREAT_WRAITH || monster.MinionSpawnType == MonsterSpawnType.GIANT_WOLF
                    || monster.MinionSpawnType == MonsterSpawnType.ANCIENT_GOLEM || monster.MinionSpawnType == MonsterSpawnType.WRAITH
                    || monster.MinionSpawnType == MonsterSpawnType.DRAGON || monster.MinionSpawnType == MonsterSpawnType.WORM)
                {
                    itemOwner.Stats.AbilityPower.FlatBonus += 2;
                }
            }
        }

        private void TargetExecute(IAttackableUnit unit, bool crit)
        {
            if (unit is IMonster)
            {
                unit.TakeDamage(itemOwner, itemOwner.Stats.AttackDamage.Total * 0.3f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                itemOwner.Stats.CurrentHealth += itemOwner.Stats.AttackDamage.Total * 0.06f;
                itemOwner.Stats.CurrentMana += itemOwner.Stats.AttackDamage.Total * 0.03f;
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
