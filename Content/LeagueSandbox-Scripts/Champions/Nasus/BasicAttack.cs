using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Spells
{
    public class NasusBasicAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };
        ISpell _spell;
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnHitUnit.AddListener(this, owner, hit, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        bool add3 = false;
        bool add6 = false;

        //CBAAAAAA MAN
        // WILL MAKE BETTER AT A LATER DATE

        public void hit(IAttackableUnit unit, bool crit)
        {
            LogDebug("yo");
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);
            if(NasusQAttack.Applied == 0)
            {
                LogDebug("q");
                CreateTimer(0.1f, () => 
                { 
                    if (unit.IsDead) 
                    {
                        LogDebug("dead to q"); 
                        if(unit is IChampion)
                        {
                            add6 = true;
                        }
                        else if(unit is ILaneMinion)
                        {
                            var unitminion = unit as ILaneMinion;
                            if(unitminion.MinionSpawnType == MinionSpawnType.MINION_TYPE_SUPER)
                            {
                                add6 = true;
                            }
                            if(unitminion.MinionSpawnType == MinionSpawnType.MINION_TYPE_CANNON)
                            {
                                add6 = true;
                            }
                            else
                            {
                                add3 = true;
                            }
                        }
                        else
                        {
                            add3 = true;
                        }
                    } 
                });
            }

        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);


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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void StackQ(int amt)
        {
            int i = 0;
            while (i < amt)
            {
                AddBuff("NasusQStacks", float.MaxValue, 1, _spell, _owner, _owner, true);
                i++;
            }
        }

        public void OnUpdate(float diff)
        {
            if(add3 == true)
            {
                StackQ(3);
                add3 = false;
            }
            if(add6 == true)
            {
                StackQ(6);
                add6 = false;
            }
        }
    }

    public class NasusBasicAttack2 : ISpellScript
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
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack", false);
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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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