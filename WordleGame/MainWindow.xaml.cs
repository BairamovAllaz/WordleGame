using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordleGame
{
    public partial class MainWindow : Window
    {
        private Dictionary<StackPanel, List<TextBlock>> _dictionary;
        public readonly int WIDTHGAMEAREA = 5;
        public readonly int HEIGHTGAMEAREA = 6;
        public MainWindow()
        {
            _dictionary = new Dictionary<StackPanel, List<TextBlock>>();
            InitializeComponent();
            InitGameObjects();
        }

        public void InitGameObjects()
        {
            InitGameAreaObject();
            for (int i = 0; i < HEIGHTGAMEAREA; i++)
            {
                StackPanel parent = new StackPanel();
                parent.Name = $"box{i}";
                parent.Orientation = Orientation.Horizontal;
                GameArea.Children.Add(parent);
                List<TextBlock> buffer = new List<TextBlock>();
                for (int j = 0; j < WIDTHGAMEAREA; j++)
                {
                    TextBlock textBlock = CreateAndInitTextBlockObject(nameOfTextBlock: j);
                    parent.Children.Add(textBlock);
                    buffer.Add(textBlock);
                }
                _dictionary.Add(parent, buffer);
            }
        }

        private void InitGameAreaObject()
        {
            GameArea.HorizontalAlignment = HorizontalAlignment.Center;
            GameArea.VerticalAlignment = VerticalAlignment.Center;
            GameArea.Width = 300;
            GameArea.Height = 300;
            GameArea.Background = new SolidColorBrush(Colors.Beige);
        }

        private TextBlock CreateAndInitTextBlockObject(int nameOfTextBlock)
        {
            return new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = $"N{nameOfTextBlock}",
                Width = 60,
                Height = 60,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
            };
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            InputBehavior inputBehavior = new InputBehavior(_dictionary);
            window.KeyDown += inputBehavior.Input_OnKeyDown;
        }
    }
}