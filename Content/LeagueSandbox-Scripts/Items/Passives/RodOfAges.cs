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
//Forbidden Idol
namespace ItemPassives
{
    public class ItemID_3027 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            SOLD = false;
            int i = 0;
            while (i++ < 10)
            {
                AddBuff("RodOfAgesCooldown", i * 60, 1, owner.GetSpell(0), owner, owner);
            }
            //AddBuff("RodOfAgesCooldown", 60f, 1, owner.GetSpell(0), owner, owner);
        }
        static internal bool SOLD;
        public void OnDeactivate(IObjAiBase owner)
        {
            //TODO
            //IF YOU WAIT 1 MINUTE
            //AND SELL ROD, YOU GET
            // ALL STACKS?

            // SOLD IS WORKAROUND IF YOU INSTA SELL
            SOLD = true;
            var b1 = owner.GetBuffsWithName("RodOfAgesCooldown");
            var b2 = owner.GetBuffsWithName("RodOfAgesCounter");
            foreach(var b in b1)
            {
                b.DeactivateBuff();
            }
            foreach (var b in b2)
            {
                //b.DeactivateBuff();
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
