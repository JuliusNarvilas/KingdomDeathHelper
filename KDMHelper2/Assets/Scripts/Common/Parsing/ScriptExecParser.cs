/*
using Common.Parsing.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Parsing
{

    public interface IScriptErrorContext
    {
        void ThrowError(string errorMsg);
    }

    public class ScriptExec : IScriptErrorContext
    {
        public Dictionary<string, IExpressionValueProvider> Variables = new Dictionary<string, IExpressionValueProvider>();

        private List<IExpressionValueProvider> m_Executions;

        public bool Run()
        {
            try
            {
                int count = m_Executions.Count;
                for (int i = 0; i < count; ++i)
                {
                    IExpressionValueProvider exec = m_Executions[i];
                    exec.GetValue(this);
                }
                return true;
            }
            catch(Exception e)
            {
                Log.ProductionLogError(e.Message);
            }
            return false;
        }

        public void ThrowError(string errorMsg)
        {
            throw new Exception(errorMsg);
        }
    }

    public class ScriptExecData
    {
        public IExpressionValueProvider lhsValue;
        public Func<IExpressionValueProvider[], IExpressionValueProvider> build;
    }

    public class ScriptExecParser : IScriptErrorContext
    {
        public enum ETokenType
        {
            None,
            Identifier,
            Dot,
            BracketOpen,
            BracketClose,
            NumberFloat,
            NumberInt,
            Operation,
            Semicolon,
            String
        }

        static readonly List<char> m_whitespaceChars = new List<char> { ' ', '\t', '\n', '\r', '\f', '\v' };

    struct ScriptTokenRange
        {
            public ScriptTokenRange(int start, int length)
            {
                StartPos = start;
                Length = length;
            }

            public int StartPos;
            public int Length;
        }

        private ScriptTokenRange NextToken(string str, int start, out ETokenType token)
        {
            int count = str.Length;
            for (int i = start; i < count; i++)
            {
                char character = str[i];
                if(!m_whitespaceChars.Contains(character))
                {
                    //if number
                    if('0' <= character && '9' >= character)
                    {
                        bool dotUsed = false;
                        int length = 1;
                        character = str[i + length];
                        while (true)
                        {
                            while (('0' <= character && '9' >= character))
                            {
                                length++;
                            }
                            if ('.' == character && !dotUsed)
                            {
                                dotUsed = true;
                                length++;
                                continue;
                            }

                            break;
                        }
                        if(dotUsed)
                            token = ETokenType.NumberFloat;
                        else
                            token = ETokenType.NumberInt;

                        return new ScriptTokenRange(i, length);
                    }

                    //one character specials
                    switch (character)
                    {
                        case '(':
                            token = ETokenType.BracketOpen;
                            return new ScriptTokenRange(i, 1);
                        case ')':
                            token = ETokenType.BracketClose;
                            return new ScriptTokenRange(i, 1);
                        case '.':
                            token = ETokenType.Dot;
                            return new ScriptTokenRange(i, 1);
                        case '+':
                            token = ETokenType.Operation;
                            return new ScriptTokenRange(i, 1);
                        case '-':
                            token = ETokenType.Operation;
                            return new ScriptTokenRange(i, 1);
                        case '*':
                            token = ETokenType.Operation;
                            return new ScriptTokenRange(i, 1);
                        case '/':
                            token = ETokenType.Operation;
                            return new ScriptTokenRange(i, 1);
                        case '=':
                            token = ETokenType.Operation;
                            return new ScriptTokenRange(i, 1);
                        case ';':
                            token = ETokenType.Semicolon;
                            return new ScriptTokenRange(i, 1);
                    }

                    //string
                    if(character == '"')
                    {
                        token = ETokenType.String;
                        int strMarkCount = 1;
                        int subStrLength = 0;
                        i++;
                        while (strMarkCount > 0 && (i + subStrLength) < count)
                        {
                            character = str[i + subStrLength];
                            if(character == '\\')
                            {
                                subStrLength++;
                                if (str[i + subStrLength] == '"')
                                    strMarkCount++;
                                continue;
                            }
                            if(character == '"')
                            {
                                strMarkCount--;
                                if (strMarkCount == 0)
                                    return new ScriptTokenRange(i, subStrLength);
                            }
                            subStrLength++;
                        }
                        return new ScriptTokenRange(i, 0);
                    }

                    //identifier
                    {
                        int length = 1;
                        character = str[i + length];
                        while (
                            ('a' <= character && 'z' >= character) ||
                            ('A' <= character && 'Z' >= character) ||
                            ('0' <= character && '9' >= character) ||
                            (character == '_')
                            )
                        {
                            length++;
                            character = str[i + length];
                        }
                        token = ETokenType.Identifier;
                        return new ScriptTokenRange(i, length);
                    }
                }
            }

            token = ETokenType.None;
            return new ScriptTokenRange(count, 0);
        }

        void Parse(string execStr)
        {
            ETokenType tokenType;
            int readIndex = 0;

            while(readIndex < execStr.Length)
            {
                ScriptTokenRange tokenRange = NextToken(execStr, readIndex, out tokenType);

                switch(tokenType)
                {
                    case ETokenType.Identifier:

                    case ETokenType.Dot:

                    case ETokenType.BracketOpen:

                    case ETokenType.BracketClose:

                    case ETokenType.NumberFloat:
                        {
                            string floatStr = execStr.Substring(tokenRange.StartPos, tokenRange.Length);
                            float val = float.Parse(floatStr);
                        }
                        break;
                    case ETokenType.NumberInt:
                        {
                            string intStr = execStr.Substring(tokenRange.StartPos, tokenRange.Length);
                            int val = int.Parse(intStr);
                        }
                        break;

                    case ETokenType.Operation:

                        break;
                }

                readIndex = tokenRange.StartPos + tokenRange.Length;
            }
        }

        private IExpressionValueProvider ExecuteNextExpression(string str, int start)
        {
            ScriptExecData expressionData = new ScriptExecData();

            ETokenType tokenType;
            ScriptTokenRange tokenRange = NextToken(str, start, out tokenType);

            switch(tokenType)
            {
                case ETokenType.Identifier:
                    //find existing variable
                    //find existing function
                    break;
                case ETokenType.NumberFloat:
                    expressionData.lhsValue = new SimpleConstantExpressionValue<float>(float.Parse(str.Substring(tokenRange.StartPos, tokenRange.Length)));
                    break;
                case ETokenType.NumberInt:
                    expressionData.lhsValue = new SimpleConstantExpressionValue<int>(int.Parse(str.Substring(tokenRange.StartPos, tokenRange.Length)));
                    break;
                case ETokenType.String:
                    expressionData.lhsValue = new SimpleConstantExpressionValue<string>(str.Substring(tokenRange.StartPos + 1, tokenRange.Length - 2));
                    break;
                default:
                    return null;
            }
        }

        private bool MakeVariable(string str, int start)
        {
            ETokenType tokenType;
            ScriptTokenRange tokenRange = NextToken(str, start, out tokenType);

            string name = string.Empty;
            if(tokenType == ETokenType.Identifier)
            {
                name = str.Substring(tokenRange.StartPos, tokenRange.Length);
            }
            else
            {
                return false;
            }

            tokenRange = NextToken(str, start, out tokenType);
            switch(tokenType)
            {
                case ETokenType.Operation:
                    if(str[tokenRange.StartPos] == '=' && tokenRange.Length == 1)
                    {

                    }
                    else
                        return false;

                    break;
            }
            if ()
            {

            }

            return true;
        }

        private Dictionary<string, System.Func<double>> m_Consts = new Dictionary<string, System.Func<double>>();
        private Dictionary<string, System.Func<double[], double>> m_Funcs = new Dictionary<string, System.Func<double[], double>>();
        private Expression m_Context;
        private Stack<Expression> m_ContextLevels;

        public List<string> m_Errors;

        public void ThrowError(string errorMsg)
        {
            m_Errors.Add(errorMsg);
        }

       
    }
}
*/