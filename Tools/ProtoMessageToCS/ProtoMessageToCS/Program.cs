using System;

namespace ProtoMessageToCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string messageFile = null;
            string messageDir = null;
            string protoDir = null;
            string dstMessageDir = null;
            string dstProtoDir = null;
            
            foreach (var arg in args)
            {
                string[] items = arg.Split('=');
                if (items.Length == 2)
                {
                    switch (items[0])
                    {
                        case "--message_file":
                            messageFile = items[1];
                            break;
                        case "--message_dir":
                            messageDir = items[1];
                            break;
                        case "--proto_dir":
                            protoDir = items[1];
                            break;
                        case "--dst_message_dir":
                            dstMessageDir = items[1];
                            break;
                        case "--dst_proto_dir":
                            dstProtoDir = items[1];
                            break;
                    }
                }
            }

            if (protoDir == null)
            {
                Console.WriteLine("--proto_dir没有设置");
            }
            else if (dstMessageDir == null)
            {
                Console.WriteLine("--dst_message_dir没有设置");
            }
            else if (dstProtoDir == null)
            {
                Console.WriteLine("--dst_proto_dir没有设置");
            }
            else if (messageFile != null)
            {
                ProtoMessageToCS.BuildMessage(messageFile, protoDir, dstMessageDir, dstProtoDir);
            }
            else if (messageDir != null)
            {
                ProtoMessageToCS.BuildMessages(messageDir, protoDir, dstMessageDir, dstProtoDir);
            }
            else
            {
                Console.WriteLine("--message_file或--message_dir没有设置");
            }

            Console.ReadKey();
        }
    }
}
