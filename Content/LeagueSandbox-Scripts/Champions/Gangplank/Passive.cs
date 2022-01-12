using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class Scurvy : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            var owner = ownermain;
            AddBuff("GangplankBleed", 3f, 1, originspell, unit, owner);
            LogDebug(unit.GetBuffWithName("GangplankBleed").StackCount.ToString());
        }
    }
}