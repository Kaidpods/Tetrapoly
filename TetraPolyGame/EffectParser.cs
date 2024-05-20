using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using TetraPolyGame;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public class EffectParser
    {
        public static Action<Player> ParseEffect(string effectDescription)
        {
            var config = new ParsingConfig { };
            ParameterExpression playerParam = Expression.Parameter(typeof(Player), "p");
            var lambda = DynamicExpressionParser.ParseLambda(
                config,
                new[] { playerParam },
                null,
                effectDescription
            );

            return (Action<Player>)lambda.Compile();
        }
    }
}
