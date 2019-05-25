using System;
using System.IO;
using WorksKit.Worker.Group;
using WorksKit.Worker.Preferences;

namespace WorksKit.Worker
{
    [WorkerGroup("2258b292-896a-4547-937c-6f0e865d5419")]
    public class SyncWorker : DefaultWorker
    {
        private readonly Preference<string> source;
        private readonly Preference<string> target;

        private readonly ListPreference<string> caches;

        public SyncWorker()
        {
            source = Preferences.Preference("source", fallback: "c://fallback/source");
            target = Preferences.Preference("target", fallback: "c://fallback/target");
            caches = Preferences.ListPreference<string>("caches");
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
            using (var watcher = new FileSystemWatcher())
            {
                watcher.Path = source;
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                watcher.EnableRaisingEvents = true;

                while (Console.Read() != 'q') ;
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void AbortWorker()
        {
        }

        public override string ToString()
        {
            return $"SyncWorker {Source}, {Target}, {caches.Value.Count}";
        }
    }
}