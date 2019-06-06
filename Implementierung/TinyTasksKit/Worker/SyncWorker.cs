using System;
using System.IO;
using TinyTasksKit.Worker.Group;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksKit.Worker
{
    [WorkerGroup("2258b292-896a-4547-937c-6f0e865d5419")]
    public class SyncWorker : DefaultWorker
    {
        private const string TempFileSuffix = ".tmp";

        private readonly ScalarPreference<string> source;
        private readonly ScalarPreference<string> target;

        private readonly ListPreference<string> caches;

        private FileSystemWatcher watcher;

        public SyncWorker()
        {
            source = Preferences.Preference<string>("source").UpdateDataType(PreferenceDataType.PathFolder);
            target = Preferences.Preference<string>("target").UpdateDataType(PreferenceDataType.PathFolder);
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

        private static string ResolveToRoot(string root, string current)
        {
            return current.Substring(root.Length + 1);
        }

        private string ResolveSourceToTarget(string path)
        {
            return Path.Combine(Target, ResolveToRoot(Source, path));
        }

        private string ResolveTargetToSource(string path)
        {
            return Path.Combine(Source, ResolveToRoot(Target, path));
        }

        private void OnRenameFileEvent(object sender, RenamedEventArgs args)
        {
            var srcOld = args.OldFullPath;
            var srcNew = args.FullPath;
            var tgtOld = ResolveSourceToTarget(srcOld);
            var tgtNew = ResolveSourceToTarget(srcNew);

            if (File.Exists(srcNew))
            {
                if (File.Exists(tgtOld))
                {
                    if (File.Exists(tgtNew)) return; // no merge of files
                    File.Move(tgtOld, tgtNew);
                }
                else
                {
                    CopyFileSafe(srcNew, tgtNew);
                }

                return;
            }

            if (Directory.Exists(srcNew))
            {
                MoveOrMergeDirectory(srcNew, tgtNew);
                Directory.Delete(tgtOld, true);
            }
        }

        private void OnCommonFileEvent(object sender, FileSystemEventArgs args)
        {
            var src = args.FullPath;
            var tgt = ResolveSourceToTarget(src);

            switch (args.ChangeType)
            {
                case WatcherChangeTypes.All:
                {
                    Logger.Trace("File System Event All: {0}", args);
                    break;
                }

                case WatcherChangeTypes.Created:
                {
                    if (File.Exists(src))
                    {
                        if (!File.Exists(tgt)) CopyFileSafe(src, tgt);
                        // src is file and tgt already exists, maybe override?
                        break;
                    }

                    if (!Directory.Exists(tgt)) Directory.CreateDirectory(tgt);
                    break;
                }

                case WatcherChangeTypes.Changed:
                {
                    if (File.Exists(src))
                    {
                        if (File.Exists(tgt)) File.Delete(tgt);
                        CopyFileSafe(src, tgt);
                        break;
                    }

                    // src is folder what should we do when folder triggers change?
                    break;
                }

                case WatcherChangeTypes.Deleted:
                {
                    if (File.Exists(tgt))
                    {
                        File.Delete(tgt);
                        break;
                    }

                    if (Directory.Exists(tgt)) Directory.Delete(tgt, true);
                    break;
                }

                case WatcherChangeTypes.Renamed:
                {
                    OnRenameFileEvent(source, (RenamedEventArgs) args);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void StartWorker()
        {
            watcher = new FileSystemWatcher
            {
                Path = source,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            watcher.Changed += OnCommonFileEvent;
            watcher.Created += OnCommonFileEvent;
            watcher.Deleted += OnCommonFileEvent;
            watcher.Renamed += OnCommonFileEvent;

            watcher.EnableRaisingEvents = true;
        }

        public override void AbortWorker()
        {
            watcher?.Dispose();
        }

        public override string ToString()
        {
            return $"SyncWorker {Source}, {Target}, {caches.Value.Count}";
        }

        private static void MoveOrMergeDirectory(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(sourceDir)) return;
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            foreach (var current in Directory.EnumerateDirectories(sourceDir))
            {
                var name = ResolveToRoot(sourceDir, current);
                var next = Path.Combine(targetDir, name);
                MoveOrMergeDirectory(current, next);
            }

            foreach (var current in Directory.EnumerateFiles(sourceDir))
            {
                var name = ResolveToRoot(sourceDir, current);
                var next = Path.Combine(targetDir, name);

                if (File.Exists(next)) File.Delete(next);
                CopyFileSafe(current, next);
            }
        }

        private static void CopyFileSafe(string src, string tgt)
        {
            var tmp = tgt + TempFileSuffix;
            File.Copy(src, tmp);
            File.Delete(tgt);
            File.Move(tmp, tgt);
        }
    }
}