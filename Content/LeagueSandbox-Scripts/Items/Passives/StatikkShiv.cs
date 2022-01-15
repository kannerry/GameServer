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

namespace ItemPassives
{
    public class ItemID_3087 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase ItemOwner;
        public void OnActivate(IObjAiBase owner)
        {
            IChampion _owner = owner as IChampion;
            ItemOwner = owner;
            AddBuff("StaticFieldCooldown", 7.0f, 1, owner.GetSpell(0), owner, ItemOwner);
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, TiamatExecute, false);
        }
        int i = 0;
        private void TiamatExecute(ISpell spell)
        {
            if (HasBuff(spell.CastInfo.Owner, "StaticField"))
            {

                ItemOwner.RemoveBuffsWithName("StaticField");
                AddBuff("StaticFieldCooldown", 7.0f, 1, ItemOwner.GetSpell(0), ItemOwner, ItemOwner);

                var target = spell.CastInfo.Targets[0].Unit;
                var x = GetUnitsInRange(target.Position, 600, true);
                foreach (var unit in x)
                {
                    if (unit.Team != ItemOwner.Team)
                    {
                        if (i < 4)
                        {
                            i++;
                            AddParticle(ItemOwner, unit, "volibear_R_chain_lighting_01.troy", target.Position);
                            unit.TakeDamage(ItemOwner, 100, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        }
                        CreateTimer(0.01f, () => { i = 0; });
                    }
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}