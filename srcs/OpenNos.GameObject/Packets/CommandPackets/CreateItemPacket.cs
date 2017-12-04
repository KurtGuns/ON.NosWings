﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core.Serializing;
using OpenNos.Domain;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$CreateItem", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class CreateItemPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short VNum { get; set; }

        [PacketIndex(1)]
        public byte? Design { get; set; }

        [PacketIndex(2)]
        public short? Upgrade { get; set; }

        public static string ReturnHelp()
        {
            return "$CreateItem ITEMVNUM DESIGN/RARE/AMOUNT/WINGS UPDATE";
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"CreateItem Command VNum: {VNum}" + Design != null ? $" Design: {Design}" : "" + Upgrade != null ? $" Upgrade: {Upgrade}" : "";
        }

        #endregion
    }
}