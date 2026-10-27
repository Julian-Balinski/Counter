using System.Xml.Serialization;

namespace Counter
{
    public partial class MainPage : ContentPage
    {
        public List<Counter> counters = new List<Counter>();
        public AbsoluteLayout CounterDisplay;
        public MainPage()
        {
            InitializeComponent();
            App.MainPage = this;
            CounterDisplay = Display;
            LoadCountersFromFile();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            foreach(var counter in counters)
            {
                counter.UpdatePosition();
            }
            SaveCountersToFile();
        }

        private async void AddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCounter());
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            App.mousePos = e.GetPosition(null);
            if (counters.Count > 0)
                foreach (var counter in counters)
                {
                    counter.UpdatePosition();
                }
        }

        private void OnPointerUp(object sender, PointerEventArgs e)
        {
            if (counters.Count > 0)
            {
                SaveCountersToFile();
                foreach (var counter in counters)
                    counter.selected = false;
            }
        }

        private void ClearCounters()
        {
            foreach(var counter in counters)
            {
                Display.Remove(counter);
            }
            counters.Clear();
        }

        public void SaveCountersToFile()
        {
            List<CounterSaveData> saveData = new();
            foreach (var counter in counters)
            {
                saveData.Add((CounterSaveData)counter);
            }
            using (FileStream fs = new FileStream("..\\..\\..\\..\\counters.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(saveData.GetType());
                serializer.Serialize(fs, saveData);
            }
        }

        public void LoadCountersFromFile()
        {
            ClearCounters();
            List<CounterSaveData> getData = new();
            using (FileStream fs = new FileStream("..\\..\\..\\..\\counters.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(getData.GetType());
                getData = serializer.Deserialize(fs) as List<CounterSaveData>;
            }
            foreach (Counter counter in getData)
            {
                counters.Add(counter);
                Display.Add(counter);
            }
        }
    }
}
