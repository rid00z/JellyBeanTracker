using System;

namespace JellyBeanTracker.Web
{
    public class Globals
    {
        #if NATIVE
        public static bool Native = true;
        #else
        public static bool Native = false;
        #endif
    }
}

