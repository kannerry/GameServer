using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3078 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase baseOwner;
        private bool Casted = false;

        public void OnActivate(IObjAiBase owner)
        {
            baseOwner = owner;
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[0], SpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[1], SpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[2], SpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.Spells[3], SpellCast);
            ApiEventManager.OnHitUnit.AddListener(this, baseOwner, TargetExecute, false);
        }

        public void SpellCast(ISpell spell)
        {
            Casted = true;
        }

        private void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            AddBuff("PhageMS", 2.0f, 1, baseOwner.GetSpell(0), baseOwner, baseOwner as IObjAiBase);
            if (Casted == true)
            {
                unit.TakeDamage(baseOwner, baseOwner.Stats.AttackDamage.BaseValue * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                Casted = false;
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}