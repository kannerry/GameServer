
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LeaguePackets.Game
{
    public class ReloadScripts : GamePacket, IUnusedPacket // 0xAC
    {
        public override GamePacketID ID => GamePacketID.ReloadScripts;

        protected override void ReadBody(ByteReader reader)
        {
        }
        protected override void WriteBody(ByteWriter writer)
        {
        }
    }
}
