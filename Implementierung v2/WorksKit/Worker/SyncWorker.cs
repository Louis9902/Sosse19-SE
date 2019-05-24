namespace WorksKit.Worker
{
    [WorkerType("2258b292-896a-4547-937c-6f0e865d5419")]
    public class SyncWorker : BasicWorker
    {
        private readonly Preference<string> source;
        private readonly Preference<string> target;

        public SyncWorker()
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