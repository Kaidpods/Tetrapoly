using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public enum CardType
    {
        COMMUNITY,
        CHANCE
    }
    public class ChanceCommunity
    {
        public string Description { get; }
        public CardType Type { get; }

        public ChanceCommunity(string description, CardType type)
        {
            Description = description;
            Type = type;
        }

        public string GetDesc()
        {
            return Description;
        }
    }

}
