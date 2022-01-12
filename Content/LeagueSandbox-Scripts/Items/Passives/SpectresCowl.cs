using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3211 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            var unit1champ = unit1 as IChampion;
            //obj.Stats.CurrentHealth += 150;
            //obj.Stats.CurrentMana += 200;
            AddBuff("SpectresCowl", 10.0f, 1, unit1champ.GetSpell(0), unit1, unit1 as IObjAiBase);
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