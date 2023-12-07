using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
namespace BeeSmart.ViewModels
{
    public class HomeModel : INotifyPropertyChanged
    {
        private static readonly List<Color> Colors = new List<Color>()
        {
            Color.Accent,
            Color.Aqua,
            Color.Black,
            Color.Blue,
            Color.Fuchsia,
            Color.Fuchsia,
            Color.Gray,
            Color.Green,
            Color.Lime,
            Color.Maroon,
            Color.Navy,
            Color.Pink,
            Color.Purple,
            Color.Red,
            Color.Silver,
            Color.Teal,
            Color.White,
            Color.Wheat,
            Color.WhiteSmoke
        };

        private static readonly List<FontAttributes> FontAttrs = new List<FontAttributes>()
        {
            FontAttributes.Bold,
            FontAttributes.Italic,
            FontAttributes.Bold | FontAttributes.Italic,
            FontAttributes.None
        };

        public Color BadgeColor { get; private set; } = Color.Red;
        public Color BadgeTextColor { get; private set; } = Color.Black;

        public ICommand ChangeColorCommand => new Command(obj =>
        {
            _color++;
            if (_color >= Colors.Count)
                _color = 0;

            BadgeColor = Colors[_color];
            RaisePropertyChanged(nameof(BadgeColor));
        });

        public ICommand ChangeTextColorCommand => new Command(obj =>
        {
            _textColor--;
            if (_textColor < 0)
                _textColor = Colors.Count - 1;

            BadgeTextColor = Colors[_textColor];
            RaisePropertyChanged(nameof(BadgeTextColor));
        });
       
      
        public ICommand btnDoorUp => new Command((obj) =>
        {
            _fontIndex++;
            if (_fontIndex >= FontAttrs.Count)
                _fontIndex = 0;
            FontAttributes = FontAttrs[_fontIndex];
            RaisePropertyChanged(nameof(FontAttributes));
        });

        private int _color;
        private int _count = 1;
        private int _textColor;
        private int _fontIndex;
       
        public string Count => _count <= 0 ? string.Empty : _count.ToString();

        public int CountValue
        {
            get => _count;
            set
            {
                if (_count == value)
                    return;

                _count = value;
                RaisePropertyChanged(nameof(CountValue));
                RaisePropertyChanged(nameof(Count));
            }
        }

        public FontAttributes FontAttributes { get; private set; } = FontAttributes.Bold;
     public   String _source {
            get; private  set; }= "LightOff.png";
        public String Source
        {
            get => _source;
            set
            {
                if (_source == value)
                    return;

               _source = value;
                RaisePropertyChanged(nameof(Source));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
