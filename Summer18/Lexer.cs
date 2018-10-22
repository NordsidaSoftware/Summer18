using System;
using System.Collections.Generic;
using System.Text;

namespace Summer18
{
    // ===================================================================
    //                   LEXER FOR SUMMER '18
    //         Består av en klasse : LEXER 
    //         Hjepeklasse : TOKEN

    internal class Lexer
    {
        // ========FIELDS========
        private string source_file;
        private int index;
        private int line;

        private Char CurrentChar;

        private static Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>() {
            {"if", TokenType.IF },
            {"else", TokenType.ELSE },
            {"true", TokenType.TRUE },
            {"false", TokenType.FALSE }
        };

        // =======CONSTRUCTOR=========
        public Lexer(string source_file)
        {
            this.source_file = source_file + " ";
            index = 0;
            line = 1;
            
        }


        public bool IsAtEnd { get { return index >= source_file.Length; } }

        public void Advance()
        {
                if (!IsAtEnd) { index++; }
        }

        private void ReadChar()
        {
            if (!IsAtEnd)
            {
                Advance();
                CurrentChar = source_file[index - 1];
            }
        }

        private bool Match(Char c)
        {
            if (c == peek()) { Advance(); return true; }
            return false;
        }
        private char peek() { return source_file[index]; }

        internal Token GetNextToken()
        {
            ReadChar();
            if (CurrentChar== ' ') { WhiteSpace(); }
            if (CurrentChar == '\n') { line++;}
            switch (CurrentChar)
            {
               
                case ('+'): { return new Token(TokenType.PLUS); }
                case ('-'): { return new Token(TokenType.MINUS); }
                case ('/'): { return new Token(TokenType.DIVIDE); }
                case ('*'): { return new Token(TokenType.STAR); }
                case ('('): { return new Token(TokenType.LEFT_PAREN); }
                case (')'): { return new Token(TokenType.RIGHT_PAREN); }

                case ('='): { return new Token(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); }
                case ('>'): { return new Token(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); }
                case ('<'): { return new Token(Match('=') ? TokenType.LESSER_EQUAL : TokenType.LESSER); }
            }
            if (Char.IsDigit(CurrentChar)) { return (readNumber()); }
            if (Char.IsLetterOrDigit(CurrentChar)) { return ( readWord()); }
          
            if (IsAtEnd) { return new Token(TokenType.EOF); }

            return new LError(index, line, CurrentChar.ToString());
           
   
        }

        void WhiteSpace() {  while (CurrentChar == ' ' && !IsAtEnd) { ReadChar(); } }

        Token readNumber()
        {
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(CurrentChar);
                ReadChar();
            } while (isNumber(CurrentChar));

            int number = CharToInt(sb.ToString());
            if (CurrentChar != ' ') { index--; }       // small backtrack if next char is not whitespace
            return new Number(number);
        }

        Token readWord()
        {
            StringBuilder sb = new StringBuilder();

            do
            {
                sb.Append(CurrentChar);
                ReadChar();
           
            } while (char.IsLetterOrDigit(CurrentChar));


            if (CurrentChar != ' ') { index--; }       // small backtrack if next char is not whitespace

            if (keywords.ContainsKey(sb.ToString().ToLower())) { return new Token(keywords[sb.ToString()]); }
           
            else return new Word(sb.ToString());
        }

        int CharToInt(string c)
        {
            int result;
            int.TryParse(c, out result);
            return result; 
        }

        bool isNumber(char c)
        {
            if (c >= '0' && c <= '9') return true;
            else return false;
        }

    }
}

    // ===================================END LEXER===========================


/* En ny lexer for summer'18. Startet den 28.04.2018 kl 20:30 - en fin lørdag i slutten av april.
 * Revet inngangsparti foran huset! 
 * 
 *Skrevet videre på 22.05 fra kl. 18. Ved gamle ferjeleiet. Ikke en så fantastisk fin dag.
 * 
 * 
 */


