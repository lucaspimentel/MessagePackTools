using System.Buffers;
using System.Text;
using MessagePack;
using MessagePackTools;

if (args.Length == 0 || args[0] == "--help")
{
    Logger.WriteLine("Usage:");
    Logger.WriteLine("  MessagePackTool [options] <filename>");
    Logger.WriteLine();
    Logger.WriteLine("Options:");
    Logger.WriteLine("  --validate      Validate the MessagePack data in the file.");
    Logger.WriteLine("  --save-to-json  Save the MessagePack data in the file to a JSON file.");
    Logger.WriteLine("  --json-output   Output the MessagePack data in the file as JSON.");
    Logger.WriteLine();
    Logger.WriteLine("Arguments:");
    Logger.WriteLine("  filename        The file containing the MessagePack data.");
    return;
}

var command = Command.None;
var filename = string.Empty;

foreach (var arg in args)
{
    switch (arg)
    {
        case "--validate":
            command = Command.Validate;
            break;
        case "--save-to-json":
            command = Command.SaveToJson;
            break;
        case "--json-output":
            command = Command.JsonOutput;
            break;
        default:
        {
            filename = arg;

            if (!File.Exists(filename))
            {
                Logger.WriteLine($"File '{filename}' not found.");
                return;
            }

            break;
        }
    }
}

if (command == Command.None)
{
    Logger.WriteLine("Must specify one of --validate, --save-to-json, or --json-output.");
    return;
}

if (string.IsNullOrWhiteSpace(filename))
{
    Logger.WriteLine("Must specify filename.");
    return;
}

var bytes = File.ReadAllBytes(filename);

if (command is Command.SaveToJson or Command.JsonOutput)
{
    var json = MessagePackSerializer.ConvertToJson(bytes);

    if (command is Command.JsonOutput)
    {
        Console.WriteLine(json);
        return;
    }

    var jsonFilename = Path.ChangeExtension(filename, ".json");
    File.WriteAllText(jsonFilename, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    Logger.WriteLine($"Saved as JSON to '{jsonFilename}'.");
}

if (command is Command.Validate)
{
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
}

static void DecodeNext(ref MessagePackReader reader, int depth, string prefix)
{
    MessagePackType nextType = reader.NextMessagePackType;
    var indent = new string(' ', depth * 2);
    Logger.Write($"{indent}{prefix}{nextType}");

    switch (nextType)
    {
        case MessagePackType.Integer:
            Logger.WriteLine($":{reader.ReadUInt64()}");
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

enum Command
{
    None,
    Validate,
    SaveToJson,
    JsonOutput
}
