using HtmlAgilityPack;

HtmlWeb web = new HtmlWeb();
HtmlDocument doc = web.Load("https://www.basketball-reference.com/boxscores/");

var MainTitle = doc.DocumentNode.SelectNodes("//*[@id='content']/h1").First().InnerHtml;

var SubTitle = doc.DocumentNode.SelectNodes("//*[@id='content']/div[2]/h2").First().InnerHtml;

var PlayedGames = doc.DocumentNode.SelectNodes("//*[@id='content']/div[3]/div");

Console.Clear();

Console.WriteLine($"Showing Results For {MainTitle}\n");

Console.WriteLine($"{SubTitle} Have Been Played Today..\n");

foreach (var game in PlayedGames)
{

    var Winner = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[2]/td[1]/a").InnerHtml;
    var Loser = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[1]/td[1]/a").InnerHtml;
    var WinningScore = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[2]/td[2]").InnerHtml;
    var LosingScore = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[1]/td[2]").InnerHtml;

    Console.WriteLine($"{Winner} ({WinningScore} : {LosingScore}) {Loser}");

}

Console.WriteLine("\nPress any key to close.");
Console.ReadLine();

