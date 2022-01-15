using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class RenektonPredator : ICharScript
    {

        IObjAiBase _owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            _owner = owner;
            _owner.Stats.CurrentMana = 1;
        }

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            _owner.Stats.CurrentMana += 5;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}