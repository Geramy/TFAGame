using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TFAGame.Network
{
    public class PacketReader
    {
        internal event PacketResponse OnEvent;

        protected PacketArguments PacketArgs
        {
            get;
            set;
        }

        public PacketClient sock;

        private bool ClientPacket
        {
            get { return PacketArgs.ClientPacket; }
        }

        private PacketType packetType
        {
            get { return PacketArgs.packetType; }
        }

        private int PacketLength
        {
            get { return PacketArgs.PacketLength; }
        }

        public Packet(PacketType pType, PacketClient client, bool ClientProtocolPacket, PacketArguments args)
        {
            PacketArgs = args;
            sock = client;
            PacketArgs.packetType = pType;
            PacketArgs.ClientPacket = ClientProtocolPacket;
        }

        protected byte ReadByte()
        {
            byte[] singleB = new byte[1];
            sock.sClient.Receive(singleB);
            return singleB[0];
        }

        protected bool ReadBool()
        {
            return ReadByte() == 0x00 ? false : true;
        }

        protected short ReadShort()
        {
            byte[] ShortData = new byte[2];
            sock.sClient.Receive(ShortData);
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ShortData, 0));
        }

        protected int ReadInt()
        {
            byte[] IntData = new byte[4];
            sock.sClient.Receive(IntData);
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(IntData, 0));
        }

        protected decimal ReadDecimal()
        {
            return (decimal)double.Parse(ReadString());
        }

        protected long ReadLong()
        {
            byte[] LongData = new byte[8];
            sock.sClient.Receive(LongData);
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(LongData, 0));
        }

        protected string ReadString()
        {
            int StrLength = ReadInt();
            byte[] StringData = new byte[StrLength];
            sock.sClient.Receive(StringData);
            return ASCIIEncoding.UTF8.GetString(StringData);
        }

        public static PacketType ReadPacketType(PacketClient cl)
        {
            byte[] ShortData = new byte[2];
            cl.sClient.Receive(ShortData);
            return (PacketType)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ShortData, 0));
        }

        protected void WriteByte(byte value)
        {
            sock.sClient.Send(new byte[] { value });
        }

        protected void WriteBool(bool value)
        {
            WriteByte((byte)(value ? 0x01 : 0x00));
        }

        protected void WriteShort(short value)
        {
            sock.sClient.Send(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
        }

        protected void WriteInt(int value)
        {
            sock.sClient.Send(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
        }

        protected void WriteDecimal(decimal value)
        {
            WriteString("" + value);
        }

        protected void WriteLong(long value)
        {
            sock.sClient.Send(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
        }

        protected void WriteString(string value)
        {
            byte[] stringData = ASCIIEncoding.UTF8.GetBytes(value);
            WriteInt(stringData.Length);
            sock.sClient.Send(stringData);
        }

        public void ReadPacket()
        {
            if (ClientPacket)
                ReadClientPacket();
            else
                ReadServerPacket();
        }

        public void WritePacket()
        {
            if (ClientPacket)
                WriteClientPacket();
            else
                WriteServerPacket();
        }

        protected void TriggerOnEvent()
        {
            OnEvent(packetType, PacketArgs, ClientPacket, sock);
        }

        /// <summary>
        /// This will get our argument for the current packet type
        /// </summary>
        /// <param name="ClientPacket"></param>
        /// <param name="Read"></param>
        /// <returns></returns>
        protected static PacketArguments GetNewArgument(bool ClientPacket, bool Read)
        {
            return new PacketArguments();
        }

        /// <summary>
        /// This reads the packet sent from the server.
        /// </summary>
        protected virtual void ReadClientPacket()
        {
            byte[] readData = new byte[sock.sClient.Available];
            sock.sClient.Receive(readData);
        }

        /// <summary>
        /// This reads the packet sent from the client
        /// </summary>
        protected virtual void ReadServerPacket()
        {
            byte[] readData = new byte[sock.sClient.Available];
            sock.sClient.Receive(readData);
        }

        /// <summary>
        /// This writes the client packet send to the server.
        /// </summary>
        protected virtual void WriteClientPacket()
        {
            WriteShort((short)packetType);
        }

        /// <summary>
        /// This writes the server packet that is sent to the client
        /// </summary>
        protected virtual void WriteServerPacket()
        {
            WriteShort((short)packetType);
        }
    }
}
