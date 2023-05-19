using System.Collections.Generic;
using System;

namespace SWI_Simulation.DataType
{

    public class Tern : IEquatable<Tern>
    {
        public TernType Type { get; private set; }
        public string Value {get; private set; }
        public List<Tern>? Arguments {get; private set;}

        public Tern(TernType type, string value, List<string> arg)
        {
            Type = type;
            Value = value;
            Arguments = new List<Tern>();
            foreach(var x in arg)
            {
                Arguments.Add(x);
            }
        }
        public Tern(TernType type, string value, List<Tern>? arg = null)
        {
            Type = type;
            Value = value;
            Arguments = arg;
            if (type == TernType.CompoundTerm && Arguments == null)
            {
                Arguments = new List<Tern>();
            }
        }

        public bool isMatch (Tern b)
        {
            if (Type != TernType.CompoundTerm || b.Type != TernType.CompoundTerm)
                return false;
            if (Arguments is null || b.Arguments is null)
                return false;
            if (Value != b.Value)
                return false;
            if (Arguments.Count != b.Arguments.Count)
                return false;
            for (int i = 0; i < Arguments.Count; ++i)
            {
                if (Arguments)
            }
        }
        public HashSet<string> GetVarArgList ()
        {
            HashSet<string> result = new HashSet<string>();
            if (Arguments is null)
                return result;
            foreach(var item in Arguments)
            {
                if (item.Type == TernType.Variable)
                {
                    result.Add(item.Value);
                }
            }
            return result;
        }

        public static implicit operator Tern(int val)
        {
            return new Tern(TernType.Atom, val.ToString());
        }
        public static implicit operator Tern(double val)
        {
            return new Tern(TernType.Atom, val.ToString());
        }
        public static implicit operator Tern(string val)
        {
            double d_result = 0;
            if (double.TryParse(val, out d_result))
            {
                return new Tern(TernType.Number, val);
            }
            if (char.IsUpper(val[0]) || val[0] == '_')
            {
                return new Tern(TernType.Variable, val);
            }
            return new Tern(TernType.Atom, val);
        }

        public override string ToString()
        {
            if (Type != TernType.CompoundTerm)
            {
                return Value;
            }
            if (Arguments?.Count == 0 || Arguments == null)
            {
                return Value;
            }
            string result = Value + "(";
            result += string.Join(", ", Arguments);
            result += ").";
            return result;
        }

        public static bool operator != (Tern a, Tern b)
        {
            return !(a == b);
        }

        public static bool operator == (Tern a, Tern b)
        {
            return a.Equals(b);
        }

        public override bool Equals(Object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (Type != ((Tern)obj).Type)
                return false;
            if (Type == TernType.Variable)
                return true;
            if (Type == TernType.Atom && Value == ((Tern)obj).Value)
                return true;
            if (Value != ((Tern)obj).Value)
                return false;

            if (Arguments == null || ((Tern)obj).Arguments == null)
                return false;
            else
            {
                var args = ((Tern)obj).Arguments ?? new List<Tern>();
                if (Arguments.Count != args.Count)
                    return false;
                for (int i = 0; i < Arguments?.Count; ++i)
                {
                    if (Arguments[i] != args[i])
                        return false;
                }
                return true;
            }
        }

        bool IEquatable<Tern>.Equals(Tern? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return Equals((Object)other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() + Type.GetHashCode();
        }
    }
}