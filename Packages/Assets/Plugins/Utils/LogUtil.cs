using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Ivy.Utils
{
    public static class Log
    {
        //private const string Tag = "";

        [Conditional("GameTest")]
        public static void Print(object message)
        {
            Debug.Log(message);
        }

        [Conditional("GameTest")]
        public static void Warning(object message)
        {
            Debug.LogWarning(message);
        }

        [Conditional("GameTest")]
        public static void Error(object message)
        {
            Debug.LogError(message);
        }
    }
}

