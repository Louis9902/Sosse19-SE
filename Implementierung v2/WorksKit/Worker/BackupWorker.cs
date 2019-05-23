namespace WorksKit.Worker
{
    public class BackupWorker : BasicWorker
    {
        private readonly Property<string> source;
        private readonly Property<string> target;

        public BackupWorker()
        {
            source = Property("source", fallback: "c://example");
            target = Property("target", fallback: "c://example");
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
    }
}