using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        if (KeyIsBackSpaceAndNotZero(e))
        {
            ClearCurrentTextBlockText();
            return;
        }
        if (IsKeyLetter(e))
        {
            if (IsIndexTextBlockIsLastWidthColumn())
            {
                SetRowMapAndSetList();
            }
            else if (IsIndexStackPanelIsLastHeightColumn()) return;
            WriteToCurrentTextBlock(e);
            e.Handled = true;
        }
    }
    private bool IsKeyLetter(KeyEventArgs e) => e.Key >= Key.A && e.Key <= Key.Z;
    private bool IsIndexTextBlockIsLastWidthColumn() => _indexPlaceListTextBlock >= 5;

    private bool IsIndexStackPanelIsLastHeightColumn() => _indexPlaceStackPanel >= 6;

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
    private bool KeyIsBackSpaceAndNotZero(KeyEventArgs e) => e.Key == Key.Back && _indexPlaceListTextBlock > 0;
}