using System.Text;
using System;

namespace Framework
{
    public struct SubString : IEquatable<SubString>
    {
        public object obj;

        public int start;

        public int length;

        public SubString(string str)
        {
            obj = str;
            start = 0;
            length = str.Length;
        }

        public SubString(string str, int start)
        {
            obj = str;
            this.start = start;
            length = str.Length;
        }

        public SubString(string str, int start, int length)
        {
            obj = str;
            this.start = start;
            this.length = length;
        }

        public SubString(StringBuilder sb, int start, int length)
        {
            obj = sb;
            this.start = start;
            this.length = length;
        }

        public override int GetHashCode()
        {
            int num = 0;
            if (obj is string text)
            {
                for (int i = 0; i < length; i++)
                {
                    num = num * 37 + text[start + i];
                }
            }
            else
            {
                StringBuilder stringBuilder = (StringBuilder)obj;
                for (int j = 0; j < length; j++)
                {
                    num = num * 37 + stringBuilder[start + j];
                }
            }

            return num;
        }

        public bool Equals(SubString other)
        {
            if (length != other.length)
            {
                return false;
            }

            int num = start;
            int num2 = other.start;
            if (obj is string text)
            {
                if (other.obj is string text2)
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (text[num + i] != text2[num2 + i])
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    StringBuilder stringBuilder = (StringBuilder)other.obj;
                    for (int j = 0; j < length; j++)
                    {
                        if (text[num + j] != stringBuilder[num2 + j])
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                StringBuilder stringBuilder2 = (StringBuilder)obj;
                if (other.obj is string text3)
                {
                    for (int k = 0; k < length; k++)
                    {
                        if (stringBuilder2[num + k] != text3[num2 + k])
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    StringBuilder stringBuilder3 = (StringBuilder)other.obj;
                    for (int l = 0; l < length; l++)
                    {
                        if (stringBuilder2[num + l] != stringBuilder3[num2 + l])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is SubString other)
            {
                return Equals(other);
            }

            return false;
        }

        public override string ToString()
        {
            return (!(obj is string text)) ? ((StringBuilder)obj).ToString(start, length) : ((start == 0 && text.Length == length) ? text : text.Substring(start, length));
        }

        public static bool operator ==(SubString a, SubString b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SubString a, SubString b)
        {
            return !(a == b);
        }
    }
}