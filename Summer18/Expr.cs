using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summer18
{
    public abstract class Expr
    {
        public abstract T AcceptVisitor<T>(IVisitor<T> visitor);
    }

    public interface IVisitor<T>
    {
       
        T VisitLiteral(Literal l);
        T VisitBinary(Binary b);
        T VisitUnary(Unary u);
        T VisitGrouping(Grouping g);
    }

    public class Literal:Expr
    {
        public Token token;
        public Literal(Token token) { this.token = token; }

        public override T AcceptVisitor<T>(IVisitor<T> visitor) { return visitor.VisitLiteral(this); }
    }

    public class Binary:Expr
    {
        public Expr left_operand, right_operand;
        public Token opr;

        public Binary(Expr left_operand, Token opr, Expr right_operand)
        {
            this.left_operand = left_operand;
            this.opr = opr;
            this.right_operand = right_operand;
        }

        public override T AcceptVisitor<T>(IVisitor<T> visitor) { return visitor.VisitBinary(this); }
    }

    public class Unary:Expr
    {
        public Token opr;
        public Expr expression;

        public Unary (Token opr, Expr expression)
        {
            this.opr = opr;
            this.expression = expression;
        }

        public override T AcceptVisitor<T>(IVisitor<T> visitor) { return visitor.VisitUnary(this); }
    }

    public class Grouping:Expr
    {
        public Expr expression;

        public Grouping(Expr expression) { this.expression = expression; }

        public override T AcceptVisitor<T>(IVisitor<T> visitor) { return visitor.VisitGrouping(this); }
    }
}
