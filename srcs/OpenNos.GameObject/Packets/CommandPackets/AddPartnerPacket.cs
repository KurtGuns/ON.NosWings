﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using NosSharp.Enums;
using OpenNos.Core.Serializing;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$AddPartner", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class AddPartnerPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short MonsterVNum { get; set; }

        [PacketIndex(1)]
        public byte Level { get; set; }

        #endregion

        #region Methods

        public static string ReturnHelp()
        {
            return "$AddPartner MONSTERVNUM LEVEL";
        }

        public override string ToString()
        {
            return $"AddPartner Command MonsterVNum: {MonsterVNum} Level: {Level}";
        }

        #endregion
    }
}