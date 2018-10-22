using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summer18
{
    public enum TokenType { NUM = 0, OPR_ADD = 1, EOF = 2, WORD = 3,
        PLUS = 4,
        MINUS = 5,
        DIVIDE = 6,
        STAR = 7,
        IF = 8,
        THEN = 9,
        ELSE = 10,
        TRUE = 11,
        FALSE = 12,
        ERROR = 13,
        EQUAL = 14,
        EQUAL_EQUAL = 15,
        GREATER_EQUAL = 16,
        GREATER = 17,
        LESSER_EQUAL = 18,
        LESSER = 19,
        BANG_EQUAL = 20,
        BANG = 21,
        LEFT_PAREN = 22,
        RIGHT_PAREN = 23
    }

    public class Token
    {
        public TokenType type;

        public Token(TokenType type)
        { this.type = type; }
        public override string ToString() => string.Format("({0})", type.ToString());
    }

    public class Word:Token
    {
        string value;

        public Word (string value):base(TokenType.WORD)
        { this.value = value; }
        public override string ToString() => ( value );
    }

    public class Number : Token
    {
        int value;
        public Number(int value) : base(TokenType.NUM) { this.value = value; }
        public override string ToString() => ( value.ToString() );
    }

    public class LError: Token
    {
        int index, line;
        string lexeme;
        public LError(int index, int line, string lexeme) : base (TokenType.ERROR)
        { this.index = index; this.line = line; this.lexeme = lexeme; }
        public override string ToString() => (string.Format(" ({0} at line {1}, index {2} )", type.ToString(), line.ToString(), index.ToString()));
    }

}
