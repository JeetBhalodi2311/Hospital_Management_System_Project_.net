using System;

namespace Hospital_Management_System.Helpers
{
    public static class IdEncoder
    {
        public static string Encode(int id)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(id.ToString());
            return Convert.ToBase64String(bytes);
        }

        public static int Decode(string encodedId)
        {
            byte[] bytes = Convert.FromBase64String(encodedId);
            string decoded = System.Text.Encoding.UTF8.GetString(bytes);
            return int.Parse(decoded);
        }
    }
}
