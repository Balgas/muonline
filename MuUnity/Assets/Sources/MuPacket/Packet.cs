namespace MuPacket 
{ 
    public class Packet 
    { 
        #region Must set properties 
        public byte[] Source { get; set; } 
        public int Index { get; set; } 
        public bool ToServer { get; set; } 
        #endregion 

        #region Read-only properties 
        public bool IsValidPacket { get { return Size != 0 && HeaderSize > 0; } } 

        public int ContentIndex { get { return Index + HeaderSize; } } 
        public ushort Size { get { return GetPacketSize(Source, Index); } } 
        public ushort ContentSize { get { return (ushort)(Size - HeaderSize); } } 
        public ushort EncSpace { get { ushort x = (ushort)(ContentSize + 1); return (ushort)((((x / 8) + (((x % 8) > 0) ? 1 : 0)) * 11) + HeaderSize); } } 
        public ushort DecSpace { get { return (ushort)(((ContentSize / 11) * 8) + HeaderSize - 1); } } 

        public byte Header { get { return Source[Index]; } } 
        public byte HeaderSize { get { return GetHeaderSize(Source, Index); } } 
        public ushort PacketType { get { try { return (ushort)(Source[ContentIndex] * 0x100 + Source[ContentIndex + 1]); } catch { return 0; } } } 
        #endregion 

        public byte Counter { get; set; } // use for C3C4 packet encrypt / decrypt only 

        public Packet(byte[] source, int index, bool tos = false, byte counter = 0) 
        { 
            Source = source; 
            Index = index; 
            ToServer = tos; 
            Counter = counter; 
        } 

        #region Method 
        public byte GetData(int index) { return Source[Index + index]; } 
        public void SetData(int index, byte value) { Source[Index + index] = value; } 

        static byte GetHeaderSize(byte[] data, int idx) 
        { 
            if (data[idx] == 0xC1 || data[idx] == 0xC3) return 2; 
            if (data[idx] == 0xC2 || data[idx] == 0xC4) return 3; 
            return 0;// shouldn't occur  
        } 
        static ushort GetPacketSize(byte[] data, int idx) 
        { 
            if (data[idx] == 0xC1 || data[idx] == 0xC3) return data[idx + 1]; ; 
            if (data[idx] == 0xC2 || data[idx] == 0xC4) return (ushort)(data[idx + 1] * 0x100 + data[idx + 2]); 
            return 0;// shouldn't occur  
        }  
        #endregion 
    } 
}  