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

namespace ItemPassives
{
    public class ItemID_3141 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        static internal bool occult;
        public void OnActivate(IObjAiBase owner)
        {
            occult = true;
            _owner = owner;
            int f = 0;
            if (!owner.HasBuff("MejaisCap"))
            {
                while (f++ < 5)
                    AddBuff("MejaisCap", float.MaxValue, 1, owner.GetSpell(0), owner, owner, true);
            }
            ApiEventManager.OnKill.AddListener(this, owner, MejaisStack, false);
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, false);
        }
        IObjAiBase _owner;
        public void MejaisStack(IDeathData deathdata)
        {
            IObjAiBase owner = deathdata.Killer as IObjAiBase;
            AddBuff("MejaisCap", float.MaxValue, 1, owner.GetSpell(0), owner, owner, true);
            AddBuff("MejaisCap", float.MaxValue, 1, owner.GetSpell(0), owner, owner, true);
            LogDebug("yo");
        }

        public void OnDeath(IDeathData deathData)
        {
            int i = 0;
            LogDebug("died");
            while (i < _owner.GetBuffWithName("MejaisCap").StackCount)
            {
                i++;
                if (i == _owner.GetBuffWithName("MejaisCap").StackCount)
                {

                    RemoveStacks(i / 2);
                }
            }
        }

        public void RemoveStacks(int var)
        {
            int x = 0;
            foreach (var swag in _owner.GetBuffsWithName("MejaisCap"))
            {
                if (x < var)
                {
                    x++;
                    swag.DeactivateBuff();
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            occult = false;
            ApiEventManager.OnKill.RemoveListener(this);
            ApiEventManager.OnDeath.RemoveListener(this);
            int i = 0;
            while (i++ < 25) {
                var x = _owner.GetBuffsWithName("MejaisCap");
                foreach(var buff in x)
                {
                    buff.DeactivateBuff();
                }
            }
        }

        public void OnUpdate(float diff)
        {
            if (_owner.HasBuff("MejaisCap"))
            {
                //LogDebug(_owner.GetBuffWithName("MejaisCap").StackCount.ToString());
            }
        }
    }
}
