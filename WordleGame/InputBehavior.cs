using System;
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
    public Dictionary<StackPanel, List<TextBlock>> GameDictionary { get; set; }
    public int IndexPlaceStackPanel { get; set; }
    public int IndexPlaceListTextBlock { get; set; }
    public  List<TextBlock> CurrentList { get; set; }
    public InputBehavior(Dictionary<StackPanel, List<TextBlock>> game)
    {
        GameDictionary = game;
        CurrentList = GameDictionary.ElementAt(IndexPlaceStackPanel).Value;
    }
    public InputBehavior()
    {
    }
    public void Input_OnKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            e.Handled = KeyBoardMoves(e.Key);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
    
    public bool KeyBoardMoves(Key e)
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
            else if(IsIndexStackPanelIsLastHeightColumn()) return true;
            WriteToCurrentTextBlock(e);
        }
        else
        {
            return false;
        }
        return true;
    }    
    
    private bool IsKeyLetter(Key e) => e >= Key.A && e <= Key.Z;

    private bool IsIndexTextBlockIsLastWidthColumn() => IndexPlaceListTextBlock >= 5;

    private bool IsIndexStackPanelIsLastHeightColumn() => IndexPlaceStackPanel >= 6;

        public void ClearCurrentTextBlockText()
    {
        CurrentList[IndexPlaceListTextBlock - 1].Text = "";
        IndexPlaceListTextBlock--;
    }

    private void WriteToCurrentTextBlock(Key e)
    {
        if (e != Key.Back)
        {
            CurrentList[IndexPlaceListTextBlock].Text = e.ToString();
        }
        ++IndexPlaceListTextBlock;
    }
    private void SetRowMapAndSetList()
    {
        ++IndexPlaceStackPanel;
        CurrentList = GameDictionary.ElementAt(IndexPlaceStackPanel).Value;
        IndexPlaceListTextBlock = 0;
    }
    public bool KeyIsBackSpaceAndNotZero(Key e) => e == Key.Back && IndexPlaceListTextBlock > 0;
}