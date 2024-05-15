using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Action<Player> effect; // Delegate to hold the card's effect

        public ChanceCommunity(string description, CardType type, Action<Player> effect)
        {
            Description = description;
            Type = type;
            this.effect = effect;
        }

        public void Execute(Player player)
        {
            // Execute the card's effect
            effect(player);
        }
    }

}
