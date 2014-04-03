namespace Model
{
    public class Bar
    {
        public int Number { get; set; }
        public double Ratio { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Bar)obj);
        }

        protected bool Equals(Bar other)
        {
            return Number == other.Number && Ratio.Equals(other.Ratio);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Number * 397) ^ Ratio.GetHashCode();
            }
        }
    }
}