
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LeaguePackets.Game
{
    public class S2C_TeamBalanceStatus : GamePacket // 0xFC
    {
        public override GamePacketID ID => GamePacketID.S2C_TeamBalanceStatus;
        public byte SurrenderReason { get; set; }
        public byte ForVote { get; set; }
        public byte AgainstVote { get; set; }
        public uint TeamID { get; set; }
        public float GoldGranted { get; set; }
        public int ExperienceGranted { get; set; }
        public int TowersGranted { get; set; }

        protected override void ReadBody(ByteReader reader)
        {

            this.SurrenderReason = reader.ReadByte();
            this.ForVote = reader.ReadByte();
            this.AgainstVote = reader.ReadByte();
            this.TeamID = reader.ReadUInt32();
            this.GoldGranted = reader.ReadFloat();
            this.ExperienceGranted = reader.ReadInt32();
            this.TowersGranted = reader.ReadInt32();
        }
        protected override void WriteBody(ByteWriter writer)
        {
            writer.WriteByte(SurrenderReason);
            writer.WriteByte(ForVote);
            writer.WriteByte(AgainstVote);
            writer.WriteUInt32(TeamID);
            writer.WriteFloat(GoldGranted);
            writer.WriteInt32(ExperienceGranted);
            writer.WriteInt32(TowersGranted);
        }
    }
}
