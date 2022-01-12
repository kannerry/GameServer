using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class Focus : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            AddBuff("PassiveMSVayne", float.MaxValue, 1, spell, owner, spell.CastInfo.Owner);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}