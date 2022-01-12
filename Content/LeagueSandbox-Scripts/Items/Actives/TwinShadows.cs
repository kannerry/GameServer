using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemSpells
{
    public class ItemWraithCollar : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        static internal IMinion minion1;
        static internal IMinion minion2;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var spellPos1 = GetPointFromUnit(owner, 200, 45);
            var spellPos2 = GetPointFromUnit(owner, 200, -45);
            var spellPos1a = GetPointFromUnit(owner, 4200, 25);
            var spellPos2a = GetPointFromUnit(owner, 4200, -25);
            minion1 = AddMinion(owner, "LuluSquill", "LuluSquill", spellPos1, targetable: false, ignoreCollision: true);
            minion2 = AddMinion(owner, "LuluSquill", "LuluSquill", spellPos2, targetable: false, ignoreCollision: true);
            ForceMovement(minion1, "troll", spellPos1a, 300, 0, 0, 0);
            ForceMovement(minion2, "face", spellPos2a, 300, 0, 0, 0);
            CreateTimer(6.0f, () => 
            { 
                minion1.TakeDamage(owner, 100000, GameServerCore.Enums.DamageType.DAMAGE_TYPE_TRUE, GameServerCore.Enums.DamageSource.DAMAGE_SOURCE_RAW, false);
                minion2.TakeDamage(owner, 100000, GameServerCore.Enums.DamageType.DAMAGE_TYPE_TRUE, GameServerCore.Enums.DamageSource.DAMAGE_SOURCE_RAW, false);
            });

        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            if(minion1 != null)
            {
                var x = GetChampionsInRange(minion1.Position, 200, true);
                foreach (var champ in x)
                {
                    if (champ.Team != _owner.Team)
                    {
                        AddBuff("TwinShadowsSlow", 2.5f, 1, _owner.GetSpell(0), champ, _owner);
                    }
                }
            }

            if(minion2 != null)
            {
                var x = GetChampionsInRange(minion2.Position, 200, true);
                foreach(var champ in x)
                {
                    if(champ.Team != _owner.Team)
                    {
                        AddBuff("TwinShadowsSlow", 2.5f, 1, _owner.GetSpell(0), champ   , _owner);
                    }
                }
            }

        }
    }
}