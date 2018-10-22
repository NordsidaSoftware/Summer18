using System;
using System.Collections.Generic;
using System.Text;

namespace Summer18
{
    // ===================================================================
    //                   PARSER FOR SUMMER '18

    /*
     *               GRAMMAR FOR SUMMER'18
     *     From Bob Nystroms book Crafting Interpreters
     *               
     * expression     → equality ;
     * equality       → comparison ( ( "!=" | "==" ) comparison )* ;
     * comparison     → addition ( ( ">" | ">=" | "<" | "<=" ) addition )* ;
     * addition       → multiplication ( ( "-" | "+" ) multiplication )* ;
     * multiplication → unary ( ( "/" | "*" ) unary )* ;
     * unary          → ( "!" | "-" ) unary
     *                | primary ;
     * primary        → NUMBER | STRING | "false" | "true" | "nil"
     *                | "(" expression ")" ;
     *
     * */

    #region PPrinter
    internal class TestPrint : IVisitor<String>
    {

        public string Print(Expr expr)
        {
            return expr.AcceptVisitor<string>(this);
        }

        public string VisitBinary(Binary b)
        {
           return Parenthesize(b.opr.ToString(), b.left_operand, b.right_operand);
        }

      

        public string VisitGrouping(Grouping g)
        {
            return Parenthesize("Grouping: ", g.expression);
        }

        public string VisitLiteral(Literal l)
        {
            return l.token.ToString();
        }

        public string VisitUnary(Unary u)
        {
            return Parenthesize(u.opr.ToString(), u.expression);
        }

        private string Parenthesize(string name, params Expr[] expressions)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(name);
            foreach (Expr expr in expressions)
            {
                sb.Append(" ");
                sb.Append(expr.AcceptVisitor<string>(this));
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
    #endregion

    internal class ParserError : Exception
    {
        public Token ErrorToken;
        public int index;
        public ParserError(string message, Token ErrorToken, int index) : base(message)
        {
            this.ErrorToken = ErrorToken;
            this.index = index;
        }
    }


    internal class Parser
    {
        private Lexer lex;

        Token CurrentToken;
        Token PreviousToken;
        List<Token> tokens = new List<Token>();

        public Parser(Lexer lex)
        {
            this.lex = lex;
        }

        internal void Parse()
        {

            //    do
            //    {
            //    CurrentToken = lex.GetNextToken();
            //    Console.Write(CurrentToken.ToString());              // entering all tokens into a list.
            //    tokens.Add(CurrentToken);

            //  } while (CurrentToken.type != TokenType.EOF);
            // Console.WriteLine("\n");

            // foreach (Token t in tokens) { Console.Write(t.ToString() + " "); }
            // Console.WriteLine();

            TestPrint tp = new TestPrint();
            CurrentToken = lex.GetNextToken();

            try
            {
                Expr expr = expression();
                Console.WriteLine(tp.Print(expr));
            }
            catch (ParserError e)
            {
                Console.WriteLine("Error in parser at :" + e.Message.ToString());
                Console.WriteLine(CurrentToken.type.ToString());
            }
                
            
          
        }

        private Expr expression() { return equality(); }

        private Expr equality()
        {
            Expr expr = comparison();
            while (match( TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL))
            {
                Token opr = PreviousToken;
                Expr right = comparison();
                expr = new Binary(expr, opr, right);
            }

            return expr;

        }

        private Expr comparison()
        {
            Expr expr = addition();
            while (match(TokenType.GREATER_EQUAL, TokenType.LESSER_EQUAL, TokenType.GREATER, TokenType.LESSER))
            {
                Token opr = PreviousToken;
                Expr right = addition();
                expr = new Binary(expr, opr, right);
            }
            return expr;
        }

        private Expr addition()
        {
            Expr expr = multiplikation();
            while (match(TokenType.PLUS, TokenType.MINUS))
            {
                Token opr = PreviousToken;
                Expr right = addition();
                expr = new Binary(expr, opr, right);
            }
            return expr;
        }

        private Expr multiplikation()
        {
            Expr expr = unary();
            while (match(TokenType.STAR, TokenType.DIVIDE))
            {
                Token opr = PreviousToken;
                Expr right = unary();
                expr = new Binary(expr, opr, right);
            }
            return expr;
        }

        private Expr unary()
        {
            if (match(TokenType.BANG, TokenType.MINUS))
            {
                Token opr = PreviousToken;
                Expr right = unary();
                return new Unary(opr, right);
            }

            return primary();
        }

        private Expr primary()
        {
            if (match(TokenType.TRUE)) { return new Literal(new Token(TokenType.TRUE)); }
            if (match(TokenType.FALSE)) { return new Literal(new Token(TokenType.FALSE)); }
            if (match(TokenType.WORD)) { return new Literal(new Word(PreviousToken.ToString())); }
            if (match(TokenType.NUM)) { return new Literal(new Word(PreviousToken.ToString())); }

            if (match(TokenType.LEFT_PAREN))
            {
                Expr expr = expression();

                consume(TokenType.RIGHT_PAREN);
                return new Grouping(expr);
            }

            throw new ParserError("Error : ", CurrentToken, 0);
        }

        private bool match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (check(type))
                {
                    advance();
                    return true;
                }
            }
            return false;
        }

        private bool check(TokenType type)
        {
            if (type == CurrentToken.type) { return true; }
            else { return false; }
        }

        private void consume(TokenType type)
        {
            if (CurrentToken.type == type) { advance();  return; }
            else { throw new ParserError("Error in consume : lookin for " + type.ToString(), CurrentToken, 0); }
        }

        private void advance()
        {
            PreviousToken = CurrentToken;
            CurrentToken = lex.GetNextToken();
        }

       












        // Dette er en test på om jeg får til å implementere 'visitor pattern'. Ser ut til å fungere ganske greit.
        /*
        void RunTrainingProgram()
        {
            Dog d = new Dog("Fido");
            Cat c = new Cat("Felix");

            FeedMe feeder = new FeedMe();

            Console.WriteLine(feeder.feed(d));
            Console.WriteLine(feeder.feed(c));
            
        }


        abstract class Animal
        {
            public string name;
            public abstract T acceptVisitor<T>(AnimalVisitor<T> visitor);
        }

        interface acceptVisitor<T> {  T acceptVisitor(AnimalVisitor<T> visitor); }

        class Dog : Animal
        {

            public Dog(string name) : base() { this.name = name; }
            public override T acceptVisitor<T>(AnimalVisitor<T> visitor)
            {
                return visitor.visitDog(this);
            }
        }

        class Cat : Animal
        {
            public Cat (string name) : base() { this.name = name; }
            public override T acceptVisitor<T>(AnimalVisitor<T> visitor)
            {
                return visitor.visitCat(this);
            }
        }

        interface AnimalVisitor<T>
        {
            T visitDog(Dog dog);
            T visitCat(Cat cat);
        }

        interface AnimalVisitor {  void visitDog(Dog dog); void visitCat(Cat cat); }

        class FeedMe : AnimalVisitor<string>
        {

            public string feed (Animal a)
            {
                return a.acceptVisitor(this);
            }

            public string visitDog(Dog dog)
            {
                return " # " + dog.name + " : Chomp chomp chomp";
            }

            string AnimalVisitor<string>.visitCat(Cat cat)
            {
                return " # " + cat.name + " : yum yum yum";
            }
        }

        class ReturningVoid : AnimalVisitor


        {
            public void returnVoid(Animal a) { a.acceptVisitor(this); }

            public void visitCat(Cat cat)
            {
                Console.WriteLine("Here!" + cat.name);
            }

            public void visitDog(Dog dog)
            {
                Console.WriteLine("Here!" + dog.name);
            }
        }
        */


    }
// ========================END PARSER===========================================
    /*
     * Skrevet videre her den 04.juli 2018 -Togtur til Sunmøre.
     * Neste dag : Blir 42 i dag. Startet sommerferie på Sunmøre. 
     * 
     * */
}