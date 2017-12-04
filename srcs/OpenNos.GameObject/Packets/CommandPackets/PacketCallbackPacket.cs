﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core.Serializing;
using OpenNos.Domain;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$Packet", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class PacketCallbackPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0, SerializeToEnd = true)]
        public string Packet { get; set; }

        public static string ReturnHelp()
        {
            return "$Packet PACKET";
        }

        #endregion
    }
}