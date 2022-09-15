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
    public Dictionary<StackPanel, List<TextBlock>> DictionaryToTest;
    public List<TextBlock> ListToTest;
    [SetUp]
    public void Init()
    {
        DictionaryToTest = new Dictionary<StackPanel, List<TextBlock>>();
        ListToTest = new List<TextBlock>()
        {
            new TextBlock(),
        };
        DictionaryToTest.Add(new StackPanel(),ListToTest);
        DictionaryToTest.Add(new StackPanel(),ListToTest);
    }

    [TearDown]
    public void CleanUp()
    {
        DictionaryToTest.Clear();
    }
    [Test]
    [Apartment((ApartmentState.STA))]
    public void KeyBoardMoves_PressBackButton_ReturnsEmptyString()
    {
            DictionaryToTest.ElementAt(0).Value[0].Text = "A";
            InputBehavior inputBehavior = new InputBehavior(DictionaryToTest);
            inputBehavior.IndexPlaceListTextBlock = 1;
            inputBehavior.KeyBoardMoves(Key.Back);
            Assert.AreEqual("",DictionaryToTest.ElementAt(0).Value[0].Text);
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
    public void KeyBoardMoves_PressLetterDictianoryWidthAndHeightOutRange_ThrowsArgumentNullException()
    {
        InputBehavior inputBehavior = new InputBehavior();
        inputBehavior.IndexPlaceStackPanel = 7;
        inputBehavior.IndexPlaceListTextBlock = 7;
        Assert.Throws<ArgumentNullException>(() => inputBehavior.KeyBoardMoves(Key.A));
    }
    [Test]
    [Apartment((ApartmentState.STA))]
    public void KeyBoardMoves_PreesedLetter_ChangesWIdthAndHeightAndSetList()
    {
        InputBehavior inputBehavior = new InputBehavior(DictionaryToTest)
        {
            IndexPlaceStackPanel = 0,
            IndexPlaceListTextBlock = 5
        };
        inputBehavior.KeyBoardMoves((Key.A));
        Assert.AreEqual(ListToTest,inputBehavior.CurrentList);
        Assert.AreEqual(1,inputBehavior.IndexPlaceStackPanel);
        Assert.AreEqual(0,inputBehavior.IndexPlaceListTextBlock);
    }
}