using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;

internal class Program
{
    public static void Main(string[] args)
    {
        List<string> teamNames = new List<string>();
        string[] readTeamNameList = File.ReadAllLines("TeamNameList.txt");
        //make a string[] reading from a file for team names, then put them in a list
        //storing the names in the file means that they won't be permantly lost when you
        //restart the program
        foreach(string name in readTeamNameList)
        {
            teamNames.Add(name);
        }

        Console.Clear();
        programIntro();
        //make a variable that for the duration of this log will be one team
        string teamPicked = getTeamName(teamNames);
        getHitNumbers(teamPicked);
        Console.WriteLine(avgDigScore(5, teamPicked));
    }
    static void programIntro()
    {
        Console.WriteLine("Hello, this program will allow you to input data for volleyball stats and this will calculate the stats and record them!");
    }

    static string getTeamName(List<string> nameList)
    {
        int i = 0;
        int num;
        bool validAnswer = false;

        Console.WriteLine("Please select which team you are inputing stats for. using their corresponding number");
        foreach(string name in nameList)
        {
            Console.WriteLine($"{i+1}: {name}");
            i++;
        }
            Console.WriteLine($"{i+1}: other");
        do
        {
            Console.WriteLine("Make sure to use their corresponding number");
            validAnswer = int.TryParse(Console.ReadLine(), out num);

        }
        //num can't be less than 1 since the lowest option is 1, and it can't be greater than the number of items in the list
        //except that we have an extra option in case the team the want isn't on there
        while(!validAnswer || num < 1 || num > nameList.Count() + 1);
        if(num == nameList.Count() + 1)
        {
            return makeNewTeam();
        }
        
        return nameList[num - 1];
    }

    static string makeNewTeam()
    {
        Console.WriteLine("What is the name of your team/player?");
        string newTeam = Console.ReadLine();
        //creates new file with that team name and leaves a basic message
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{newTeam}.txt"), true))
        {
            outputFile.WriteLine("This will hold your desired stats, such as hitting percentage, errors, assists, aces etc...");
        }
        //write to the TeamNameList file the new team
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"TeamNameList.txt"), true))
        {
            outputFile.WriteLine(newTeam);
        }
        
        return newTeam;
    }

    //getHitNumbers() will collect the raw data from the user, then use the hitPercentage() method to make meaning of the data
    //then write it to the file for whatever team it may be

    static (double, double, double) getHitNumbers(string whichTeam)
    {
        (double kills, double attacks, double errors) hittingStats;

        Console.WriteLine("Please provide the number of attack attempts this player/team had");
        hittingStats.attacks = Convert.ToDouble(Console.ReadLine());
        //($"{whichTeam}.txt", $"{hittingStats.attacks} attack attempts were made");
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"{hittingStats.attacks} attack attempts.");
        }
        
        Console.WriteLine("Please provide the number of kills this player/team got");
        hittingStats.kills = Convert.ToDouble(Console.ReadLine());
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"{hittingStats.kills} kills.");
        }

        Console.WriteLine("Please provide the number of attack errors this player/team had");
        hittingStats.errors = Convert.ToDouble(Console.ReadLine());
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"{hittingStats.errors} attacking errors.");
        }
        
        return hittingStats;

    }

    //hitting percentage is calculated by doing (kills - errors)/total number of attacks
    //also even though it's called a percentage it is left as a decimal that goes to the thousandths
    //like baseball batting average
    static double hitPercentage((double kills, double attacks, double error) hittingStats, string whichTeam)
    {
        
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"Hitting percentage: ");
        }
        return (hittingStats.kills - hittingStats.error)/hittingStats.attacks;
    }

    //ace percentage is the (aces - errors)/attempts, similar to hitting it is just a decimal going to the thousandths
    static double acesPerSetPercentage(int serves, int aces, int error, string whichTeam)
    {
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"Ace percentage: ");
        }
        return (aces - error) / serves;
    }

    //dig percentages are calculated (digs - error)/attempts, similar to hitting it is just a decimal going to the thousandths
    static double digPercentages(double attempts, double digs, double errors, string whichTeam)
    {
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"Dig percentage: ");
        }

        return (digs - errors) / attempts;
    }

    //average dig score is calculated by summing all the dig scores and then dividing it by the number
    //of data points, unlike the other methods, only the numbers 0-3 are acceptable since dig scoring
    //is graded on a scale of 0-3, 3 being the best and 0 being the worst
    static double avgDigScore(double attempts, string whichTeam)
    {
        bool answer = false;
        int num;
        int sumDigScore = 0;
        for(int i = 0; i < attempts; i++)
        {
            Console.WriteLine($"Please give dig score number {i + 1}:");
            do
            {
                answer = int.TryParse(Console.ReadLine(), out num);
                if(!answer || num < 0 || num > 3)
                {
                    Console.WriteLine("Please give a whole number between 0 and 3");
                }
                else
                {
                    sumDigScore += num;
                }
            }
            while(!answer || num < 0 || num > 3);
        }
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($"Dig Score: {sumDigScore / attempts}");
        }
        return sumDigScore / attempts;
    }
}