namespace Counter
{
    public partial class App : Application
    {
        public static Point? mousePos = new Point(0, 0);
        public static Views.MainPage MainPage;
        public App()
        {
            InitializeComponent();

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
            return new Window(new AppShell())
            {
                Width = displayInfo.Width / 2,
                Height = displayInfo.Height / 2,
                X = (displayInfo.Width / displayInfo.Density - displayInfo.Width / 2) / 2,
                Y = (displayInfo.Height / displayInfo.Density - displayInfo.Height / 2) / 2
            };
        }
    }
}