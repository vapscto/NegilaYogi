﻿using System;
using System.Collections.Generic;

namespace paytm.util
{
    class Constants
    {
        public const String VALUE_SEPARATOR_TOKEN = "|";
        public const Int16 SALT_LENGTH = 4;
        public static Boolean USE_UNICODE_ENCODING = false;

        //public static byte[] CRYPTO_INIT_VECTOR = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76, 0x76, 0x61, 0x6e };
        //public static byte[] CRYPTO_INIT_VECTOR = new byte[] { 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 };
        public static byte[] CRYPTO_INIT_VECTOR = new byte[] { 0x40, 0x40, 0x40, 0x40, 0x26, 0x26, 0x26, 0x26, 0x23, 0x23, 0x23, 0x23, 0x24, 0x24, 0x24, 0x24 };
        //public const String CRYPTO_INIT_VECTOR = "@1B2c3D4e5F6g7H8";

    }
}