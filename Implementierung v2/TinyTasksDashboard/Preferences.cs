using System.Windows.Forms;
using TinyTasksKit.Worker;

namespace TinyTasksDashboard
{
    public partial class WorkerOptions : Form
    {
        private readonly IWorker worker;

        public WorkerOptions(IWorker worker)
        {
            this.worker = worker;
            InitializeComponent();
            Initialize();
        }

        public bool RequirementsAreSet { get; private set; }

        private void Initialize()
        {
            Group.Text = $@"Group: {worker.Group}";
            Label.Text = $@"Label: {worker.Label}";
        }
        
    }
}