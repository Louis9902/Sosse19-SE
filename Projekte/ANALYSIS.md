# Analysis

## Concept
* configuration driven backups with certain triggers:
  - file modification
  - time based system
* compression of the backups (zip|tar)
* user interface for creating and manipulating the configuration files
  - the structure of the jobs is defined in the configuration file
  - jobs creation is independent from the user interface 

## User Stories
* user wants to regularly back up his game stats to another drive
* every time there is a change to a file in a directory the user wants to copy the file also to another drive
* a user has a timed and changed based backup plan, but he is restarting his computer, the unsaved changes should not be lost during this restart 

## Conditions
* data driven task definition (xml|json)
  - [xml object mapping](https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization)
* modification data must be persistent throughout the restarts of the program
  - if the program if forced to exit but there are cached file changes, these should be saved to disk
* while copying the files it must be ensured that the backup is left untouched until it is verified that the copied file is not corrupted during the copy process
  - when `C:/Games/Something/File.xy` must be copied to `D:/Backups/Games/File.xy` it should first be copied to a temporary file like `D:/Backups/Games/File.xy.tmp` and after this has successfully finished it should be renamed to the destination file
* efficient file comparison
  - [file watcher](https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher)
  - cache hashes