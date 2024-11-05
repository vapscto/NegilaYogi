using System;
using System.Collections.Generic;
using System.Text;

namespace paytm.util
{
    class MessageConsole
    {
        private static Boolean MESSAGING_ON = false ;

        public static void WriteLine()
        {
            if (!MESSAGING_ON) 
                return;

            Console.WriteLine();
        }

        public static void WriteLine(String value)
        {
            if (!MESSAGING_ON)
                return;

            Console.WriteLine(value);
        }

        public static void Write(String value)
        {
            if (!MESSAGING_ON)
                return;

            Console.Write(value);
        }

        public static void WriteLine(String value, Object arg0, Object arg1)
        {
            if (!MESSAGING_ON)
                return;

            Console.WriteLine(value,arg0, arg1);
        }
    }
}
