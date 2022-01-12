using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemSpells
{
    public class ItemRighteousGlory : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            AddParticlePos(owner, "Global_Item_RighteousGlory_Cas.troy", owner.Position, owner.Position, size: 2);
            AddParticleTarget(owner, owner, "Global_Item_RighteousGlory_PreAoe.troy", owner, lifetime: 3.0f);

            CreateTimer(3.0f, () =>
            {
                AddParticleTarget(owner, owner, "Global_Item_RighteousGlory_Aoe.troy", owner, lifetime: 3.0f);
                var champs = GetChampionsInRange(owner.Position, 750, true);
                foreach (var champ in champs)
                {
                    if (champ.Team != owner.Team)
                    {
                        AddBuff("RighteousGlorySlow", 1.0f, 1, spell, champ, owner);
                    }
                }
            });

            var champs = GetChampionsInRange(owner.Position, 750, true);
            foreach(var champ in champs)
            {
                if(champ.Team == owner.Team)
                {
                    AddBuff("RighteousGloryMS", 6.0f, 1, spell, champ, owner);
                }
            }
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
        }
    }
}