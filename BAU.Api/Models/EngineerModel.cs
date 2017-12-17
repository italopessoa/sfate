namespace BAU.Api.Models
{
    public class EngineerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (this.Name != (obj as EngineerModel).Name
            || (this.Id != (obj as EngineerModel).Id))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                if (!string.IsNullOrEmpty(Name))
                {
                    hash = hash * 23 + Name.GetHashCode();
                }
                return hash;
            }
        }
    }
}
