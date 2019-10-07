using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KnuthCycle
{
    public class CycleExpression
    {
        public static char Permute(string cycleExpr, char c)
        {
            int idx = cycleExpr.IndexOf(c);
            if (idx == -1)
                return c;
            if (cycleExpr[idx + 1] != ')')
            {
                return cycleExpr[idx + 1];
            }
            else
            {
                while (cycleExpr[idx] != '(')
                {
                    idx--;
                }
                return cycleExpr[idx + 1];
            }
        }

        public static string Permute(string cycleExpr, string input)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
            {
                sb.Append(Permute(cycleExpr,c));
            }
            return sb.ToString();
        }

        public static bool IsValidInput(string input)
        {
            if (input == null)
                return false;

            var inputChars = new HashSet<char>(input);

            return inputChars.Count == input.Length;
        }

        public static bool IsValidExpression(string expr)
        {
            if (!String.IsNullOrEmpty(expr))
            {
                if (Regex.IsMatch(expr, "^(\\([a-z]*\\))+"))
                {
                    return true;
                }
            }
            return false;
        }

        protected static int IndexOfFirstUntagged(char[] marked)
        {
            for (int i=0; i < marked.Length; i++)
            {
                if (marked[i] == '(')
                    continue;
                if (!char.IsUpper(marked[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        protected static bool AreEqual(char a, char b)
        {
            return char.ToLower(a) == char.ToLower(b);
        }

        protected static int IndexOfMatch(char[] marked, int start, char match)
        {
            for (int i = start+1; i < marked.Length; i++)
            {
                if (AreEqual(marked[i],match))
                {
                    return i;
                }
            }
            return 0;
        }

        protected static char Tag(char c)
        {
            return char.ToUpper(c);
        }

        public static string Multiply(string expr)
        {
            if (!IsValidExpression(expr))
                throw new Exception("Invalid cycle notation");

            char[] arrExpr = InitExpression(expr);
            var sb = new StringBuilder();

            char start = '\0';
            char current = '\0';
            int next = 0;
            bool cycleFound = false;

            while ((next = IndexOfFirstUntagged(arrExpr)) > 0)
            {
                cycleFound = false;
                sb.Append('(');
                sb.Append(arrExpr[next]);
                start = arrExpr[next];
                arrExpr[next] = Tag(start);
                while (!cycleFound)
                {
                    current = arrExpr[next+1];
                    while (true)
                    {
                        if ((next = IndexOfMatch(arrExpr, next+1, current)) > 0)
                        {
                            arrExpr[next] = Tag(current);
                            break;
                        }
                        
                        if (!AreEqual(current,start))
                        {
                            sb.Append(current);
                            next = 0;
                            continue;
                        }
                        cycleFound = true;
                        sb.Append(')');
                        break;
                    }
                }
                // Pop singleton cycles from output
                if (sb[sb.Length - 3] == '(')
                    sb.Remove(sb.Length - 3,3);
            }
            return sb.ToString().ToLower();
        }

        protected static char[] InitExpression(string expr)
        {
            char[] marked = expr.ToCharArray();
            char letter = '\0';
            for (int i = 0; i < marked.Length; i++)
            {
                if (marked[i] == '(')
                    letter = Tag(marked[i + 1]);
                if (marked[i] == ')')
                    marked[i] = letter;
            }
            return marked;
        }

    }
}
