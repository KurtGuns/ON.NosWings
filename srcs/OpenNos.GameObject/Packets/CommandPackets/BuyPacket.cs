﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using NosSharp.Enums;
using OpenNos.Core.Serializing;
namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$Buy", PassNonParseablePacket = true, Authority = AuthorityType.User)]
    public class BuyPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public string Item { get; set; }

        [PacketIndex(1)]
        public int Amount { get; set; }

        public static string ReturnHelp()
        {
            return "$Buy <Item> <Amount>";
        }

        #endregion

    }
}