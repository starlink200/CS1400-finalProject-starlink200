/**************************************
* Name: Caleb Roskelley
* Project: Final CS1400 Project
*
* What's Left:
*  Requirements
*   const
*   pass-by-reference (in/ref/out)
*   2d or jagged arrays
*   string formatting
*  Finishing Program:
*   Creating overall avg
*   RPI
*   Comparing Teams
*************************************/
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        //a bool which will be used when checking if the user has more data to put in
        bool doAgain = false;
        do
        {

            Console.Clear();
            programIntro();
            
            List<string> teamNames = new List<string>();
            string[] readTeamNameList = File.ReadAllLines("TeamNameList.txt");
            //make a string[] reading from a file for team names, then put them in a list
            //storing the names in the file means that they won't be permantly lost when you
            //restart the program
            foreach(string name in readTeamNameList)
            {
                teamNames.Add(name);
            }
            //make a variable that for the duration of this log will be one team
            string teamPicked = getTeamName(teamNames);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{teamPicked}.txt"), true))
            {
                outputFile.WriteLine($"Hitting Stats");
            }
            hitPercentage(getHitNumbers(teamPicked), teamPicked);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{teamPicked}.txt"), true))
            {
                outputFile.WriteLine($"Defensive Stats");
            }
            (double attempts, double digs, double errors) digStats = getDigNumbers(teamPicked);
            digPercentages(digStats, teamPicked);
            avgDigScore(digStats.attempts, teamPicked);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{teamPicked}.txt"), true))
            {
                outputFile.WriteLine($"Serving Stats");
            }
            acesPerSetPercentage(getServeNumbers(teamPicked), teamPicked);
            doAgain = moreStats();
        }
        while(doAgain);
    }
    static void programIntro()
    {
        Console.WriteLine("Hello, this program will allow you to input data for volleyball stats and this will calculate the stats and record them!");
    }

    //getTeamName() is the method that allows the user to select which team they will be inputing stats for
    //it will use a list to display the options and then return a string which is a teams name, should correspond
    //with one of the text files
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

    //makeNewTeam() will ask the user for the name of their team and create a file to store that teams stats in
    //as well as add their name to the file that contains all of the team names
    static string makeNewTeam()
    {
        Console.WriteLine("What is the name of your team?");
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

    //to avoid having to use a huge amount of repitious code to get raw data for each actions(hitting, serving, digging)
    //the askForNumbers() method will take in 2 strings the name of what they are attempting i.e attacking and the goal action i.e. kills
    static (double, double, double) askForNumbers(string attemptName, string action, string whichTeam)
    {
        (double attempts, double action, double errors) stats;
        bool userInput = false;
        double num;

        Console.WriteLine($"Please provide the number of {attemptName} attempts your team had");
        do
        {
            userInput = double.TryParse(Console.ReadLine(), out num);
            if(!userInput)
            {
                Console.WriteLine("Please give a valid number");
            }

        }
        while(!userInput);
        stats.attempts = num;

        Console.WriteLine($"Please provide the number of {action} your team got");
        do
        {
            userInput = double.TryParse(Console.ReadLine(), out num);
            if(!userInput)
            {
                Console.WriteLine("Please give a valid number");
            }
            else if(num > stats.attempts)
            {
                Console.WriteLine("The number of {action} cannot be greater than the number of attempts");
                userInput = false;
            }

        }
        while(!userInput);
        stats.action = num;

        Console.WriteLine($"Please provide the number of {attemptName} errors your team had");
        do
        {
            string userAnswer = Console.ReadLine();
            userInput = double.TryParse(userAnswer, out num);
            //if the user would like to reinput the values the method will call itself and run again then return the new values
            if(userAnswer.ToLower().Equals("r"))
            {
                Console.WriteLine("user answer was r");
                return askForNumbers(attemptName, action, whichTeam);
            }
            else
            {
                if(!userInput)
                {
                    Console.WriteLine("Please give a valid number");
                }
                else if(num + stats.action > stats.attempts)
                {
                    Console.WriteLine($"Your number of errors combine with number of {action} cannot be greater than the number of attempts. If you need to reinput the number of attempts or {action} press R");
                    userInput = false;
                }
            }
        }
        while(!userInput);
        stats.errors = num;
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($" {stats.attempts} {attemptName} attempts.");
            outputFile.WriteLine($" {stats.action} {action}.");
            outputFile.WriteLine($" {stats.errors} {attemptName} errors.");
        }

        return stats;
    }
    
    //getHitNumbers() will collect the raw data from the user by using the askForNumbers method
    static (double, double, double) getHitNumbers(string whichTeam)
    {
        (double kills, double attacks, double errors) hittingStats;
    
        hittingStats = askForNumbers("attack", "kills", whichTeam);
        return hittingStats;

    }

    //hitting percentage is calculated by doing (kills - errors)/total number of attacks
    //also even though it's called a percentage it is left as a decimal that goes to the thousandths
    //like baseball batting average
    static double hitPercentage((double attacks, double kills, double error) hittingStats, string whichTeam)
    {
        
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($" Hitting percentage: {(hittingStats.kills - hittingStats.error)/hittingStats.attacks:N3}");
        }
        return (hittingStats.kills - hittingStats.error)/hittingStats.attacks;
    }

    //getServeNumers() will collect the raw data concerning serving from the user
    static (double, double, double) getServeNumbers(string whichTeam)
    {
        (double attempts, double aces, double errors) serveStats;
        serveStats = askForNumbers("service", "aces", whichTeam);
        return serveStats;
    }

    //ace percentage is the (aces - errors)/attempts, similar to hitting it is just a decimal going to the thousandths
    static double acesPerSetPercentage((double serves, double aces, double error) serveStats, string whichTeam)
    {
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($" Ace percentage: {(serveStats.aces - serveStats.error) / serveStats.serves:N3}");
        }
        return (serveStats.aces - serveStats.error) / serveStats.serves;
    }

    //getDigNumbers() will use askForNumbers() to get raw data regarding defensive stats
    static (double, double, double) getDigNumbers(string whichTeam)
    {
        (double attempts, double digs, double errors) digStats;
        
        digStats = askForNumbers("digs", "digs", whichTeam);
        return digStats;
    }
    
    //dig percentages are calculated (digs - error)/attempts, similar to hitting it is just a decimal going to the thousandths
    static double digPercentages((double attempts, double digs, double errors) digNumbers, string whichTeam)
    {
        using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{whichTeam}.txt"), true))
        {
            outputFile.WriteLine($" Dig percentage: {(digNumbers.digs - digNumbers.errors) / digNumbers.attempts:N3}");
        }

        return (digNumbers.digs - digNumbers.errors) / digNumbers.attempts;
    }

    //average dig score is calculated by summing all the dig scores and then dividing it by the number
    //of data points, unlike the other methods, only the numbers 0-3 are acceptable since dig scoring
    //is graded on a scale of 0-3, 3 being the best and 0 being the worst
    static double avgDigScore(double attempts, string whichTeam)
    {
        bool answer = false;
        double num;
        double sumDigScore = 0;
        for(int i = 0; i < attempts; i++)
        {
            Console.WriteLine($"Please give dig score number {i + 1}:");
            do
            {
                answer = double.TryParse(Console.ReadLine(), out num);
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
            outputFile.WriteLine($" Dig Score: {sumDigScore / attempts:N2}");
        }
        return sumDigScore / attempts;
    }

    static bool moreStats()
    {
        bool validAnswer = false;
        int num;
        Console.WriteLine("Would you like to input more stats for a seperate game?");
        do
        {
            Console.WriteLine("1: Yes");
            Console.WriteLine("2: No");
            validAnswer = int.TryParse(Console.ReadLine(), out num);
            if(!validAnswer || num < 1 || num > 2)
            {
                Console.WriteLine("Please give a valid answer by using the options corresponding number");
            }
        }
        while(!validAnswer || num < 1 || num > 2);
        switch(num)
        {
            case 1:
                return true;
            default:
                return false;
        }
    }
}