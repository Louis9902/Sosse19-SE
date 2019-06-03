using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using TinyTasksKit.Worker.Group;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksKit.Worker
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
            caches = Preferences.ListPreference<string>("caches").ToggleHidden();
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

                watcher.Changed += OnCommonFileEvent;
                watcher.Created += OnCommonFileEvent;
                watcher.Deleted += OnCommonFileEvent;
                watcher.Renamed += OnCommonFileEvent;

                watcher.EnableRaisingEvents = true;

                while (Console.Read() != 'q') ;
            }
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
                    File.Copy(srcNew, tgtNew);
                }

                return;
            }

            if (Directory.Exists(srcNew))
            {
                MoveOrMergeDirectory(srcNew, tgtNew);
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
                        if (!File.Exists(tgt)) File.Copy(src, tgt);
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
                        File.Copy(src, tgt);
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

                    if (!Directory.Exists(tgt)) Directory.Delete(tgt, true);
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

        public override void AbortWorker()
        {
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
                File.Move(current, next);
            }
        }
    }
}