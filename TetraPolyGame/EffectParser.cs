using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using alias = System.Linq.Dynamic;
using System.Reflection;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public class EffectParser
    {
        public static Action<Player> ParseEffect(string effectDescription)
        {
            // Compile the code snippet into a delegate
            LambdaExpression lambda = alias.DynamicExpression.ParseLambda(typeof(Player), typeof(void), effectDescription);
            return (Action<Player>)lambda.Compile();
        }
    }
}
