using System;
using System.Data.SqlClient;
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
            source = Preferences.Preference<string>("source");
            target = Preferences.Preference<string>("target");
            caches = Preferences.ListPreference<string>("caches").MakeHidden();
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
            var oldTargetPath = e.OldFullPath.Replace(Source, Target);
            var newTargetPath = e.FullPath.Replace(Source, Target);


            if (Directory.Exists(e.FullPath))
            {
                Directory.Move(oldTargetPath, newTargetPath);
            }
            else
            {
                if (File.Exists(newTargetPath)) File.Move(oldTargetPath, newTargetPath);
                else throw new NotImplementedException();
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var targetPath = e.FullPath.Replace(Source, Target);
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                case WatcherChangeTypes.Created:
                {
                    if (!File.Exists(e.FullPath)) return;
                    if (File.Exists(targetPath)) File.Copy(e.FullPath, targetPath);
                    break;
                }
                case WatcherChangeTypes.Deleted:
                {
                    if (File.Exists(targetPath)) File.Delete(targetPath);
                    break;
                }
            }
        }

        public override void AbortWorker()
        {
        }

        public override string ToString()
        {
            return $"SyncWorker {Source}, {Target}, {caches.Value.Count}";
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}