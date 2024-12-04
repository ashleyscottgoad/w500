using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using W500Core;

public class HtmlExtractor
{
    public static LbGameData ExtractGameData (string filePath)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.Load(filePath);

        var scriptNode = htmlDoc.DocumentNode.SelectSingleNode("//script[contains(text(), 'window.gameData')]");
        if (scriptNode == null)
        {
            throw new Exception("Script node containing 'window.gameData' not found.");
        }

        var scriptContent = scriptNode.InnerText;
        var startIndex = scriptContent.IndexOf("window.gameData = {") + "window.gameData = ".Length;
        var endIndex = scriptContent.IndexOf("}", startIndex) + 1;
        var gameDataJson = scriptContent.Substring(startIndex, endIndex - startIndex);
        var gameData = JsonSerializer.Deserialize<LbGameData>(gameDataJson.Trim());
        return gameData;
    }
}
