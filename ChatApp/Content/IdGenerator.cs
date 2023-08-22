using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChatApp.Content
{
    public class IdGenerator
    {
        public static string generate()
        {
            int length = 50;
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            string s = "";
            Random random = new Random();
            char letter;

            for(int i = 0; i<length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                s += letter;
            }
            return s;
        }
    }
}