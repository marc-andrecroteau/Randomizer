using System;

namespace Model
{
    public class Foo
    {
        public int Number { get; set; }
        public double Ratio { get; set; }
        public Bar Bar { get; set; }

        public Foo()
        {
            Bar = new Bar();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((Foo)obj);
        }

        protected bool Equals(Foo other)
        {
            return Number == other.Number && Ratio.Equals(other.Ratio) && Equals(Bar, other.Bar);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Number;
                hashCode = (hashCode * 397) ^ Ratio.GetHashCode();
                hashCode = (hashCode * 397) ^ (Bar != null ? Bar.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}