#Use Case
### 2.1 Start application  
_TYPE:_ Application start  
_DESCRIPTION:_ User or system is staerting the application  
_TRIGGER:_ User or system is starting the application  
_RESULT:_ The application has started in the background  
_ACTORS:_ User or system  
_INPUT INFO:_ Config und Storage file location  
_PRECONDITIONS:_ none  
_POSTCONDITIONS:_ none  
_STEPS:_ 1. User or system starts the application  
         2. Application reads the storage file from the disk  
         3. Application creates workers  
         4. Application creates a timer  

### 2.2 Stop application
_TYPE:_ Programm stop  
_DESCRIPTION:_ User or system stops the application  
_TRIGGER:_ PC is shutting down or user kills the app  
_RESULT:_ The application will shutdown  
_ACTORS:_ User or system  
_INPUT INFO:_ none  
_PRECONDITIONS:_ App is running  
_POSTCONDITIONS:_ none  
_STEPS:_ 1. User or system triggers a shutdown event  
         2. Application is finishing running jobs  
         3. Application is saving data to disk  
         4. Application is quiting  

### 1.1 Open GUI
_TYPE:_ UI start  
_DESCRIPTION:_ UI is starting and showing current config  
_TRIGGER:_ User opens the UI  
_RESULT:_ The application interface is shown  
_ACTORS:_ User  
_INPUT INFO:_ Config file  
_PRECONDITIONS:_ none  
_POSTCONDITIONS:_ none  
_STEPS:_ 1. User opens the UI  
         2. UI is parsing the config file  

### 1.2 Manage configuration
_TYPE:_ User modifies avaiable Jobs  
_DESCRIPTION:_ User can change, add or delete jobs  
_TRIGGER:_ PC starts or user restarts the app  
_RESULT:_ The application has started  
_ACTORS:_ User or Os  
_INPUT INFO:_ Storage file  
_PRECONDITIONS:_ none  
_POSTCONDITIONS:_ none  
_STEPS:_ 1. User or Os starts the application  

### 1.3 Exit graphical user interface
_TYPE:_ UI exit  
_DESCRIPTION:_ User closes the graphical user interface and the changes will be either saved or not   
_TRIGGER:_ User clicks a button  
_RESULT:_ The graphical user interface is closed and the changes either saved or not  
_ACTORS:_ User  
_INPUT INFO:_ If the user either wants to save the changes or not  
_PRECONDITIONS:_ The graphical user interface must be shown already  
_POSTCONDITIONS:_ none  
_STEPS:_ 1. User clicks a button  
         2. The graphical user interface closes  
