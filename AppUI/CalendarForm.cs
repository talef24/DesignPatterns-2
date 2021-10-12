using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppLogic;
using FacebookWrapper.ObjectModel;

namespace AppUI
{
    public partial class CalendarForm : Form
    {
        private readonly AppManager r_AppManager;
        private DateTime m_LastDateTasksDisplayed;

        public CalendarForm()
        {
            InitializeComponent();
            r_AppManager = AppManager.Instance;
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime chosenDate = monthCalendar.SelectionStart;

            deleteCheckedTasks();
            clearPreviouslyData();
            fetchBirthdaysOfChosenDate(chosenDate);
            fetchEventsOfChosenDate(chosenDate);
            fetchTasksOfChosenDate(chosenDate);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            r_AppManager.SaveTasksinCalendar();
            deleteCheckedTasks();
        }

        private void fetchBirthdaysOfChosenDate(DateTime i_Date)
        {
            List<string> friendsList = r_AppManager.GetBirthdaysOfChosenDate(i_Date);

            foreach (string friendName in friendsList)
            {
                listBoxBirthday.Items.Add(friendName);
            }
        }

        private void fetchEventsOfChosenDate(DateTime i_Date)
        {
            List<string> eventsList = r_AppManager.GetEventsOfChosenDate(i_Date);

            foreach (string eventName in eventsList)
            {
                listBoxEvent.Items.Add(eventName);
            }
        }

        private void fetchTasksOfChosenDate(DateTime i_Date)
        {
            List<string> tasksList = r_AppManager.GetTasksOfChosenDate(i_Date);

            foreach (string task in tasksList)
            {
                checkedListBoxTask.Items.Add(task);
            }

            m_LastDateTasksDisplayed = i_Date;
        }

        private void deleteCheckedTasks()
        {
            foreach (string checkedTask in checkedListBoxTask.CheckedItems)
            {
                r_AppManager.RemoveTaskFromCalendar(m_LastDateTasksDisplayed, checkedTask);
            }
        }

        private void buttonAddTask_Click(object sender, EventArgs e)
        {
            if(textBoxAddTask.Text != string.Empty)
            {
                r_AppManager.AddTaskToCalendar(monthCalendar.SelectionStart, textBoxAddTask.Text);
                checkedListBoxTask.Items.Add(textBoxAddTask.Text);
                textBoxAddTask.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Please enter a task");
            }
        }

        private void clearPreviouslyData()
        {
            listBoxBirthday.Items.Clear();
            listBoxEvent.Items.Clear();
            checkedListBoxTask.Items.Clear();
        }
    }
}