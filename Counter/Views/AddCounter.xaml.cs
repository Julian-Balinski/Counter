using System.Numerics;
using System.Text.RegularExpressions;

namespace Counter.Views;

public partial class AddCounter : ContentPage
{
    public bool pickingColor = false;
    public Point? ColorPickerPos = new Point(200, 0);
    public Color counterColor = new(255, 0, 0);
    public int counterDefaultCount = 0;
    public string counterName = "Counter";
    public Random RNG = new();
    public bool preventLooping = false;
    public AddCounter()
    {
        InitializeComponent();
    }
    private void FullyLoaded(object sender, EventArgs e)
    {
        RandomizeColor();
    }

    public void UpdateColor()
    {
        float hue = (float)ColorSlider.Value;
        Color tempHue = Color.FromHsv(hue, 1, 1);
        ColorPickerHue.Fill = tempHue;
        ColorSlider.MaximumTrackColor = tempHue;
        counterColor = Color.FromHsv(hue, (float)(ColorPickerPos.Value.X / ColorPicker.Width), (float)(1 - ColorPickerPos.Value.Y / ColorPicker.Height));
        ColorDisplay.BackgroundColor = counterColor;
    }

    public void RandomizeColor()
    {
        ColorSlider.Value = RNG.NextDouble();
        Vector2 temp = new((float)RNG.NextDouble(), (float)RNG.NextDouble());
        temp = Vector2.Normalize(temp);
        temp = Vector2.Multiply(temp, new Vector2(82.5f * (float)RNG.NextDouble()));
        SatValue.Text = (100 - temp.X).ToString("N2");
        ValValue.Text = temp.Y.ToString("N2");
        ColorPickerPos = new Point((100 - temp.X) * 2, temp.Y * 2);
        UpdateColor();
    }

    private void OnHueChange(object sender, ValueChangedEventArgs e)
    {
        if (!preventLooping)
        {
            preventLooping = true;
            HueValue.Text = (ColorSlider.Value * 360).ToString("N2");
            UpdateColor();
            preventLooping = false;
        }
    }

    private void OnPointerMove(object sender, PointerEventArgs e)
    {
        if (pickingColor && !preventLooping)
        {
            preventLooping = true;
            ColorPickerPos = e.GetPosition(ColorPicker);
            ValValue.Text = Math.Clamp((200 - ColorPickerPos.Value.Y) / 2 + RNG.NextDouble() - 0.5, 0, 100).ToString("N2");
            SatValue.Text = Math.Clamp(ColorPickerPos.Value.X / 2 + RNG.NextDouble() - 0.5, 0, 100).ToString("N2");
            UpdateColor();
            preventLooping = false;
        }
    }

    private void OnPointerDown(object sender, PointerEventArgs e)
    {
        pickingColor = true;
    }

    private void OnPointerUp(object sender, PointerEventArgs e)
    {
        pickingColor = false;
    }

    private static string FilterIntText(string input)
    {
        return new Regex(@"(?=[-\d])-?(?!-)\d*").Match(input).Value;
    }

    private static string FilterDoubleText(string input)
    {
        return new Regex(@"(?=[-\d.,])-?(?!-)\d*[,.]?\d*").Match(input).Value;
    }

    private void UpdateName(object sender, TextChangedEventArgs e)
    {
        counterName = CounterName.Text;
    }

    private void UpdateDefaultValue(object sender, TextChangedEventArgs e)
    {
        CounterDefaultValue.Text = FilterIntText(CounterDefaultValue.Text);
        if (Int32.TryParse(CounterDefaultValue.Text, out int temp))
        {
            counterDefaultCount = temp;
        }
    }

    private void FinilizeAddCounter(object sender, EventArgs e)
    {
        Counter temp = new(counterName, counterDefaultCount, counterColor, new(0, 0));
        App.MainPage.counters.Add(temp);
        App.MainPage.CounterDisplay.Add(temp);
        App.MainPage.SaveCountersToFile();
        Navigation.PopAsync(true);
    }

    private void ResetCounterForm(object sender, EventArgs e)
    {
        RandomizeColor();
        CounterName.Text = "";
        CounterDefaultValue.Text = "";
    }

    private void ValueChanged(object sender, TextChangedEventArgs e)
    {
        if (!preventLooping)
        {
            preventLooping = true;
            ValValue.Text = FilterDoubleText(ValValue.Text);
            if (Double.TryParse(ValValue.Text, out double temp))
                ColorPickerPos = new Point(ColorPickerPos.Value.X, Math.Clamp((100 - temp) * 2, 0, 200));
            else
                ColorPickerPos = new Point(ColorPickerPos.Value.X, 0);
            UpdateColor();
            preventLooping = false;
        }
    }

    private void SaturationChanged(object sender, TextChangedEventArgs e)
    {
        if (!preventLooping)
        {
            preventLooping = true;
            SatValue.Text = FilterDoubleText(SatValue.Text);
            if (Double.TryParse(SatValue.Text, out double temp))
                ColorPickerPos = new Point(Math.Clamp(temp * 2, 0, 200), ColorPickerPos.Value.Y);
            else
                ColorPickerPos = new Point(0, ColorPickerPos.Value.Y);
            UpdateColor();
            preventLooping = false;
        }
    }

    private void HueChanged(object sender, TextChangedEventArgs e)
    {
        if (!preventLooping)
        {
            preventLooping = true;
            HueValue.Text = AddCounter.FilterDoubleText(HueValue.Text);
            if (Double.TryParse(HueValue.Text, out double temp))
                ColorSlider.Value = Math.Clamp(temp / 360, 0, 1);
            else
                ColorSlider.Value = 0;
            UpdateColor();
            preventLooping = false;
        }

    }
}