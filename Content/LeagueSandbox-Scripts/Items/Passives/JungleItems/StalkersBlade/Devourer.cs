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
//Devourer
namespace ItemPassives
{
    public class ItemID_3710 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase itemOwner;
        public void OnActivate(IObjAiBase owner)
        {
            itemOwner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            ApiEventManager.OnKillUnit.AddListener(this, owner, KillUnit, false);
            ApiEventManager.OnKill.AddListener(this, owner, KillChamp, false);
        }
        int i = 0;

        private void KillChamp(IDeathData dat)
        {
            i++;
            i++;
            LogDebug(i.ToString());
        }

        private void KillUnit(IDeathData dat)
        {
            if(dat.Unit is IMonster)
            {
                var monster = dat.Unit as IMonster; // KRUG RED CHICKEN WOLF BLUE GROMP DRAGON BARON
                if(monster.MinionSpawnType == MonsterSpawnType.GOLEM || monster.MinionSpawnType == MonsterSpawnType.ELDER_LIZARD 
                    || monster.MinionSpawnType == MonsterSpawnType.GREAT_WRAITH || monster.MinionSpawnType == MonsterSpawnType.GIANT_WOLF 
                    || monster.MinionSpawnType == MonsterSpawnType.ANCIENT_GOLEM || monster.MinionSpawnType == MonsterSpawnType.WRAITH 
                    || monster.MinionSpawnType == MonsterSpawnType.DRAGON || monster.MinionSpawnType == MonsterSpawnType.WORM)
                {
                    i++;
                    LogDebug(i.ToString());
                }
            }
        }

        private void TargetExecute(IAttackableUnit unit, bool crit)
        {
            unit.TakeDamage(itemOwner, 40 + i, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            i = 0;
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
