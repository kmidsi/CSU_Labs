using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcOOP
{
    public class Token
    {

    }

    public class Parenthesis : Token
    {
        public bool opening;
    }

    public class Number : Token
    {
        public double value;
    }

    public class Operation : Token
    {
        public char symbol;
    }
}
