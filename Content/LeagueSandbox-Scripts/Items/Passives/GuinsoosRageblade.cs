using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3124 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;

            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            ApiEventManager.OnTakeDamage.AddListener(this, owner, Lifeline, false);
        }
        float dmg = 0;
        bool timer = false;
        private void Lifeline(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            if (unit1.Stats.CurrentHealth < unit1.Stats.HealthPoints.Total * 0.5f)
            {
                var unit1champ = unit1 as IChampion;
                if (timer != true)
                {
                    timer = true;
                    AddBuff("GuinsoosLifeline", 10.0f, 1, unit1champ.GetSpell(0), unit1, unit1champ);
                    CreateTimer(30.0f, () => { timer = false; });
                }
            }
        }

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            AddBuff("GuinsoosStack", 8.0f, 1, _owner.GetSpell(0), _owner, _owner);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, owner);
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}