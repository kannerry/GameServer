using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3075 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            var unit1champ = unit1 as IChampion;
            if(unit2 is IChampion)
            {
                unit2.TakeDamage(unit1, unit2.Stats.AttackDamage.Total * 0.3f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}