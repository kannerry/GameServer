using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Passives
{
    public class EzrealRisingSpellForceBuff : ICharScript
    {
        ISpell _spell;
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHit, false);
        }

        public void OnHit(IAttackableUnit unit, bool spell)
        {
            LogDebug("yo");

            //AddBuff("EzrealRisingSpellForce", 6.0f, 1, _spell, _owner, _owner);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}