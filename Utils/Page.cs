namespace timviec.Utils
{
    public class Page
    {
        public int? Previous { get; set; }
        public int Current { get; set; }
        public int? Next { get; set; }

        public Page (int current, int max)
        {
            this.Previous = current - 1;
            this.Current = current;
            this.Next = current + 1;

            if (this.Previous <= 0) this.Previous = null;
            if (this.Next > max) this.Next = null;
        }
    }
}
