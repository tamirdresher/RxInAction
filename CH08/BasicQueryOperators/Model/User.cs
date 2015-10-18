using System;

namespace BasicQueryOperators.Model
{
    internal class User
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return String.Format("Id={0} Name:{1}", Id, Name);
        }
    }
}