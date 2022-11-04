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

Logger.WriteLine($"Read {bytes.Length} from file '{filename}'.");

ReadAll(bytes);

Logger.WriteLine();
Logger.WriteLine("Done.");

static void ReadAll(byte[] bytes)
{
    var reader = new MessagePackReader(bytes);
    int depth = 0;

    while (!reader.End)
    {
        ReadNext(ref reader, depth, prefix: string.Empty);
    }
}

static void ReadNext(ref MessagePackReader reader, int depth, string prefix)
{
    MessagePackType nextType = reader.NextMessagePackType;
    Logger.Write(depth, $"{prefix}{nextType}");

    switch (nextType)
    {
        case MessagePackType.Integer:
            Logger.WriteLine($": {reader.ReadInt64()}");
            break;
        case MessagePackType.Nil:
            reader.ReadNil();
            Logger.WriteLine();
            break;
        case MessagePackType.Boolean:
            Logger.WriteLine($": {reader.ReadBoolean()}");
            break;
        case MessagePackType.Float:
            Logger.WriteLine($": {reader.ReadDouble()}");
            break;
        case MessagePackType.String:
            Logger.WriteLine($": {reader.ReadString()}");
            break;
        case MessagePackType.Array:
            int arrayLength = reader.ReadArrayHeader();
            depth++;

            Logger.WriteLine($": {arrayLength}");

            for (int x = 0; x < arrayLength; x++)
            {
                ReadNext(ref reader, depth, $"{x}:");
            }

            break;
        case MessagePackType.Map:
            int mapLength = reader.ReadMapHeader();
            depth++;

            Logger.WriteLine($": {mapLength}");

            for (int x = 0; x < mapLength; x++)
            {
                ReadNext(ref reader, depth, $"key {x}:");
                ReadNext(ref reader, depth, $"value {x}:");
            }

            break;
        case MessagePackType.Unknown:
        case MessagePackType.Binary:
        case MessagePackType.Extension:
        default:
            break;
    }
}
