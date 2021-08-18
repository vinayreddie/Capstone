using System;

namespace Capstone.Models
{
    internal class MaxValueAttribute : Attribute
    {
        private int v1;
        private string v2;

        public MaxValueAttribute(int v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}