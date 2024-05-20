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
        private string effectDescription;
        private Action<Player> effect; // Delegate to hold the card's effect

        public ChanceCommunity(string description, CardType type, string effectDesc)
        {
            Description = description;
            Type = type;
            effectDescription = effectDesc;
            this.effect = ParseEffect(effectDescription);
        }
        public static Action<Player> ParseEffect(string effectDescription)
        {
            var config = new ParsingConfig { };
            ParameterExpression playerParam = Expression.Parameter(typeof(Player), "p");
            var lambda = DynamicExpressionParser.ParseLambda(
                config,
                [playerParam],
                null,
                effectDescription
            );

            return (Action<Player>)lambda.Compile();
        }
        
        public void SetEffect()
        {
            effect = ParseEffect(effectDescription);
        }

        public void Execute(Player p)
        {
            // Execute the card's effect
            effect(p);
        }
    }

}
