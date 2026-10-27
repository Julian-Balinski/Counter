using System.Diagnostics;

namespace Counter;

public partial class Counter
{
    public int Count
    {
        get => Int32.Parse(CounterDisplay.Text);
        set => CounterDisplay.Text = value.ToString();
    }
    public string Name
    {
        get => CounterName.Text;
        set => CounterName.Text = value;
    }
    public Color Color
    {
        get => CounterTab.BackgroundColor;
        set => CounterTab.BackgroundColor = value;
    }
    public Thickness Pos
    {
        get => Margin;
        set => Margin = value;
    }
    public int defaultCount = 0;
    public bool selected = false;
    public bool deleting = false;
    public Point? mousePosOffset = new Point(0, 0);

    public static explicit operator Counter(CounterSaveData v)
    {
        return new Counter(v);
    }
    public Counter(CounterSaveData saveData)
    {
        InitializeComponent();
        defaultCount = saveData.defaultValue;
        Name = saveData.name;
        Count = saveData.count;
        Color = Color.FromArgb(saveData.color);
        if (Color.GetLuminosity() > 0.5)
            CounterName.TextColor = new Color(0.1333f);
        Pos = saveData.pos;
    }
    public Counter(string name, int defaultCount, Color color, Thickness pos)
    {
        InitializeComponent();
        this.defaultCount = defaultCount;
        Name = name;
        Count = defaultCount;
        Color = color;
        if (color.GetLuminosity() > 0.5)
            CounterName.TextColor = new Color(0.1333f);
        Pos = pos;
    }
    //protected override void OnHandlerChanged()
    //{
    //    base.OnHandlerChanged();
    //    ColorPicker.SelectedIndex = 0;
    //}

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        if (!deleting)
        {
            CounterDisplay.Text = (++Count).ToString();
            App.MainPage.SaveCountersToFile();
        }
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        if (!deleting)
        {
            CounterDisplay.Text = (--Count).ToString();
            App.MainPage.SaveCountersToFile();
        }
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        if (!deleting)
        {
            Count = defaultCount;
            App.MainPage.SaveCountersToFile();
        }
    }

    //private void OnColorPicked(object sender, EventArgs e)
    //{
    //    int margin = 190;
    //    string darkTextColor = "#222222";
    //    string lightTextColor = "#DDDDDD";
    //    string strBGColor = "#222222";
    //    string strTextColor = lightTextColor;
    //    /*
    //    datk_gray/0 = #222222
    //    dark_blue/1 = #0000AA
    //    dark_green/2 = #00AA00
    //    dark_cyan/3 = #00AAAA
    //    dark_red/4 = #AA0000
    //    dark_purple/5 = #AA00AA
    //    gold/6 = #EEAA00
    //    light_gray/7 = #AAAAAA
    //    gray/8 = #555555
    //    blue/9 = #5555EE
    //    green/10 = #55EE55
    //    cyan/11 = #55EEEE
    //    red/12 = #EE5555
    //    magenta/13 = #EE55EE
    //    yellow/14 = #EEEE55
    //    white/15 = #EEEEEE
    //     */
    //    switch (ColorPicker.SelectedIndex)
    //    {
    //        case 0:
    //            margin = 141;
    //            strBGColor = "#222222";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 1:
    //            margin = 140;
    //            strBGColor = "#0000AA";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 2:
    //            margin = 131;
    //            strBGColor = "#00AA00";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 3:
    //            margin = 137;
    //            strBGColor = "#00AAAA";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 4:
    //            margin = 147;
    //            strBGColor = "#AA0000";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 5:
    //            margin = 126;
    //            strBGColor = "#AA00AA";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 6:
    //            margin = 173;
    //            strBGColor = "#EEAA00";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 7:
    //            margin = 140;
    //            strBGColor = "#AAAAAA";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 8:
    //            margin = 173;
    //            strBGColor = "#555555";
    //            strTextColor = lightTextColor;
    //            break;
    //        case 9:
    //            margin = 174;
    //            strBGColor = "#5555EE";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 10:
    //            margin = 163;
    //            strBGColor = "#55EE55";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 11:
    //            margin = 171;
    //            strBGColor = "#55EEEE";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 12:
    //            margin = 178;
    //            strBGColor = "#EE5555";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 13:
    //            margin = 146;
    //            strBGColor = "#EE55EE";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 14:
    //            margin = 161;
    //            strBGColor = "#EEEE55";
    //            strTextColor = darkTextColor;
    //            break;
    //        case 15:
    //            margin = 166;
    //            strBGColor = "#EEEEEE";
    //            strTextColor = darkTextColor;
    //            break;
    //    }
    //    ColorPicker.Margin = new Thickness(margin, 0, 0, 0);
    //    Color BGColor = Color.FromArgb(strBGColor);
    //    Color TextColor = Color.FromArgb(strTextColor);
    //    ColorPicker.BackgroundColor = BGColor;
    //    ColorPicker.TextColor = TextColor;
    //    CounterTab.BackgroundColor = BGColor;
    //    CounterName.TextColor = TextColor;
    //}

    private void OnPointerDown(object sender, PointerEventArgs e)
    {
        if (!deleting)
        {
            mousePosOffset = new Point(App.mousePos.Value.X - Margin.Left, App.mousePos.Value.Y - Margin.Top);
            selected = true;
        }
    }
    public void UpdatePosition()
    {
        if (!deleting)
        {
            double maxX = Math.Max(Window.Width - Width - 16, 0);
            double maxY = Math.Max(Window.Height - Height - 88, 0);
            double x = 0;
            double y = 0;
            if (selected)
            {
                x = Math.Clamp(App.mousePos.Value.X - mousePosOffset.Value.X, 0, maxX);
                y = Math.Clamp(App.mousePos.Value.Y - mousePosOffset.Value.Y, 0, maxY);
            }
            else
            {
                x = Math.Clamp(Pos.Left, 0, maxX);
                y = Math.Clamp(Pos.Top, 0, maxY);
            }
            Pos = new Thickness(x, y);
        }
    }

    private async void RemoveCounter(object sender, PointerEventArgs e)
    {
        if (!deleting)
        {
            deleting = true;
            for (var i = 0; i < 30; i++)
            {
                Scale *= 1.025 - 0.015 * (2 * i - 5) - 0.0125;
                await Task.Delay(16);
            }
            App.MainPage.CounterDisplay.Remove(this);
            App.MainPage.counters.Remove(this);
            App.MainPage.SaveCountersToFile();
        }
    }
}