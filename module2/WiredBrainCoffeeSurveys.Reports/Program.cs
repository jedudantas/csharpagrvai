using System;
using System.Collections.Generic;
using System.IO;

namespace WiredBrainCoffeeSurveys.Reports
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quitApp = false;


            do
            {
                Console.WriteLine("Please choose a report (rewards, comments, tasks)");
                var selectedReport = Console.ReadLine();

                switch (selectedReport)
                {
                    case "rewards":
                        GenerateWinnerEmails();
                        break;
                    case "comments":
                        GenerateCommentsReport();
                        break;
                    case "tasks":
                        GenerateTasksReport();
                        break;
                    case "quit":
                        quitApp = true;
                        break;
                    default:
                        Console.WriteLine("Meepo");
                        break;
                }

            } while (!quitApp);


            
        }

        public static void GenerateWinnerEmails()
        {
            var selectedEmails = new List<string>();
            int counter = 0;

            Console.WriteLine(Environment.NewLine+"Selected Winners Output");
            while (selectedEmails.Count < 2 && counter < Q1Results.Responses.Count)
            {
                var currentItem = Q1Results.Responses[counter];

                if (currentItem.FavoriteProduct == "Cappucino")
                {
                    selectedEmails.Add(currentItem.EmailAddress);
                    Console.WriteLine(currentItem.EmailAddress);
                }

                counter++;
            }

            File.WriteAllLines("WinnersReports.csv", selectedEmails);
        }

        public static void GenerateCommentsReport()
        {
            var comments = new List<string>();

            Console.WriteLine(Environment.NewLine + "Comments Output:");

            for (var i = 0; i < Q1Results.Responses.Count; i++)
            {
                var currentResponse = Q1Results.Responses[i];
                if (currentResponse.WouldRecommend < 7.0)
                {
                    Console.WriteLine(currentResponse.Comments);
                    comments.Add(currentResponse.Comments);
                }
            }

            foreach (var response in Q1Results.Responses)
            {
                if (response.AreaToImprove == Q1Results.AreaToImprove)
                {
                    Console.WriteLine(response.Comments);
                    comments.Add(response.Comments);
                }
            }

            File.WriteAllLines("CommentsReport.csv", comments);

        }

        public static void GenerateTasksReport()
        {
            var tasks = new List<string>();

            //Calculated Values
            double responseRate = Q1Results.NumberResponded / Q1Results.NumberSurveyed;
            double overallScore = (Q1Results.FoodScore + Q1Results.ServiceScore + Q1Results.CoffeeScore + Q1Results.PriceScore) / 4;

            //Boolean Values
            if (Q1Results.CoffeeScore < Q1Results.FoodScore)
            {
                tasks.Add("Investigate");
            }

            if (overallScore > 8)
            {
                tasks.Add("Reward staff");
            }
            else
            {
                tasks.Add("Work for better score");
            }

            if (responseRate < .33)
            {
                tasks.Add("Improve response rate");
            }
            else if (responseRate >= .33 && responseRate < .66)
            {
                tasks.Add("Send free coffee");
            }
            else
            {
                tasks.Add("Discount on coffee");
            }


            switch (Q1Results.AreaToImprove)
            {
                case "RewardsProgram":
                    tasks.Add("Revisit rewards");
                    break;

                case "Cleanliness":
                    tasks.Add("Contact the cleaning vendor");
                    break;

                case "MobileApp":
                    tasks.Add("Contact consulting firm about app");
                    break;
                default:
                    tasks.Add("Read comments");
                    break;
            }
            Console.WriteLine(Environment.NewLine+"Tasks Output:");
            foreach(var task in tasks)
            {
                Console.WriteLine(task);
            }

            File.WriteAllLines("TasksReport.csv", tasks);
        }
    }
}
