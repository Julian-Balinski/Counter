namespace Counter
{
    public class CounterSaveData
    {
        public int count = 0;
        public int defaultValue = 0;
        public string name = string.Empty;
        public string color = string.Empty;
        public Thickness pos = new();
        public CounterSaveData() { }
        public CounterSaveData(Counter counter)
        {
            count = counter.Count;
            defaultValue = counter.defaultCount;
            name = counter.Name;
            color = counter.Color.ToArgbHex();
            pos = counter.Pos;
        }

        public static explicit operator CounterSaveData(Counter v)
        {
            return new CounterSaveData(v);
        }
    }
}
