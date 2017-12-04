﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core.Serializing;
using OpenNos.Domain;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$ShoutHere", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class ShoutHerePacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0, SerializeToEnd = true)]
        public string Message { get; set; }

        public static string ReturnHelp()
        {
            return "$ShoutHere MESSAGE";
        }

        #endregion
    }
}