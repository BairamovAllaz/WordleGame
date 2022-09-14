using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NUnit.Framework;
using List = System.Windows.Documents.List;

namespace WordleGame.Test;

public class Tests
{
    [Test]
    [Apartment((ApartmentState.STA))]
    public void KeyBoardMoves_PressBackButton_ReturnsEmptyString()
    {
            Dictionary<StackPanel, List<TextBlock>> dictionary = new Dictionary<StackPanel,List<TextBlock>>();
            dictionary.Add(new StackPanel(),new List<TextBlock>
            {
                new TextBlock()
            });
            dictionary.ElementAt(0).Value[0].Text = "A";
            InputBehavior inputBehavior = new InputBehavior(dictionary);
            inputBehavior.IndexPlaceListTextBlock = 1;
            inputBehavior.KeyBoardMoves(Key.Back);
            Assert.AreEqual("",dictionary.ElementAt(0).Value[0].Text);
            Assert.AreEqual(0,inputBehavior.IndexPlaceStackPanel);
    }
    [Test]
    [Apartment((ApartmentState.STA))]
    public void KeyBoardMoves_PressNotLetter_ReturnsFalse()
    {
        InputBehavior inputBehavior = new InputBehavior();
        bool actual = inputBehavior.KeyBoardMoves(Key.D5);
        Assert.IsFalse(actual);
    }
    [Test]
    [Apartment((ApartmentState.STA))]
    public void KeyBoardMoves_PressLetterDictianoryWidthAndHeightOutRange_ReturnsFalse()
    {
        InputBehavior inputBehavior = new InputBehavior();
        inputBehavior.IndexPlaceStackPanel = 7;
        inputBehavior.IndexPlaceListTextBlock = 7;
        bool actual = inputBehavior.KeyBoardMoves((Key.A));
        Assert.IsFalse(actual);
    }
    [Test]
    [Apartment((ApartmentState.STA))]
    public void KeyBoardMoves_PreesedLetter_ChangesWIdthAndHeightAndSetList()
    {
        List<TextBlock> Listt = new List<TextBlock>()
        {
            new TextBlock()
        };
        Dictionary<StackPanel, List<TextBlock>> dictionary = new Dictionary<StackPanel, List<TextBlock>>();
        dictionary.Add(new StackPanel(),Listt);
        dictionary.ElementAt(0).Value[0].Text = "A";
        InputBehavior inputBehavior = new InputBehavior(dictionary)
        {
            IndexPlaceStackPanel = 0,
            IndexPlaceListTextBlock = 0
        };
        inputBehavior.KeyBoardMoves((Key.A));
        Assert.AreEqual(Listt,inputBehavior.CurrentList);
        Assert.AreEqual(1,inputBehavior.IndexPlaceStackPanel);
        Assert.AreEqual(0,inputBehavior.IndexPlaceListTextBlock);
    }
}