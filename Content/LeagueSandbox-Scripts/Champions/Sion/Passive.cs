using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class SionPassive : ICharScript
    {
        private ISpell Spell;
        private int counter;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
            ApiEventManager.OnResurrect.AddListener(this, owner, OnRessurect, false);
            Spell = spell;
        }

        public void OnDeath(IDeathData deathData)
        {
            AddBuff("SionPassiveDelay", 2f, 1, Spell, deathData.Unit, deathData.Unit as IObjAiBase);
        }

        public void OnRessurect(IObjAiBase owner)
        {
            counter++;
            //This is to avoid a loop in his passive.
            if (counter == 2)
            {
                ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
                counter = 0;
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}