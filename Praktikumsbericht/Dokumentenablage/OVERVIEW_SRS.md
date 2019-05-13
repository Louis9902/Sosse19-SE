# Software Requirements Specification

## 1. Introduction
### 1.1 Purpose
The purpose of this document is to give a detailed description of the functional and nonfunctional requirements for the application “Backupper”. It will illustrate the purpose and the complete explanation for the development of the system. The document will also explain system limitations and interfaces, this is primarily intended to be proposed to a customer for its approval and a reference for developing the first version of the system.

### 1.2 Scope
The “Backupper” software is an application software which helps people to create easily backups of their data. The application should be free to download and fully customizable for the user. Users can create their own backup tasks via a graphical user interface or directly by editing a configuration file. The created tasks can be started by different events, which are predefined by the software. One of these triggers should be able to observe a file change and store the changes until a threshold is reached. After this the files should be copied to a desired location. To guarantee that no file changes are missed during the reboot of the computer, these changes must be persistent and wrote to disk of the have not reached the threshold. The application must also be independent of any geographical areas as well as independent of the operating system.

### 1.3 Definitions, acronyms, and abbreviations
| Term  | Definition                                 |
|-------|--------------------------------------------|
| User  | Someone who interacts with the application |
| DESCR | Description                                |
| DEPED | Dependencies                               |

### 1.4 References
[IEEE Software Engineering Standards Committee, “IEEE Std 830-1998, IEEE Recommended
Practice for Software Requirements Specifications”, October 20, 1998][1]

### 1.5 Overview
The following parapraphs of this document provide an overview of the system functionality. This chapter also introduces the stakeholder and his interaction with the system.  Further, the chapter also mentions the system constraints and assumptions about the product.

## 2 Overall description
### 2.1 Product perspective
### 2.2 Product functions
### 2.3 User characteristics
> ~~Stakeholders~~
### 2.4 Constraints

## 3. Specific requirements
### 3.1 External interface Requirements
> This section provides a detailed description of all interfaces for the software. 

#### 3.1.1 User interfaces

#### 3.1.2 Software interfaces

### 3.2 Functional Requirements
> This section includes the requirements that specify all the fundamental actions of the software system.

#### 3.2.1 Functional Requirement 1
_**NAME:**_ F-REQ-01  
_**PRIORITY:**_ medium  
_**TITLE:**_ Configurable through a graphical user interface  
_**DESCR:**_ Allows the user to create new and configure existing backup plans with a easy to use and intuitive graphical user interface.  
_**DEPED:**_ none  

#### 3.2.2 Functional Requirement 2
_**NAME:**_ F-REQ-02  
_**PRIORITY:**_ high  
_**TITLE:**_ Different triggers for staring a backup  
_**DESCR:**_ Different action can start a backup task like a file changes inside a folder or a certain time has expired.  
_**DEPED:**_ none 

#### 3.2.3 Functional Requirement 3
_**NAME:**_ F-REQ-03  
_**PRIORITY:**_ high  
_**TITLE:**_ Application data should be persistent throughout system restarts  
_**DESCR:**_ The application data must be persistent throughout system restarts, this includes which files are modified but not saved.  
_**DEPED:**_ none 

#### 3.2.4 Functional Requirement 4
_**NAME:**_ F-REQ-04  
_**PRIORITY:**_ high  
_**TITLE:**_ Copying of files and directories shall be fail-safe  
_**DESCR:**_ When copying files these should be copyied first to a temporary file until this is correctly completed and afterwards just renamed to the correct file while the old file is just replaced with the temporary copy.  
_**DEPED:**_ none 

### 3.3 Non-Functional Requirements
> This section includes the requirements that specify all the non fundamental actions of the software system.

#### 3.2.1 Non-Functional Requirement 1
_**NAME:**_ Q-REQ-01  
_**TITLE:**_ Compression of the source directories  
_**DESCR:**_ When copying the files to a desired location they will be compressed in some way.  
_**DEPED:**_ none  

#### 3.2.2 Non-Functional Requirement 2
_**NAME:**_ Q-REQ-02  
_**TITLE:**_ Application should not be dependent on any geographical areas  
_**DESCR:**_ The application should be able to let the user choose from multiple languages.  
_**DEPED:**_ none  

[1]: https://standards.ieee.org/standard/830-1998.html
