using System.Windows.Forms;
using TinyTasksKit.Worker;

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
        }
        
    }
}