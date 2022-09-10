using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Controls;
using System.Windows.Input;

namespace WordleGame;

public class InputBehavior
{
    private Dictionary<StackPanel, List<TextBlock>> _dictionary;
    private int _indexPlaceStackPanel;
    private int _indexPlaceListTextBlock;
    private List<TextBlock> _currentList;
    public InputBehavior(Dictionary<StackPanel, List<TextBlock>> dictionary)
    {
        _dictionary = dictionary;
        _currentList = _dictionary.ElementAt(_indexPlaceStackPanel).Value;
    }
    public void Input_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyIsNumberOrNumpad(e)) return;
            if (KeyIsBackSpaceAndNotZero(e))
            {
                ClearCurrentTextBlockText();
                return;
            }
            else if (IsIndexTextBlockIsLastWidthColumn())
            {
                SetRowMapAndSetList();
            }
            else if (IsIndexStackPanelIsLastHeightColumn()) return;
            WriteToCurrentTextBlock(e);
            e.Handled = true;
        }

        private bool IsIndexTextBlockIsLastWidthColumn()
        {
            if (_indexPlaceListTextBlock >= 5)
            {
                return true;
            }
            return false;
        }
        
        private bool IsIndexStackPanelIsLastHeightColumn()
        {
            if (_indexPlaceStackPanel >= 6)
            {
                return true;
            }
            return false;
        }
        private void ClearCurrentTextBlockText()
        {
            _currentList[_indexPlaceListTextBlock - 1].Text = "";
            _indexPlaceListTextBlock--;
        }

        private void WriteToCurrentTextBlock(KeyEventArgs e)
        {
            if (e.Key != Key.Back)
            {
                _currentList[_indexPlaceListTextBlock].Text = e.Key.ToString();
            }
            ++_indexPlaceListTextBlock;
        }

        private void SetRowMapAndSetList()
        {
            ++_indexPlaceStackPanel;
            _currentList = _dictionary.ElementAt(_indexPlaceStackPanel).Value;
            _indexPlaceListTextBlock = 0;
        }


        private bool KeyIsNumberOrNumpad(KeyEventArgs e)
        {
            if (((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                 (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                return true;
            }
            return false;
        }

        private bool KeyIsBackSpaceAndNotZero(KeyEventArgs e)
        {
            if (e.Key == Key.Back && _indexPlaceListTextBlock > 0)
            {
                return true;
            }
            return false;
        }
}