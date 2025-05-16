# **OpenGamedev**

*Empowering users to lead game development in a transparent, efficient, and community-driven way.*

## **Table of Contents**

* [Overview](#bookmark=id.dzs5qxu1ufc1)  
* [Development Process](#bookmark=id.d96a91vdfuwr)  
  * [Concept Phase](#bookmark=id.v9exkjrb37z1)  
  * [Development Cycle](#bookmark=id.ae5wo2v70uvl)  
* [Technical Implementation Concepts](#bookmark=id.7rw1p33xcucv)  
  * [Tasks and Solutions](#bookmark=id.6wo2xigkewl1)  
  * [Task Lifecycle and Queues](#bookmark=id.42wp8l8wwx4c)  
  * [Work Areas and Conflict Prevention](#bookmark=id.s5tj2haiij1n)  
  * [Dependencies](#bookmark=id.wm5l1qlmmtmh)  
  * [Superseded Tasks](#bookmark=id.esykabv8wzbm)  
  * [Background Processing](#bookmark=id.hc2o5eyzogs7)  
* [Current Challenges](#bookmark=id.otkdgdh0wlmv)  
* [Future Plans](#bookmark=id.fh7m0tmrzq7j)  
* [Credits](#bookmark=id.sw9mzdd88ihg)

## **Overview**

**OpenGamedev** is built on the idea of letting the community drive development. The project is designed to harness collective creativity and efficiency, ensuring that users not only propose ideas but also actively participate in turning them into reality.

## **Development Process**

### **Concept Phase**

* Contest:  
  Organize a contest to gather the best project concepts from the community. This initial step sets the tone for user engagement and innovation.

### **Development Cycle**

1. **Task Collection:** Gather tasks proposed by users.  
2. **Voting:** Conduct community voting to select the most promising tasks.  
3. **Time Estimation:** Estimate how long each task will take to complete.  
4. **Task Development:** Begin work on selected tasks.  
5. **Solution Gathering:** Collect solutions from various contributors.  
6. **Final Voting:** Vote to select the best solution.  
7. **Iteration:** Start the cycle again with a fresh collection of tasks.

## **Technical Implementation Concepts**

The core of the OpenGamedev platform relies on several key technical concepts and automated processes to manage the collaborative development workflow.

### **Tasks and Solutions**

* **Feature Request (Task):** Represents a distinct piece of work or a feature idea that needs to be implemented (e.g., "Implement Player Movement," "Create Goblin Model"). Tasks have a lifecycle, priority (determined by voting), and are associated with specific Work Areas.  
* **Solution:** A concrete implementation submitted by a contributor for a specific Feature Request. A Task can have multiple Solutions proposed by different users. Solutions go through automated checks (builds) and community voting to determine the accepted version.

### **Task Lifecycle and Queues**

Tasks progress through various statuses, managed automatically by the system:

* Proposed: A new task idea has been submitted.  
* TaskVoting: The task is currently being voted on by the community to determine its priority and whether it should be approved for implementation.  
* WaitingDependency: The task is approved but waiting for other tasks (its dependencies) to reach a certain status (ReadyForImplementation or Implemented).  
* ReadyForImplementation: The task has won voting (or been otherwise approved), its dependencies are met, and its required Work Areas are free. Contributors can now start working on and submitting Solutions for this task.  
* InProgress: Indicates that at least one Solution for this task is currently being worked on or processed (e.g., building, being reviewed). *Note: In this model, multiple solutions can be 'in progress' simultaneously for the same task.*  
* SolutionVotingExpired: Voting for solutions related to this task has ended without a winning solution.  
* Implemented: A Solution for this task has been accepted by the community (won voting) and successfully merged into the project's main codebase.  
* Superseded: The task has become obsolete because a higher-priority task covering the same Work Areas has been Implemented.  
* Closed: The task has been manually closed or archived.

The system maintains conceptual queues of tasks based on these statuses (e.g., a queue of tasks in TaskVoting, a queue of tasks in WaitingDependency).

### **Work Areas and Conflict Prevention**

* **Work Area:** Represents a specific, logically distinct part of the project's codebase or assets (e.g., "Code: Gameplay Mechanics", "Assets: Character Models", "Story: Main Quest"). Tasks are associated with one or more Work Areas.  
* **Conflict Prevention:** To avoid conflicting work on the same project parts, the system uses Work Areas to control the flow of tasks. A task can only transition to the ReadyForImplementation status if **all** the Work Areas it requires are not currently "occupied" by another task already in the Implemented status. This ensures that work on a specific area happens sequentially based on task priority.

### **Dependencies**

* **Task Dependencies:** A task can be marked as depending on other tasks. A task waiting for dependencies will remain in the WaitingDependency status until all tasks it depends on reach either ReadyForImplementation or Implemented status. This is managed automatically by the background processing service.

### **Superseded Tasks**

* **Alternative Resolution:** When a task is successfully Implemented (i.e., its winning solution is merged), the system checks for other tasks that are still in earlier statuses (Proposed, TaskVoting, WaitingDependency, ReadyForImplementation). If another task has a **lower priority** and requires the **exact same set of Work Areas** as the newly implemented task, it is automatically marked as Superseded. This mechanism ensures that if the community votes for a higher-priority alternative for a specific project area, less popular alternatives for the same area are automatically retired from the active workflow.

### **Background Processing**

* **Task Queue Background Service:** A dedicated background service (IHostedService / BackgroundService) runs continuously to automate the transitions between task statuses. It periodically performs checks:  
  * Moves tasks from WaitingDependency to ReadyForImplementation if dependencies are met.  
  * Moves tasks from TaskVoting to ReadyForImplementation if they win voting *and* their required Work Areas are free.  
  * Processes Accepted Solutions, attempts to merge them into the main codebase (interacting with a Git integration service).  
  * Upon successful merge (task becomes Implemented), triggers checks for Superseded tasks and re-evaluates dependencies.  
  * (Planned) Performs proactive checks for Git conflicts in solutions.

This automated system, driven by community voting and clear rules based on task statuses, dependencies, and Work Areas, aims to provide a transparent and efficient development pipeline.

## **Current Challenges**

One of the key challenges is efficiently scheduling tasks:

* Sequential Processing:  
  Processing tasks one after another may not utilize resources efficiently because many tasks can be executed in parallel.  
* Dependent Tasks System:  
  A system based on dependent tasks might discourage task creators from declaring dependencies—since a task without dependencies can start sooner—complicating the prioritization process.  
* Proposed Approach (Initial/Conceptual):  
  For now, the idea is to execute tasks sequentially within different streams. For example:  
  * **Main Quest Stream:** Tasks related to the main narrative, like the primary quest scenario.  
  * **Hero Model Stream:** Tasks involving the creation and development of the hero's model.  
  * **Enemy Behavior Stream:** Tasks dedicated to programming enemy actions and behavior.

This approach leaves room for optimization. Alongside these, additional tasks might run in parallel, such as writing side quest scenarios, creating enemy models, or developing day/night transition logic. *(Note: The Work Area system described above is the refined technical approach to address the challenges outlined here, moving beyond simple streams).*

## **Future Plans**

* **Frontend Development:** Build a dedicated frontend for a better user experience.  
* **Security Enhancements:** Implement protection measures against falsification.  
* **Reward System:** Develop a system to reward users for their contributions.  
* **Automated Build System:** Implement an automatic build process for submitted solutions, allowing users unfamiliar with the engine to evaluate results.  
  * Consider using CI/CD tools to trigger builds on push.  
  * Support both predefined engines and custom configurations.  
  * Ensure flexibility for adapting to different development environments.

## **Credits**

If you reuse or adapt any parts of this project, please give credit to **OlafOats** for the original concept and logic.