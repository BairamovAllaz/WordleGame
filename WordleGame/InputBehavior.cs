using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WordleGame;

public class InputBehavior
{
    public Dictionary<StackPanel, List<TextBlock>> GameDictionary { get; set; }
    public int IndexPlaceStackPanel { get; set; }
    public int IndexPlaceListTextBlock { get; set; }
    public  List<TextBlock> CurrentList { get; set; }
    private List<string> _words;
    private string _wordToCheck;
    private WebApiHandler _webApiHandler;
    public InputBehavior(Dictionary<StackPanel, List<TextBlock>> game)
    {
        _webApiHandler = new WebApiHandler(new HttpClient());
        _words = Task.Run(async () => await InitWords(_webApiHandler)).Result;
        _wordToCheck = _words.ElementAt(new Random().Next(0,_words.Count));
        GameDictionary = game;
        CurrentList = GameDictionary.ElementAt(IndexPlaceStackPanel).Value;
    }
    public InputBehavior()
    {
    }

    private async Task<List<string>> InitWords(WebApiHandler webApiHandler)
    {
        var wordString = await webApiHandler.GetWords();
        return WebApiHandler.ConvertToList(wordString);
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
        Trace.WriteLine(_wordToCheck);
        if (KeyIsBackSpaceAndNotZero(e))
        {
            ClearCurrentTextBlockText();
            return true;
        }

        if (IsKeyLetter(e))
        {
            if (IsIndexTextBlockIsLastWidthColumn() && IsKeyboardEnter(Key.Enter))
            {
                Check(GameDictionary.ElementAt(IndexPlaceStackPanel).Value);
                SetRowMapAndSetList();
            }
            else if(IsIndexStackPanelIsLastHeightColumn()) return true;
            else
            {
                WriteToCurrentTextBlock(e);
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    private void Check(List<TextBlock> column)
    {
        for (int i = 0; i < column.Count;++i)
        {
            var textBlock = column[i];
            for (int j = 0; j < column[i].Text.Length; j++)
            {
                var characther = textBlock.Text;
                var isContanins = _wordToCheck.Contains(characther);
                var indexW = _wordToCheck.IndexOf(characther, StringComparison.Ordinal);
                if (isContanins)
                {
                    if (indexW == j)
                    {
                        textBlock.Foreground = new SolidColorBrush(Colors.Green);
                        //it will be green
                    }
                    else
                    {
                        textBlock.Foreground = new SolidColorBrush(Colors.Gray);
                        //it will be gray
                    }
                }else
                {
                    textBlock.Foreground = new SolidColorBrush(Colors.Red);
                    //it will be red
                }
            }
        }
    }

    private bool IsKeyboardEnter(Key e)
    {
        return e == Key.Enter;
    }
    
    private bool IsKeyLetter(Key e) => e >= Key.A && e <= Key.Z;

    private bool IsIndexTextBlockIsLastWidthColumn() => IndexPlaceListTextBlock >= 5;

    private bool IsIndexStackPanelIsLastHeightColumn() => IndexPlaceStackPanel >= 6;

    private void ClearCurrentTextBlockText()
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
    private bool KeyIsBackSpaceAndNotZero(Key e) => e == Key.Back && IndexPlaceListTextBlock > 0;
}