using System.IO;

namespace Framework
{
    public interface INetPackage
    {
        int id { get; set; }
        short sequence { get; set; }
        byte[] body { get; set; }
        int bodyLength { get; set; }

        bool Read(BinaryReader br, int available);
        void Write(BinaryWriter bw);
        void Reset();
    }

    public class NetPackage 
    {
       
    }
}