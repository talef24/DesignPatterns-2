using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace AppLogic
{
    internal class Calendar
    {
        private const int k_NumOfDigitsInMonth = 2;
        private const int k_NumOfDigitsInDay = 2;
        private const int k_IndexOfMonthInString = 3;
        private const int k_IndexOfDayInString = 0;
        private readonly User r_LoggedInUser;
        private readonly Dictionary<string, List<string>> r_Tasks = new Dictionary<string, List<string>>();
        private List<List<string>> m_TasksToRemember = new List<List<string>>();
        private List<string> m_DatesToRemember = new List<string>();

        internal Calendar(User i_LoggedInUser)
        {
            r_LoggedInUser = i_LoggedInUser;
        }

        internal List<List<string>> TasksToRemember
        {
            get { return m_TasksToRemember; }
            set { m_TasksToRemember = value; }
        }

        internal List<string> DatesToRemember
        {
            get { return m_DatesToRemember; }
            set { m_DatesToRemember = value; }
        }

        internal void AddTask(string i_Date, string i_Task)
        {
            if (!r_Tasks.ContainsKey(i_Date))
            {
                r_Tasks.Add(i_Date, new List<string>());   
            }

            r_Tasks[i_Date].Add(i_Task);
        }

        internal void RemoveTask(string i_Date, string i_Task)
        {
            r_Tasks[i_Date].Remove(i_Task);
            if(r_Tasks[i_Date].Count == 0)
            {
                r_Tasks.Remove(i_Date);
            }
        }

        internal List<string> GetTasksOfTheDay(DateTime i_Date)
        {
            List<string> tasks = new List<string>();

            if (r_Tasks.ContainsKey(i_Date.ToShortDateString()))
            {
                tasks = r_Tasks[i_Date.ToShortDateString()];
            }

            return tasks;
        }

        internal List<string> GetBirthdayFriendsOfTheDay(DateTime i_Date)
        {
            string monthOfChosenDate = i_Date.ToShortDateString().Substring(k_IndexOfMonthInString, k_NumOfDigitsInMonth);
            string dayOfChosenDate = i_Date.ToShortDateString().Substring(k_IndexOfDayInString, k_NumOfDigitsInDay);
            string date = string.Format("{0}/{1}", monthOfChosenDate, dayOfChosenDate);
            List<string> friendsNames = new List<string>();

            foreach (User friend in r_LoggedInUser.Friends)
            {
                if (friend.Birthday != null)
                {
                    if (friend.Birthday.Substring(0, 5).Equals(date))
                    {
                        friendsNames.Add(friend.Name);
                    }
                }
            }

            return friendsNames;
        }

        internal List<string> GetEventsOfTheDay(DateTime i_DateTime)
        {
            List<string> events = new List<string>();

            foreach (Event currentEvent in r_LoggedInUser.Events)
            {
                if (currentEvent.StartTime <= i_DateTime && currentEvent.EndTime >= i_DateTime)
                {
                    events.Add(currentEvent.Name);
                }
            }

            return events;
        }

        internal void SaveTasksAndDates()
        {
            List<List<string>> tasks = new List<List<string>>();
            List<string> dates = new List<string>();
            
            foreach (string key in r_Tasks.Keys)
            {
                if(r_Tasks.TryGetValue(key, out List<string> tasksOfOneDay))
                {
                    tasks.Add(tasksOfOneDay);
                    dates.Add(key);
                }
            }

            m_TasksToRemember = tasks;
            m_DatesToRemember = dates;
        }

        internal void UpdateDictionary()
        {
            for(int i = 0; i < m_TasksToRemember.Count; i++)
            {
                r_Tasks.Add(m_DatesToRemember[i], m_TasksToRemember[i]);
            }
        }
    }
}