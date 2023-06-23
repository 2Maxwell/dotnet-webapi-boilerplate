namespace FSH.WebApi.Domain.Shop;

public class Pax
{
    public int Adult { get; set; } = 1;
    public List<Child>? Children { get; set; } = new List<Child>();
    public int Beds
    {
        get
        {
            int counter = 0;

            if (Children!.Count > 0)
            {
                foreach (Child item in Children)
                {
                    _ = item.ExtraBed ? (counter++) : (counter = counter);
                }
            }

            return counter + Adult;
        }
    }
}

public class Child
{
    public int Age { get; set; }
    public bool ExtraBed { get; set; }
}

