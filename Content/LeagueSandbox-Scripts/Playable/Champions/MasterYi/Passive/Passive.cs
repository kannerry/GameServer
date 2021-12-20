using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class MasterYiPassive : ICharScript
    {
        private IObjAiBase _owner;
        private ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        private int i = 0;

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            i++;
            AddBuff("DoubleStrikeStacks", 4.0f, 1, _spell, _owner, _owner);

            _owner.Spells[0].LowerCooldown(1.0f);

            if (i == 4)
            {
                _owner.RemoveBuffsWithName("DoubleStrikeStacks");
                CreateTimer(0.15f, () => { SpellCast(_owner, 0, SpellSlotType.ExtraSlots, _owner.TargetUnit.Position, _owner.TargetUnit.Position, false, Vector2.Zero); });
                i = 0;
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