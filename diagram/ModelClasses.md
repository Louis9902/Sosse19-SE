@startuml

 abstract class Worker {
    -identifier: Integer 
    #manager : WorkerManager
    
    #Worker(identifier: Integer, manager: WorkerManager)

    {abstract} +bool Start(reader: TextReader)
    {abstract} +bool Abort(writer: TextWriter)
}

class BackupWorker extends Worker  {
    -source: String {File, Directory}
    -target: String {File, Directory}
    -task: Task
    -observer: FileObserver

    +BackupWorker(identifier: Integer, manager: WorkerManager, source: String, target: String)

    +bool Start(TextReader reader)
    +bool Abort(TextWriter writer)
}
 
class WorkerManager {
    -workers : List<Worker>
    -storage : String {File}

    +bool StartService()
    +bool AbortService()

    +Worker CreateWorker(id: Integer)
}

class FileObserver {
    +FileObserver(file: string)

    +List<String> Changes {get} 
}

class Entrypoint {
    -{static} manager : WorkerManager

    {static} +void Main(args: string[])
}

Entrypoint "1" -- "1" WorkerManager
WorkerManager "1" -- "n" Worker
FileObserver "1" -- "1" BackupWorker

@enduml