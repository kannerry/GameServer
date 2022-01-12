using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Passives
{
    public class Battle_Fury : ICharScript
    {
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            owner.Stats.CurrentMana = 1;
            ApiEventManager.OnHitUnit.AddListener(this, owner, AddFury, false);
        }

        public void AddFury(IAttackableUnit unit, bool boolean)
        {
            LogDebug(boolean.ToString());
            if(boolean == true)
            {
                _owner.Stats.CurrentMana += 10;
            }
            if(boolean == false)
            {
                _owner.Stats.CurrentMana += 5;
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            _owner.Stats.AttackDamage.FlatBonus = (float)(_owner.Stats.CurrentMana * 0.25);
            _owner.Stats.CriticalChance.FlatBonus = (float)(_owner.Stats.CurrentMana * 0.004);
        }
    }
}