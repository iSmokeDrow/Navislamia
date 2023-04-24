﻿using Navislamia.Network.Entities;
using Navislamia.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Network.Packets.Game
{
    public class TM_CS_ACCOUNT_WITH_AUTH : Packet, ISerializablePacket
    {
        public CString Account { get; set; } = new CString(61);

        public long OneTimePassword;

        public TM_CS_ACCOUNT_WITH_AUTH() : base((ushort)ClientPackets.TM_CS_ACCOUNT_WITH_AUTH) => Serialize();

        public TM_CS_ACCOUNT_WITH_AUTH(Span<byte> buffer) : base(buffer) => Deserialize();

        public void Deserialize()
        {
            Span<byte> data = Data;

            Account.Data = data.Slice(0, Account.Length).ToArray();

            OneTimePassword = BitConverter.ToInt64(data.Slice(Account.Length, sizeof(long)));
        }

        public void Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
