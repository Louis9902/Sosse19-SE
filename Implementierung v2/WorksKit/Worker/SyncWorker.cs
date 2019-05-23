namespace WorksKit.Worker
{
    public class BackupWorker : BasicWorker
    {
        private readonly Preference<string> source;
        private readonly Preference<string> target;

        public BackupWorker()
        {
            source = Preferences.Preference("source", fallback: "c://example");
            target = Preferences.Preference("target", fallback: "c://example");
        }

        public string Source
        {
            get => source.Value;
            set => source.Value = value;
        }

        public string Target
        {
            get => target.Value;
            set => target.Value = value;
        }

        public override void StartWorker()
        {
            throw new System.NotImplementedException();
        }

        public override void AbortWorker()
        {
            throw new System.NotImplementedException();
        }
    }
}