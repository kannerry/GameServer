using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3156 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TargetExecute, false);
        }
        float dmg = 0;
        bool timer = false;
        private void TargetExecute(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            if (unit1.Stats.CurrentHealth < unit1.Stats.HealthPoints.Total * 0.3f)
            {
                var unit1champ = unit1 as IChampion;
                if (timer != true)
                {
                    timer = true;
                    unit1champ.ApplyShield(unit1, 400, false, true, false);
                    CreateTimer(5.0f, () => { unit1champ.ApplyShield(unit1, -400, false, true, false); });
                    CreateTimer(90.0f, () => { timer = false; });
                }
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