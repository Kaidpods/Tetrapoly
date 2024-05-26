using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public class Event
    {
        public int Id;
        protected string Name;
        protected string Description;

        public Event(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDesc()
        {
            return Description;
        }
    }

    
}
