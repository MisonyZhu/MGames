using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProtoMessageToCS
{
    public static class ProtoMessageToCS
    {
        const string MESSAGE_FILE_FORMAT = @"// Genreated by Tools/ProtoMessageToCS
namespace Game.Net.{0}
{{{1}}}";
        const string MESSAGE_FORMAT = @"
    struct {0}ID : IMessageID
    {{
        public int id => {1};
    }}

    // {2}
    // {3}
    class {0}Message : Message<{0}ID{4}>
    {{
    }}
";

        const string TAB1 = @"    ";
        const string LINE_TAB = @"
    ";

        const string GENERATE_PROTO_CMD = @"{0}..\protoc --proto_path={1}\ --csharp_out={2}{3}";

        public static void BuildMessages(string messageDir, string protoDir, string dstMessageDir, string dstProtoDir)
        {
            try
            {
                SafeCreateDirectory(dstMessageDir);
                HashSet<string> protos = new HashSet<string>();
                var files = Directory.GetFiles(messageDir, "*.xml", SearchOption.TopDirectoryOnly);
                int count = files.Length;
                for (int i = 0; i < count; ++i)
                {
                    Build(files[i], protoDir, dstMessageDir, dstProtoDir, protos);
                }
                BuildProtos(protos, protoDir, dstProtoDir);
                Console.WriteLine("Build Complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void BuildMessage(string messageFile, string protoDir, string dstMessageDir, string dstProtoDir)
        {
            try
            {
                SafeCreateDirectory(dstMessageDir);
                HashSet<string> protos = new HashSet<string>();
                Build(messageFile, protoDir, dstMessageDir, dstProtoDir, protos);
                BuildProtos(protos, protoDir, dstProtoDir);
                Console.WriteLine("Build Complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        class BuildData
        {
            public StringBuilder builder = new StringBuilder();
        }

        const string MESSAGE = "Message";
        static void Build(string messageFile, string protoDir, string dstMessageDir, string dstProtoDir, HashSet<string> protos)
        {
            Console.WriteLine("Build Message:" + Path.GetFileName(messageFile));
            string fileName = Path.GetFileNameWithoutExtension(messageFile);
            var buildDatas = new Dictionary<string, BuildData>();
            var doc = new XmlDocument();
            doc.Load(messageFile);
            foreach (XmlElement root in doc.SelectNodes("messages"))
            {
                int rootID = int.Parse(root.GetAttribute("id"));

                foreach (XmlElement e in root.SelectNodes("message"))
                {
                    string name = e.GetAttribute("class");
                    if (name.EndsWith(MESSAGE))
                        name = name.Substring(0, name.Length - MESSAGE.Length);

                    int id = int.Parse(e.GetAttribute("id"));
                    string type = e.GetAttribute("type");
                    string desc = e.GetAttribute("desc");

                    string protoName = null;
                    string protoMessage = null;
                    var proto = e.SelectSingleNode("proto") as XmlElement;
                    if (proto != null)
                    {
                        protoName = proto.HasAttribute("name") ? proto.GetAttribute("name") : proto.GetAttribute("class");
                        protoMessage = GetProtoName(ref protoName);
                    }

                    if (!buildDatas.TryGetValue(protoName, out BuildData buildData))
                    {
                        buildData = new BuildData();
                        buildDatas.Add(protoName, buildData);
                    }

                    int messageID = rootID * 1000 + id;
                    buildData.builder.AppendFormat(MESSAGE_FORMAT, name, messageID, desc, type,
                        protoMessage != null ? ", " + protoMessage : "");
                }
            }

            foreach (var item in buildDatas)
            {
                string protoName = item.Key;
                var data = item.Value;

                string text = string.Format(MESSAGE_FILE_FORMAT, protoName, data.builder.ToString());
                File.WriteAllText(PathEx.Combine(dstMessageDir, protoName + "Message.cs"), text);

                CollectProtos(protoDir, FindProtoFile(protoName, protoDir), protos);
            }

        }

        const string IMPORT_PATTERN = "import \"(.*)\";";

        public static void CollectProtos(string protoDir, string protoFile, HashSet<string> protos)
        {
            if (string.IsNullOrEmpty(protoFile))
                return;

            if (protos.Contains(protoFile))
                return;

            protos.Add(protoFile);
            string text = File.ReadAllText(PathEx.Combine(protoDir, protoFile));
            foreach (Match match in Regex.Matches(text, IMPORT_PATTERN))
            {
                CollectProtos(protoDir, match.Groups[1].Value, protos);
            }
        }

        static string FindProtoFile(string protoName, string protoDir)
        {
            var files = Directory.GetFiles(protoDir, "*.proto", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (string.Equals(name.Replace("_", ""), protoName, StringComparison.OrdinalIgnoreCase))
                {
                    return Path.GetFileName(file);
                }
            }
            return null;
        }

        const string PROTO = "Proto";
        const string PROTOS = "Protos";
        static string GetProtoName(ref string protoName)
        {
            int index1 = protoName.LastIndexOf('.');
            string protoMessage = protoName.Substring(index1 + 1);
            int index2 = protoName.LastIndexOf('.', index1 - 1);
            protoName = protoName.Substring(index2 + 1, index1 - index2 - 1);
            if (protoName.EndsWith(PROTO))
                protoName = protoName.Substring(0, protoName.Length - PROTO.Length);
            else if (protoName.EndsWith(PROTOS))
                protoName = protoName.Substring(0, protoName.Length - PROTOS.Length);
            return protoMessage;
        }

        static void BuildProtos(HashSet<string> protos, string protoDir, string dstProtoDir)
        {
            SafeCreateDirectory(dstProtoDir);

            StringBuilder builder = new StringBuilder();
            foreach (var proto in protos)
            {
                builder.Append(" ").Append(proto);
            }

            Console.WriteLine("Build Protos ...");
            string cmd = string.Format(GENERATE_PROTO_CMD, AppDomain.CurrentDomain.BaseDirectory, protoDir, dstProtoDir, builder.ToString());
            ProcessTools.RunCmd(cmd);
        }

        static void SafeCreateDirectory(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }
    }
}
