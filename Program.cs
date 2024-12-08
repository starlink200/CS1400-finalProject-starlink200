using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Clear();
        programIntro();
        (double kills, double attacks, double errors) hittingStats;

        Console.WriteLine("Please provide the number of kills this player got");
        hittingStats.kills = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Please provide the number of attack attempts this player had");
        hittingStats.attacks = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Please provide the number of attack errors this player had");
        hittingStats.errors = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine(hitPercentage(hittingStats));
    }
    static void programIntro()
    {
        Console.WriteLine("Hello, this program will allow you to input data for volleyball stats and this will calculate the stats and record them!");
    }

    //hitting percentage is calculated by doing (kills - errors)/total number of attacks
    //also even though it's called a percentage it is left as a decimal that goes to the thousandths
    //like baseball batting average
    static double hitPercentage((double kills, double attacks, double error) hittingStats)
    {
        Console.Write("This players hitting percentage is: ");
        return (hittingStats.kills - hittingStats.error)/hittingStats.attacks;
    }

    //ace percentage is the (aces - errors)/attempts
    static double acesPerSetPercentage(int serves, int aces, int error)
    {
        return 1;
    }

    static double digPercentages(double digs)
    {
        return 1;
    }
}