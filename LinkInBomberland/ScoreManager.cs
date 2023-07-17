using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinkInBomberland
{
    internal class ScoreManager
    {
        bool newRecord;
        public ScoreManager() 
        {
            newRecord = false;
        }

        private String readHighScore()
        {
            return File.ReadAllText("C:\\Users\\tingmon\\Downloads\\gibhub backup" +
                "\\link_in_bomberland-master\\LinkInBomberland\\highScore.txt");
        }

        public String showHighScoreScene()
        {
            String rawText = readHighScore();

            List<int> topTenList = sortScoreString(rawText);
            
            return printHighScoreString(topTenList, ".");
        }

        // sort the numbers before return - for read 
        private List<int> sortScoreString(String rawText) 
        {
            List<int> scores = new List<int>();
            String[] strArr = rawText.Split(new char[] { '\n' });
            foreach (String str in strArr)
            {
                int score = 0;
                if (Int32.TryParse(str,out score))
                {
                    scores.Add(score);
                }

            }
            scores.Sort();
            scores.Reverse();
            return scores;
        }

        // sort the numbers before return - for write
        private List<int> sortScoreString(String rawText, int newScore)
        {
            List<int> scores = new List<int>();
            scores.Add(newScore);
            String[] strArr = rawText.Split(new char[] { '\n' });
            foreach (String str in strArr)
            {
                int score = 0;
                if (Int32.TryParse(str, out score))
                {
                    scores.Add(score);
                }
            }
            scores.Sort();
            scores.Reverse();
            //scores.RemoveRange(scores.Count - 2, 2);
            if (scores.Contains(newScore))
            {
                newRecord = true;
            }
            else
            {
                newRecord = false;
            }
            return scores;
        }

        private String printHighScoreString(List<int> topTenList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int score in topTenList)
            {
                sb.Append(score);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private String printHighScoreString(List<int> topTenList, String delimiter)
        {
            int rank = 0;
            StringBuilder sb = new StringBuilder();
            foreach (int score in topTenList)
            {
                sb.Append(++rank + delimiter + " ");
                sb.Append(score);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public bool updateHighScore(int newScore)
        {
            // read
            String scoreString = readHighScore();

            // sort
            List<int> scores = sortScoreString(scoreString, newScore);

            scoreString = printHighScoreString(scores);

            writeHighScore(scoreString);

            return newRecord;
        }

        private void writeHighScore(String newScoreString)
        {
            using (StreamWriter writetext = new StreamWriter("C:\\Users\\tingmon\\Downloads\\gibhub backup" +
                "\\link_in_bomberland-master\\LinkInBomberland\\highScore.txt", false))
            {
                writetext.WriteLine(newScoreString);
            }
        }
    }
}
