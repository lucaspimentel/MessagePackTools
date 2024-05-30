using System.Buffers;
using MessagePack;
using MessagePackTools;

if (args.Length != 1)
{
    Logger.WriteLine("Must specify filename.");
    return;
}

var filename = args[0];

if (!File.Exists(filename))
{
    Logger.WriteLine($"File '{filename}' not found.");
    return;
}

var bytes = File.ReadAllBytes(filename);

Logger.WriteLine($"Loaded {bytes.Length:N0} bytes from '{filename}'.");
Logger.WriteLine();

var reader = new MessagePackReader(bytes);

while (!reader.End)
{

    try
    {
        DecodeNext(ref reader, depth: 0, prefix: string.Empty);
    }
    catch (EndOfStreamException)
    {
        Logger.WriteLine();
        Logger.WriteLine("Reached end of bytes, but expected more data.");

        return;
    }
}

Logger.WriteLine();
Logger.WriteLine("Done.");

static void DecodeNext(ref MessagePackReader reader, int depth, string prefix)
{
    MessagePackType nextType = reader.NextMessagePackType;
    var indent = new string(' ', depth * 2);
    Logger.Write($"{indent}{prefix}{nextType}");

    switch (nextType)
    {
        case MessagePackType.Integer:
            Logger.WriteLine($":{reader.ReadInt64()}");
            break;

        case MessagePackType.Nil:
            reader.ReadNil();
            Logger.WriteLine();
            break;

        case MessagePackType.Boolean:
            Logger.WriteLine($":{reader.ReadBoolean()}");
            break;

        case MessagePackType.Float:
            Logger.WriteLine($":{reader.ReadDouble()}");
            break;

        case MessagePackType.String:
            Logger.WriteLine($":{reader.ReadString()}");
            break;

        case MessagePackType.Array:
            int arrayLength = reader.ReadArrayHeader();
            depth++;

            Logger.WriteLine($":{arrayLength}");

            for (int x = 0; x < arrayLength; x++)
            {
                DecodeNext(ref reader, depth, $"[{x}]:");
            }

            break;

        case MessagePackType.Map:
            int mapLength = reader.ReadMapHeader();
            depth++;

            Logger.WriteLine($":{mapLength}");

            for (int x = 0; x < mapLength; x++)
            {
                var index = x.ToString();
                DecodeNext(ref reader, depth, $"[{index}] key:");

                var mapValueIndent = new string(' ', index.Length + 3);
                DecodeNext(ref reader, depth, $"{mapValueIndent}value:");
            }

            break;

        case MessagePackType.Binary:
            var bytes = reader.ReadBytes()?.ToArray();

            if (bytes == null)
            {
                Logger.WriteLine("(null)");
            }
            else
            {
                Logger.Write($":{bytes.Length}");
                Logger.WriteLine($":{HexConverter.ToString(bytes)}");
            }

            break;

        case MessagePackType.Unknown:
        case MessagePackType.Extension:
        default:
            Logger.WriteLine();
            reader.Skip();
            break;
    }
}
