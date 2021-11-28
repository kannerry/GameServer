using GameServerCore.Domain.GameObjects;
using GameServerCore.Packets.Enums;

namespace PacketDefinitions420.PacketDefinitions.S2C
{
    public class ShowHpAndName : BasePacket
    {
        public ShowHpAndName(IAttackableUnit unit, bool show)
            : base((PacketCmd)0xCE, unit.NetId)
        {
            Write(show);
            Write((byte)0x00);
        }
    }
}