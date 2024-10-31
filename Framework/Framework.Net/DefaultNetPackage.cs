using System.IO;

namespace Framework
{
    public class DefaultNetPackage : INetPackage
    {
        const int LENGTH_SIZE = sizeof(int);
        const int ID_SIZE = sizeof(int);
        const int SEQUENCE_SIZE = sizeof(short);
        const int HEADER_SIZE = LENGTH_SIZE + ID_SIZE + SEQUENCE_SIZE;
        public int id { get; set; }
        public short sequence { get; set; }
        public byte[] body { get; set; }
        public int bodyLength { get; set; }

        public bool Read(BinaryReader br, int available)
        {
            if (bodyLength == 0)
            {
                if (available < 4)
                    return false;

                bodyLength = Reverse(br.ReadInt32()) - LENGTH_SIZE;
                available -= LENGTH_SIZE;
            }

            if (bodyLength <= available)
            {
                id = Reverse(br.ReadInt32());
                sequence = Reverse(br.ReadInt16());
                bodyLength -= ID_SIZE + SEQUENCE_SIZE;
                if (bodyLength > 0)
                {
                    body = BufferPool.Get(bodyLength);
                    br.Read(body, 0, bodyLength);
                }
                else
                {
                    body = System.Array.Empty<byte>();
                }
                return true;
            }
            return false;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Reverse(bodyLength + HEADER_SIZE));
            bw.Write(Reverse(id));
            bw.Write(Reverse(sequence));
            if (bodyLength > 0)
            {
                bw.Write(body, 0, bodyLength);
            }
        }

        public void Reset()
        {
            id = 0;
            sequence = 0;
            body = null;
            bodyLength = 0;
        }


        #region  Helper
        
        public static short Reverse(short value) => (short) (((int) value & (int) byte.MaxValue) << 8 | (int) value >> 8);

        public static int Reverse(int value) => ((value & (int) byte.MaxValue) << 24) + ((value >> 8 & (int) byte.MaxValue) << 16) + ((value >> 16 & (int) byte.MaxValue) << 8) + (value >> 24 & (int) byte.MaxValue);
        
        

        #endregion
    }
}