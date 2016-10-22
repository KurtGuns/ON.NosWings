﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)
using OpenNos.Core;

namespace OpenNos.GameObject
{
    [PacketHeader("effect")]
    public class EffectPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte EffectType { get; set; }

        [PacketIndex(1)]
        public long Id { get; set; }

        [PacketIndex(2)]
        public short EffectId { get; set; }

        #endregion
    }
}
