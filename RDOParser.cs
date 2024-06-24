using System;
using System.Text;
using System.Collections.Generic;
public class RDOParser
{
    private string text;
    private int pos;
    private char? currentChar;
    public RDOParser(string _text)
    {
        text = _text;
        pos = 0;
        currentChar = text.Length > 0 ? (char?)text[this.pos] : null;
    }
    private void Error (string msg)
    {
        throw new Exception($"Error parsing input at position {pos} : {msg}");
    }
    private void Advance()
    {
        if (currentChar == null)
        {
            Error($"cursor out of range {pos}");
        }
        pos++;
        currentChar = pos < text.Length ?(char?)text[pos] : null;

    }
    private void SkipWhiteSpace()
    {
        while(currentChar != null && char.IsWhiteSpace((char)currentChar))
        {
            Advance();
        }

    }
    private void Match(char expected)
    {
        if (currentChar == expected)
        {
            Advance();
        }
        else 
        {
            Error($"Expected'{expected}'");
        }
    }
    private string ParserString()
    {
        Match('"');
        StringBuilder result = new System.Text.StringBuilder();
        while (currentChar != null && currentChar != '"')
        {
            if (char.IsLetterOrDigit((char)currentChar) || char.IsWhiteSpace((char)currentChar))
            {
                result.Append(currentChar);
                Advance();
            }
            else 
            {
                Error($"Invalid character in a string at position {pos}");
            }
        }
        Match('"');
        return result.ToString();
    }
    private string ParseValue()
    {
        SkipWhiteSpace();
        
        if (currentChar == '"')
        {
            return ParserString();
        }
        if (!char.IsLetterOrDigit((char)currentChar))
        {
            Error($"Invalid character in a value at position {pos}");
        }
        StringBuilder result = new System.Text.StringBuilder();
        while (currentChar != null && char.IsLetterOrDigit((char)currentChar))
        {
            result.Append(currentChar);
            Advance();
        }
        return result.ToString();

    }
    private List<object> ParseArray()
    {
        Match('[');
        List<object> result = new List<object>();
        SkipWhiteSpace();
        if (currentChar == ']')
        {
            Error($"Array contains no value at position{pos}");
        }
        while (true)
        {
            string value = ParseValue();
            result.Add(value);
            SkipWhiteSpace();
            if (currentChar == ']') 
            {
                break;
            }
        }
        Match(']');
        return result;

    }
    private List<object> ParsePair()
    {
        string key = ParseValue();
        SkipWhiteSpace();
        Match(':');
        SkipWhiteSpace();
        List<object> result =  ParseArray();
        result.Add(key);
        return result;
    }
    private rdo ParseObject()
    {
        rdo result = new rdo();
        SkipWhiteSpace();
        Match('{');
        SkipWhiteSpace();
        if (currentChar == '}')
        {
            Error($"RDO object has no pair {pos}");

        }
        while (true)
        {
            List<object> pair = ParsePair();
            string key = (string) pair[pair.Count - 1];
            pair.RemoveAt(pair.Count - 1);
            result.AddPair(key, pair);
            SkipWhiteSpace();
            if (currentChar == '}')
            {
                break;
            }
        }
        Match('}');
        return result;
    }
    private List<rdo> ParseListObject()
    {
        List<rdo> result = new List<rdo>();
        SkipWhiteSpace();
        if (currentChar == null)
        {
            Error("File has no object");
        }
        rdo rdoObject = ParseObject();
        result.Add(rdoObject);
        SkipWhiteSpace();
        while (currentChar != null)
        {
            SkipWhiteSpace();
            rdoObject = ParseObject();
            result.Add(rdoObject);
        }
        return result;

    }
    public List<rdo> Parse()
    {
        SkipWhiteSpace();
        List<rdo> result = ParseListObject();
        SkipWhiteSpace();
        if (currentChar != null)
        {
            this.Error("Unexpected character at the end of input");
        }
        return result;
    }

}
