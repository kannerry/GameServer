using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3151 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;
            int i = 0;
            while (i++ < 60)
            {
                ApiEventManager.OnSpellHit.AddListener(this, owner.GetSpell((byte)(i)), Burn, false);
            }

            StatsModifier.MagicPenetration.FlatBonus += 10;
            owner.AddStatModifier(StatsModifier);
        }

        public void Burn(ISpell spell, IAttackableUnit unit, ISpellMissile mis, ISpellSector sec)
        {
            if (!spell.SpellName.Contains("BasicAttack"))
            {
                if (spell.CastInfo.Targets[0].Unit != null)
                {
                    var unitspell = spell.CastInfo.Targets[0].Unit;
                    AddParticle(unitspell, unitspell, "AcidBurn_aura.troy", unitspell.Position, lifetime: 3.0f);
                    AddBuff("LiandrysBurn", 3f, 1, _owner.GetSpell(0), unitspell, _owner);
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}