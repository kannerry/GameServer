using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Passives
{
    public class JarvanIVMartialCadence : ICharScript
    {

        private IObjAiBase _owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }
        int i = 0;
        public void TargetExecute(IAttackableUnit target, bool isCrit)
        {
            if (!target.HasBuff("JarvanCD"))
            {
                target.TakeDamage(_owner, 100, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            }
            AddBuff("JarvanCD", 10.0f, 1, _owner.GetSpell(0), target, _owner);

        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}