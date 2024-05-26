using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TetraPolyGame;

[TestClass]
public class MonopolyGameTests
{
    [TestMethod]
    public void MovePlayer_NormalMove_Success()
    {


        var player = new Player("TestPlayer", 100); // Create a player instance
        player.SetPos(5); // Set initial position
        var mainWindow = new MainWindow(); // Create a Monopoly game 

        player.RollDice();

        // Assert
        Assert.AreEqual(7, player.GetPosition()); // Expected position after rolling dice (e.g., move = 3, move2 = 4)
        Assert.AreEqual(300, player.Money); // Expected money after passing "Go" (e.g., 200 added)
    }
}
