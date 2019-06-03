using System;
using System.Windows.Forms;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksDashboard
{
    public partial class Parameters : Form
    {
        private readonly IWorker worker;

        public Parameters(IWorker worker)
        {
            this.worker = worker;
            InitializeComponent();
            Initialize();
        }

        public bool RequirementsAreSet { get; private set; } = true;

        private void Initialize()
        {
            Group.Text = $@"Group: {worker.Group}";
            Label.Text = $@"Label: {worker.Label}";

            foreach (IPreference preference in worker.Preferences)
            {
                if (preference.IsHidden) continue;

                Options.Rows.Add(preference.Name, preference.ToDisplayString());
            }
        }

        private void Terminate()
        {
            var preferences = worker.Preferences;

            for (var index = 0; index < Options.Rows.Count-1; index++)
            {
                var row = Options.Rows[index];
                var name = row.Cells[0].Value as string;
                var value = row.Cells[1].Value as string;
                preferences[name]?.FromDisplayString(value);
            }

            foreach (IPreference preference in preferences)
            {
                if (preference.IsSatisfied) continue;
                RequirementsAreSet = false;
                break;
            }
        }

        private void OnOkayClick(object sender, EventArgs args)
        {
            Terminate();
            Close();
        }
    }
}