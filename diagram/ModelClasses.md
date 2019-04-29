@startuml

 abstract class Worker {
    -identifier: Guid 
    #manager: WorkerManager
    
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
    -workers: List<Worker>
    -storage: String {File}

    +bool StartWorkers()
    +bool AbortWorkers()

    +Worker CreateWorker(id: Integer)
}

class FileObserver {
    +FileObserver(file: string)

    +Changes: List<String> {get} 
}

class Entrypoint {
    -{static} manager: WorkerManager

    {static} +void Main(args: string[])
}

class WorkerTypeRegister<< (S,#FF7700) Static >> {
    -map: Dictionary<Long, Type>
    +void Register()
    +Type SearchType(id: Long)
}

Entrypoint "1" -- "1" WorkerManager : starts >
WorkerManager "1" -- "n" Worker : manages >
FileObserver "1" -- "1" BackupWorker : keeps track of <

WorkerTypeRegister "1" -- "1" Entrypoint : registers <

@enduml
