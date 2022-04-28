using HtmlAgilityPack;
using System.Net;
using ConsoleTableExt;
using Newtonsoft.Json;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

HtmlWeb web = new HtmlWeb();
HtmlDocument doc = web.Load("https://www.basketball-reference.com/boxscores/");

var MainTitle = doc.DocumentNode.SelectNodes("//*[@id='content']/h1").First().InnerHtml;

var SubTitle = doc.DocumentNode.SelectNodes("//*[@id='content']/div[2]/h2").First().InnerHtml;

var PlayedGames = doc.DocumentNode.SelectNodes("//*[@id='content']/div[3]/div");

Console.Clear();

Console.WriteLine($"Showing Results For {MainTitle}\n");

Console.WriteLine($"{SubTitle} Have Been Played Today..\n");

List<Result> Results = new List<Result>();

foreach (var game in PlayedGames)
{
    var Winner = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[2]/td[1]/a").InnerHtml;
    var Loser = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[1]/td[1]/a").InnerHtml;
    var WinningScore = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[2]/td[2]").InnerHtml;
    var LosingScore = game.SelectSingleNode($"//div[{PlayedGames.IndexOf(game) + 1}]/table[1]/tbody/tr[1]/td[2]").InnerHtml;

    Results.Add(new Result{
        Winner = Winner,
        Loser = Loser,
        WinningScore = WinningScore,
        LosingScore = LosingScore
    });

    Console.WriteLine($"{Winner} ({WinningScore} : {LosingScore}) {Loser}");
}

string EmailBody = $"<table><tr><th>Winner</th><th>Score</th><th>Loser</th><th>Score</th>";

    foreach (var Result in Results)
    {
        EmailBody += $"<tr><td>{Result.Winner}</td><td>{Result.WinningScore}</td><td>{Result.Loser}</td><td>{Result.LosingScore}</td></tr>";
    }

EmailBody += "</table>";

// create message

var email = new MimeMessage();
email.From.Add(MailboxAddress.Parse("test@gmail.com"));
email.To.Add(MailboxAddress.Parse("test@icloud.com"));
email.Subject = "Your Daily Results";
email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = EmailBody};

// send message

using (var client = new SmtpClient()) {
    client.Connect("smtp.gmail.com", 587);

    client.Authenticate("test@gmail.com", "passwr");

    client.Send(email);
    client.Disconnect(true);
}

Console.WriteLine("Email Sent, Press Any Key To Close");
Console.ReadLine();

public class Result {
    public string Winner {get; set;}
    public string Loser {get; set;}
    public string WinningScore {get; set;}
    public string LosingScore {get; set;}
}

