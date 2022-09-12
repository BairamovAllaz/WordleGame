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
    public Dictionary<StackPanel, List<TextBlock>> _dictionary { get; set; }
    private int _indexPlaceStackPanel;
    private int _indexPlaceListTextBlock;
    private List<TextBlock> _currentList;

    public InputBehavior(Dictionary<StackPanel, List<TextBlock>> dictionary)
    {
        _dictionary = dictionary;
        _currentList = _dictionary.ElementAt(_indexPlaceStackPanel).Value;
    }
    public InputBehavior(){}

    public void Input_OnKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = behavs(e.Key);
    }

    public bool behavs(Key e)
    {
        if (KeyIsBackSpaceAndNotZero(e))
        {
            ClearCurrentTextBlockText();
            return true;
        }
        if (IsKeyLetter(e))
        {
            if (IsIndexTextBlockIsLastWidthColumn())
            {
                SetRowMapAndSetList();
            }
            else if (IsIndexStackPanelIsLastHeightColumn()) return true;
            WriteToCurrentTextBlock(e);
        }
        return true;
    }    
    
    private bool IsKeyLetter(Key e) => e >= Key.A && e <= Key.Z;
    private bool IsIndexTextBlockIsLastWidthColumn() => _indexPlaceListTextBlock >= 5;

    private bool IsIndexStackPanelIsLastHeightColumn() => _indexPlaceStackPanel >= 6;

    public void ClearCurrentTextBlockText()
    {
        _currentList[_indexPlaceListTextBlock - 1].Text = "";
        _indexPlaceListTextBlock--;
    }

    private void WriteToCurrentTextBlock(Key e)
    {
        if (e != Key.Back)
        {
            _currentList[_indexPlaceListTextBlock].Text = e.ToString();
        }
        ++_indexPlaceListTextBlock;
    }
    private void SetRowMapAndSetList()
    {
        ++_indexPlaceStackPanel;
        _currentList = _dictionary.ElementAt(_indexPlaceStackPanel).Value;
        _indexPlaceListTextBlock = 0;
    }
    public bool KeyIsBackSpaceAndNotZero(Key e) => e == Key.Back && _indexPlaceListTextBlock > 0;
}