using System;

namespace Grains
{
    public static class CharGuidConverter
    {
        public static Guid GetGuid(char c) => new Guid(c, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        public static char GetChar(Guid g) => BitConverter.ToChar(g.ToByteArray(), 0);
    }
}