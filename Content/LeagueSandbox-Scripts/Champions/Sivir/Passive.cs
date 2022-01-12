using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class SivirPassive : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, PassiveMS, false);
        }

        public void PassiveMS(IAttackableUnit unit, bool boolean)
        {
            AddBuff("SivirPassive", 2.0f, 1, originspell, ownermain, ownermain);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}