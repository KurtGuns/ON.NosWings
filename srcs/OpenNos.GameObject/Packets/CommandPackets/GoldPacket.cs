﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using NosSharp.Enums;
using OpenNos.Core.Serializing;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$Gold", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class GoldPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public long Amount { get; set; }

        public static string ReturnHelp()
        {
            return "$Gold AMOUNT";
        }

        #endregion
    }
}