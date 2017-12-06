﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using NosSharp.Enums;
using OpenNos.Core.Serializing;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$Clear", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class ClearInventoryPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        public static string ReturnHelp()
        {
            return "$Clear INVENTORYTYPE";
        }

        #endregion
    }
}