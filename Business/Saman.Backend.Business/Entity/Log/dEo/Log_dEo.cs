using Saman.Backend.Business.Entity.Log.Rsx;

namespace Saman.Backend.Business.Entity.Log
{
    public class LogOperation_dEo
    {
        public enum LogOperation : byte
        {
            Read = 0,
            Create = 1,
            Update = 2,
            Delete = 3,
            List = 4,
        }

        public static List<KeyValuePair<LogOperation, string>> List() => Enum.GetValues(typeof(LogOperation)).Cast<LogOperation>().Select(x => new KeyValuePair<LogOperation, string>(x, Title(x) ?? nameof(x))).ToList();

        public static string? Title(LogOperation value) { try { return Log_Rsx.ResourceManager.GetString("LogOperation_dEo_" + Enum.GetName(typeof(LogOperation), value)); } catch { return string.Empty; } }

        public static byte Value(LogOperation value) { try { return (byte)value; } catch { return 0; } }
    }
}
