using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HookApp.Models
{
    [Obsolete("ここで使用している拡張メソッドは使用しないようにする！")]
    static class Refactoring_Temp_Extensions
    {

        [Obsolete("逃げの拡張メソッド・使用しないようにする")]
        public static bool ContainsKey(this List<KeyData> nameDictionary, KeyboardUtilConstants.VirtualKeyCode vkCode)
        {
            return nameDictionary.Any((item) => item.KeyCode == vkCode);
        }

        [Obsolete("逃げの拡張メソッド・使用しないようにする")]
        public static string GetKeyString(this List<KeyData> nameDictionary, KeyboardUtilConstants.VirtualKeyCode vkCode)
        {
            return nameDictionary.Where( (item) => item.KeyCode == vkCode).Select((item) => item.DisplayString).FirstOrDefault();
        }
    }
}
