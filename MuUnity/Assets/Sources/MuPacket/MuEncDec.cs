using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.IO; 

namespace MuPacket 
{ 
    public static class MuEncDec 
    { 
        #region Constant data 
        public enum ReadEncfileResult { InvalidPath, FileCorrupted, Success } 
        /// <summary> 
        /// MuError.log 
        /// </summary> 
        
        //static readonly uint[] xor_tab_muerror = new uint[] { 0x9F81BD7C, 0x56E2933D, 0x3ED2732A, 0xBF9583F2 }; 
		public static readonly uint MAP_FILE_SIZE = 65536*3+2;
       	public static readonly uint ATT_FILE_65KB_SIZE = 65536+4;
		public static readonly uint ATT_FILE_129KB_SIZE = 65536*2+4;
		
		/// <summary> 
        /// used to decrypt login and password, Terrain*.att files in 
        /// client-side Data/World*/ 
        /// </summary> 
        static readonly byte[] xor_table_3byte = new byte[] { 0xFC, 0xCF, 0xAB }; 
        /// <summary> 
        /// used to decrypt client-side Data/Enc1.dat and Data/Dec2.Dat 
        /// </summary> 
        static readonly uint[] xor_tab_datfile = new uint[] { 0x3F08A79B, 0xE25CC287, 0x93D27AB9, 0x20DEA7BF }; 
        /// <summary> 
        /// used to decrypt C1/C2 packets 
        /// </summary> 
        static readonly byte[] xor_tab_C1C2 = new byte[] { 
            0xE7, 0x6D, 0x3A, 0x89, 0xBC, 0xB2, 0x9F, 0x73, 
            0x23, 0xA8, 0xFE, 0xB6, 0x49, 0x5D, 0x39, 0x5D, 
            0x8A, 0xCB, 0x63, 0x8D, 0xEA, 0x7D, 0x2B, 0x5F, 
            0xC3, 0xB1, 0xE9, 0x83, 0x29, 0x51, 0xE8, 0x56 
        };
		static readonly byte[] xor_tab_file = new byte[] { 
            0xd1, 0x73, 0x52, 0xf6,
			0xd2, 0x9a, 0xcb, 0x27,
			0x3e, 0xaf, 0x59, 0x31,
			0x37, 0xb3, 0xe7, 0xa2
        }; 
        #endregion 

        #region Need to load data 
        static uint[] enc1_keys = new uint[12]; 
        static uint[] enc2_keys = new uint[12]; 
        static uint[] dec1_keys = new uint[12]; 
        static uint[] dec2_keys = new uint[12]; 
        public static bool LoadedKey { get; set; } 
        #endregion 

        #region Basic memmory edit method 
        /// <summary> 
        /// decrypts login/passwod, and also used to decrypt  
        /// Terrain*.att files in client-side Data/World*/ 
        /// </summary> 
        static void MuXor3Byte(byte[] buffer, int ptr, uint len) 
        { 
            for (uint i = 0; i < len; ++i) 
                buffer[ptr + i] ^= xor_table_3byte[i % 3]; 
        } 
		public static void MuXorFile2(ref byte[] buffer, uint len) 
        { 
            for (uint i = 0; i < len; ++i) 
                buffer[i] ^= xor_table_3byte[i % 3]; 
        } 
		public static void MuXorFile(ref byte[] buffer, uint len) 
        {	
			byte key = 0x5E;
            for (uint i = 0; i < len; ++i) {
				byte encode = buffer[i];
                buffer[i] ^= xor_tab_file[i % 16]; 
				buffer[i] -= key;
				key = (byte)((byte)encode+(byte)0x3D);
			}
        }
        static void ShiftRight(byte[] buffer, int ptr, uint len, uint shift) 
        { 
            if (shift == 0) return; 
            for (int i = 1; i < len; ++i) 
            { 
                buffer[ptr] = (byte)((buffer[ptr] << (int)shift) | (buffer[ptr + 1] >> (8 - (int)shift))); 
                ++ptr; 
            } 
            buffer[ptr] <<= (int)shift; 
        } 
        static void ShiftLeft(byte[] buffer, int ptr, uint len, uint shift) 
        { 
            if (shift == 0) return; 
            ptr += (int)len - 1; 
            for (int i = 1; i < len; ++i) 
            { 
                buffer[ptr] = (byte)((buffer[ptr] >> (int)shift) | (buffer[ptr - 1] << (8 - (int)shift))); 
                --ptr; 
            } 
            buffer[ptr] >>= (int)shift; 
        } 
        static int ShiftBytes(byte[] buffer, int buf, uint arg_4, byte[] packet, int pkt, uint arg_C, uint arg_10) 
        { 
            uint size_ = ((((arg_10 + arg_C) - 1) / 8) + (1 - (arg_C / 8))); 
            byte[] tmp1 = new byte[20]; 
            Array.Copy(packet, pkt + arg_C / 8, tmp1, 0, size_);
            uint var_4 = (arg_10 + arg_C) & 0x7; 
            if (var_4 != 0) tmp1[size_ - 1] &= (byte)(0xFF << (8 - (int)var_4)); 
            arg_C &= 0x7; 
            ShiftRight(tmp1, 0, size_, arg_C); 
            ShiftLeft(tmp1, 0, size_ + 1, arg_4 & 0x7); 
            if ((arg_4 & 0x7) > arg_C) 
                ++size_; 
            if (size_ > 0) 
                for (int i = 0; i < size_; ++i) 
                    buffer[buf + i + (arg_4 / 8)] |= tmp1[i];
            return (int)(arg_10 + arg_4); 
        } 
        static void Encode8BytesTo11Bytes(byte[] outbuffer, int outbuf, byte[] packet, int pktptr, uint num_bytes, uint[] dec_dat) 
        { 
            byte[] finale = new byte[2]; 
            finale[0] = (byte)num_bytes; 
            finale[0] ^= 0x3D; 
            finale[1] = 0xF8; 
            for (int k = 0; k < num_bytes; ++k)
                finale[1] ^= packet[pktptr + k]; 
            finale[0] ^= finale[1]; 
            ShiftBytes(outbuffer, outbuf, 0x48, finale, 0, 0x00, 0x10); 
            uint[] ring = new uint[4]; 
            ushort[] cryptbuf = new ushort[4];
            for (int i = 0; i < num_bytes; i += 2) 
            { 
                cryptbuf[i / 2] = (ushort)(packet[pktptr + i]); 
                if (i + 1 < num_bytes) 
                    cryptbuf[i / 2] += (ushort)(packet[pktptr + i + 1] * 0x100); 
            } 
            ring[0] = ((dec_dat[8] ^ (cryptbuf[0])) * dec_dat[4]) % dec_dat[0]; 
            ring[1] = ((dec_dat[9] ^ (cryptbuf[1] ^ (ring[0] & 0xFFFF))) * dec_dat[5]) % dec_dat[1]; 
            ring[2] = ((dec_dat[10] ^ (cryptbuf[2] ^ (ring[1] & 0xFFFF))) * dec_dat[6]) % dec_dat[2]; 
            ring[3] = ((dec_dat[11] ^ (cryptbuf[3] ^ (ring[2] & 0xFFFF))) * dec_dat[7]) % dec_dat[3]; 
            uint[] ring_backup = ring.ToArray(); 
            ring[2] = ring[2] ^ dec_dat[10] ^ (ring_backup[3] & 0xFFFF); 
            ring[1] = ring[1] ^ dec_dat[9] ^ (ring_backup[2] & 0xFFFF); 
            ring[0] = ring[0] ^ dec_dat[8] ^ (ring_backup[1] & 0xFFFF); 
            byte[] subring = new byte[16]; 
            for (int i = 0; i < 4; i++) 
            { 
                subring[i * 4] = (byte)(ring[i] % 0x100); 
                subring[i * 4 + 1] = (byte)(ring[i] / 0x100); 
                subring[i * 4 + 2] = (byte)(ring[i] / 0x10000); 
                subring[i * 4 + 3] = (byte)(ring[i] / 0x1000000); 
            } 
            ShiftBytes(outbuffer, outbuf, 0x00, subring, 0, 0x00, 0x10); 
            ShiftBytes(outbuffer, outbuf, 0x10, subring, 0, 0x16, 0x02); 
            ShiftBytes(outbuffer, outbuf, 0x12, subring, 4, 0x00, 0x10); 
            ShiftBytes(outbuffer, outbuf, 0x22, subring, 4, 0x16, 0x02); 
            ShiftBytes(outbuffer, outbuf, 0x24, subring, 8, 0x00, 0x10); 
            ShiftBytes(outbuffer, outbuf, 0x34, subring, 8, 0x16, 0x02); 
            ShiftBytes(outbuffer, outbuf, 0x36, subring, 12, 0x00, 0x10); 
            ShiftBytes(outbuffer, outbuf, 0x46, subring, 12, 0x16, 0x02); 
        } 
        static int Decode11BytesTo8Bytes(byte[] outbuffer, int outbuf, byte[] packet, int pktptr, uint[] dec_dat) 
        { 
            uint[] ring = new uint[] { 0x00000000, 0x00000000, 0x00000000, 0x00000000 }; 
            byte[] subr = new byte[16]; 
            ShiftBytes(subr, 0, 0x00, packet, pktptr, 0x00, 0x10); 
            ShiftBytes(subr, 0, 0x16, packet, pktptr, 0x10, 0x02); 
            ShiftBytes(subr, 4, 0x00, packet, pktptr, 0x12, 0x10); 
            ShiftBytes(subr, 4, 0x16, packet, pktptr, 0x22, 0x02); 
            ShiftBytes(subr, 8, 0x00, packet, pktptr, 0x24, 0x10); 
            ShiftBytes(subr, 8, 0x16, packet, pktptr, 0x34, 0x02); 
            ShiftBytes(subr, 12, 0x00, packet, pktptr, 0x36, 0x10); 
            ShiftBytes(subr, 12, 0x16, packet, pktptr, 0x46, 0x02); 
            //copy subring to ring 
            for (int i = 0; i < 4; i++) 
                ring[i] = (uint)(subr[i * 4] + subr[i * 4 + 1] * 0x100 + subr[i * 4 + 2] * 0x10000 + subr[i * 4 + 3] * 0x1000000); 
            ring[2] = ring[2] ^ dec_dat[10] ^ (ring[3] & 0xFFFF); 
            ring[1] = ring[1] ^ dec_dat[9] ^ (ring[2] & 0xFFFF); 
            ring[0] = ring[0] ^ dec_dat[8] ^ (ring[1] & 0xFFFF); 
            //ushort* cryptbuf =(ushort*)outbuf; 
            ushort[] cryptbuf = new ushort[4]; 
            cryptbuf[0] = (ushort)(dec_dat[8] ^ ((ring[0] * dec_dat[4]) % dec_dat[0])); 
            cryptbuf[1] = (ushort)(dec_dat[9] ^ ((ring[1] * dec_dat[5]) % dec_dat[1]) ^ (ring[0] & 0xFFFF)); 
            cryptbuf[2] = (ushort)(dec_dat[10] ^ ((ring[2] * dec_dat[6]) % dec_dat[2]) ^ (ring[1] & 0xFFFF)); 
            cryptbuf[3] = (ushort)(dec_dat[11] ^ ((ring[3] * dec_dat[7]) % dec_dat[3]) ^ (ring[2] & 0xFFFF)); 
            //update outbuffer from cryptbuff 
            outbuffer[outbuf] = (byte)(cryptbuf[0] % 0x100); 
            outbuffer[outbuf + 1] = (byte)(cryptbuf[0] / 0x100); 
            outbuffer[outbuf + 2] = (byte)(cryptbuf[1] % 0x100); 
            outbuffer[outbuf + 3] = (byte)(cryptbuf[1] / 0x100); 
            outbuffer[outbuf + 4] = (byte)(cryptbuf[2] % 0x100); 
            outbuffer[outbuf + 5] = (byte)(cryptbuf[2] / 0x100); 
            outbuffer[outbuf + 6] = (byte)(cryptbuf[3] % 0x100); 
            outbuffer[outbuf + 7] = (byte)(cryptbuf[3] / 0x100); 

            byte[] finale = new byte[] { 0x00, 0x00 }; 
            ShiftBytes(finale, 0, 0, packet, pktptr, 0x48, 0x10); 
            finale[0] ^= finale[1]; 
            finale[0] ^= 0x3D; 
            byte m = 0xF8; 
            for (int k = 0; k < 8; ++k) 
                m ^= (byte)(outbuffer[outbuf + k]); 
            if (m == finale[1]) 
                return finale[0]; 
            return -1; 
        } 
        #endregion 

        #region Backround Encrypt - Decrypt 
        static int MuPacketEncSpace(Packet pkt) 
        { 
            ushort x = (ushort)(pkt.ContentSize + 1); 
            return (((x / 8) + (((x % 8) > 0) ? 1 : 0)) * 11) + pkt.HeaderSize; 
        } 
        static int MuPacketDecSpace(Packet pkt) 
        { 
            return ((pkt.ContentSize / 11) * 8) + pkt.HeaderSize - 1; 
        } 

        /// <summary> 
        /// (if C1, offset = 2), (if C2, offset = 3) 
        /// </summary> 
        static void MU_ForceEncodeC1C2(byte[] buffer, int buf, int len, int offset = 2) 
        { 
            for (int p = 1; p < len; ++p) 
                buffer[buf + p] ^= (byte)(buffer[buf + p - 1] ^ xor_tab_C1C2[(p + offset) % 32]); 
        } 
        /// <summary> 
        /// (if C1, offset = 2), (if C2, offset = 3) 
        /// </summary> 
        static void MU_ForceDecodeC1C2(byte[] buffer, int buf, int len, int offset = 2) 
        { 
            --len; 
            for (int p = len; p > 0; --p) 
                buffer[buf + p] ^= (byte)(buffer[buf + p - 1] ^ xor_tab_C1C2[(p + offset) % 32]); 
        } 
        /// <summary> 
        /// encode c1/c2 packet 
        /// </summary> 
        static void MU_EncodeC1C2(Packet packet) 
        { 
            MU_ForceEncodeC1C2(packet.Source, packet.ContentIndex, packet.ContentSize, packet.HeaderSize); 
        } 
        /// <summary> 
        /// decode c1/c2 packet 
        /// </summary> 
        static void MU_DecodeC1C2(Packet packet) 
        { 
            MU_ForceDecodeC1C2(packet.Source, packet.ContentIndex, packet.ContentSize, packet.HeaderSize); 
        } 
        /// <summary> 
        /// encode c3/c4 packet 
        /// </summary> 
        static bool MU_ForceEncodeC3C4(byte[] outbuffer, int outbuf, ref ushort outlen, byte[] inbuffer, int inbuf, ushort len, uint[] dec_dat) 
        { 
            outlen = 0; 
            uint offset = 0; 
            for (offset = 0; (offset + 8) <= len; offset += 8) 
            { 
                //memset( outbuf, 0, 11); 
                Array.Clear(outbuffer, outbuf, 11); 
                Encode8BytesTo11Bytes(outbuffer, outbuf, inbuffer, (int)(inbuf + offset), 8, dec_dat); 
                outlen += 11; 
                outbuf += 11; 
            } 
            if (offset < len) 
            { 
                //memset( outbuf, 0, 11); 
                Array.Clear(outbuffer, outbuf, 11); 
                Encode8BytesTo11Bytes(outbuffer, outbuf, inbuffer, (int)(inbuf + offset), (uint)(len - offset), dec_dat); 
                outlen += 11; 
            } 
            return true; 
        } 
        /// <summary> 
        /// decode c3/c4 packet 
        /// </summary> 
        static bool MU_ForceDecodeC3C4(byte[] outbuffer, int outbuf, ref ushort outlen, byte[] inbuffer, int inbuf, ushort len, uint[] dec_dat) 
        { 
            if (len % 11 != 0) 
                return false;// invalid size specified 
            outlen = 0; 
            int rez = 0; 
            for (int offset = 0; offset < len; offset += 11) 
            { 
                rez = Decode11BytesTo8Bytes(outbuffer, outbuf, inbuffer, inbuf + offset, dec_dat); 
                if (rez <= 0) return false;// failed to decrypt 
                outlen += (ushort)rez; 
                outbuf += 8; 
            } 
            return true; 
        } 
        /// <summary> 
        /// decrypt c3/c4 packet 
        /// </summary> 
        static bool MU_DecodeC3C4(byte[] outbuffer, int outbuf, Packet pkt, uint[] dec_dat, ref byte dec_key) 
        { 
            byte hdrSize = pkt.HeaderSize; 
            byte hdr = pkt.Header; 
            ushort dec_size = 0; 
            if (MU_ForceDecodeC3C4(outbuffer, outbuf + hdrSize - 1, ref dec_size, pkt.Source, pkt.ContentIndex, pkt.ContentSize, dec_dat) == false) 
                return false;// decryption fails 
            dec_size += (ushort)(hdrSize - 1); 
            dec_key = outbuffer[outbuf + hdrSize - 1]; 
            outbuffer[outbuf] = (byte)(hdr - 2); 
            if (hdrSize == 2) 
                outbuffer[outbuf + 1] = (byte)dec_size; 
            else 
            { 
                outbuffer[outbuf + 1] = (byte)((dec_size & ~0x00FF) >> 8); 
                outbuffer[outbuf + 2] = (byte)(dec_size & ~0xFF00); 
            } 
            return true;// decrypt success 
        } 
        /// <summary> 
        /// encrypt c3/c4 packet 
        /// </summary> 
        static bool MU_EncodeC3C4(byte[] outbuffer, int outbuf, Packet pkt, uint[] dec_dat, byte enc_key) 
        { 
            byte hdrSize = pkt.HeaderSize; 
            byte hdr = pkt.Header; 
            byte tmp = pkt.Source[hdrSize - 1]; 
            ushort enc_len = 0; 
            ushort size = pkt.Size; 
            pkt.Source[hdrSize - 1] = enc_key; 
            bool rs = MU_ForceEncodeC3C4(outbuffer, outbuf + hdrSize, ref enc_len, pkt.Source, pkt.Index + hdrSize - 1, (ushort)(size - hdrSize + 1), dec_dat); 
            pkt.Source[hdrSize - 1] = tmp; 
            if (rs == true) 
            { 
                outbuffer[outbuf] = (byte)(hdr + 2); 
                enc_len += (ushort)hdrSize; 
                if (hdrSize == 2) 
                    outbuffer[outbuf + 1] = (byte)enc_len; 
                else 
                { 
                    outbuffer[outbuf + 1] = (byte)((enc_len & ~0x00FF) >> 8); 
                    outbuffer[outbuf + 2] = (byte)(enc_len & ~0xFF00); 
                } 
            } 
            return rs; 
        } 
        #endregion 

        #region Read Enc client file 
        /// <summary> 
        /// out_dat element count = 16 
        /// </summary> 
        static ReadEncfileResult ReadEncfile(string file, uint[] out_dat) 
        {	
			byte[] stream = Util.Storage.Load(file);
			
			if (stream == null) return ReadEncfileResult.InvalidPath; 
			
			
            long size = stream.Length; 
            if (size != 54) return ReadEncfileResult.FileCorrupted; 
			
			byte[] seek = new byte[48];
			Array.ConstrainedCopy (seek, 0, stream, 6, 48);	
			
            uint[] buf = new uint[4]; 
            
            for (int i = 0; i < 4; i++) buf[i] = BitConverter.ToUInt32(seek, 0+i*4); 
            out_dat[0] = buf[0] ^ xor_tab_datfile[0]; 
            out_dat[1] = buf[1] ^ xor_tab_datfile[1]; 
            out_dat[2] = buf[2] ^ xor_tab_datfile[2]; 
            out_dat[3] = buf[3] ^ xor_tab_datfile[3]; 
            for (int i = 0; i < 4; i++) buf[i] = BitConverter.ToUInt32(seek, 16+i*4); 
            out_dat[4] = buf[0] ^ xor_tab_datfile[0]; 
            out_dat[5] = buf[1] ^ xor_tab_datfile[1]; 
            out_dat[6] = buf[2] ^ xor_tab_datfile[2]; 
            out_dat[7] = buf[3] ^ xor_tab_datfile[3]; 
            for (int i = 0; i < 4; i++) buf[i] = BitConverter.ToUInt32(seek, 32+i*4); 
            out_dat[8] = buf[0] ^ xor_tab_datfile[0]; 
            out_dat[9] = buf[1] ^ xor_tab_datfile[1]; 
            out_dat[10] = buf[2] ^ xor_tab_datfile[2]; 
            out_dat[11] = buf[3] ^ xor_tab_datfile[3]; 
            
            return ReadEncfileResult.Success; 
        } 
        public static void LoadKeys(string dir) 
        { 
            ReadEncfileResult result; 
            result = ReadEncfile(dir+"Enc1.dat", enc1_keys); 
            if (result != ReadEncfileResult.Success) throw new Exception(result.ToString()); 
            result = ReadEncfile(dir+"Enc2.dat", enc2_keys); 
            if (result != ReadEncfileResult.Success) throw new Exception(result.ToString()); 
            result = ReadEncfile(dir+"Dec1.dat", dec1_keys); 
            if (result != ReadEncfileResult.Success) throw new Exception(result.ToString()); 
            result = ReadEncfile(dir+"Dec2.dat", dec2_keys); 
            if (result != ReadEncfileResult.Success) throw new Exception(result.ToString());
            LoadedKey = true; 
        } 
        #endregion 

        #region Extension Methods 
        public static void MuXor3Byte(this Packet packet, int ptr, uint len) 
        { 
            for (uint i = 0; i < len; ++i) 
                packet.Source[packet.Index + ptr + i] ^= xor_table_3byte[i % 3]; 
        } 
        public static void EncodeC1C2(this Packet packet) 
        { 
            MU_ForceEncodeC1C2(packet.Source, packet.ContentIndex, packet.ContentSize, packet.HeaderSize); 
        } 
        public static void DecodeC1C2(this Packet packet) 
        { 
            MU_ForceDecodeC1C2(packet.Source, packet.ContentIndex, packet.ContentSize, packet.HeaderSize); 
        } 
        public static Packet DecryptPacket(this Packet pkt) 
        { 
            byte[] buf = new byte[pkt.DecSpace]; 
            byte counter = 0; 
            if (MU_DecodeC3C4(buf, 0, pkt, pkt.ToServer ? dec1_keys : dec2_keys, ref counter)) 
            { 
                Packet dec = new Packet(buf, 0, pkt.ToServer, counter); 
                if (pkt.ToServer) 
                    dec.DecodeC1C2(); 
                return dec; 
            } 

            return null; 
        } 
        public static Packet EncryptPacket(this Packet pkt, Packet des_pkt = null) 
        { 
            if (pkt.ToServer) 
                pkt.EncodeC1C2(); 

            if (des_pkt == null) 
            { 
                byte[] buf = new byte[pkt.EncSpace]; 
                if (MU_EncodeC3C4(buf, 0, pkt, pkt.ToServer ? enc1_keys : enc2_keys, pkt.Counter)) 
                    return new Packet(buf, 0, pkt.ToServer, pkt.Counter); 
            } 
            else 
                if (MU_EncodeC3C4(des_pkt.Source, des_pkt.Index, pkt, pkt.ToServer ? enc1_keys : enc2_keys, pkt.Counter)) 
                    return des_pkt; 
             
            return null; 
        } 
        #endregion 
    } 
}  