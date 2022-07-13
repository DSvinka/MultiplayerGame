namespace Code.Utils
{
    public static class DLogger
    {
        private static string GenerateSourceString(string classSource, string methodSource)
        {
            return $"<b><color=grey>[<color=teal>{classSource}</color>:<color=green>{methodSource}</color>]</color></b>";
        }
    
        public static void Info(string classSource, string methodSource, string text)
        {
            var source = GenerateSourceString(classSource, methodSource);
            UnityEngine.Debug.Log($"{source} <color=white>{text}</color>");
        }
    
        public static void Debug(string classSource, string methodSource, string text)
        {
            var source = GenerateSourceString(classSource, methodSource);
            UnityEngine.Debug.Log($"{source} <color=aqua>{text}</color>");
        }

        public static void Warning(string classSource, string methodSource, string text)
        {
            var source = GenerateSourceString(classSource, methodSource);
            UnityEngine.Debug.LogWarning($"{source} <color=yellow>{text}</color>");
        }
    
        public static void Error(string classSource, string methodSource, string text)
        {
            var source = GenerateSourceString(classSource, methodSource);
            UnityEngine.Debug.LogError($"{source} <color=red><b>{text}</b></color>");
        }
    }
}