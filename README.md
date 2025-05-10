---

# OpenGamedev

*Empowering users to lead game development in a transparent, efficient, and community-driven way.*

---

## Table of Contents

- [Overview](#overview)
- [Development Process](#development-process)
  - [Concept Phase](#concept-phase)
  - [Development Cycle](#development-cycle)
- [Current Challenges](#current-challenges)
- [Future Plans](#future-plans)
- [Credits](#credits)

---

## Overview

**OpenGamedev** is built on the idea of letting the community drive development. The project is designed to harness collective creativity and efficiency, ensuring that users not only propose ideas but also actively participate in turning them into reality.

---

## Development Process

### Concept Phase

- **Contest:**  
  Organize a contest to gather the best project concepts from the community. This initial step sets the tone for user engagement and innovation.

### Development Cycle

1. **Task Collection:** Gather tasks proposed by users.
2. **Voting:** Conduct community voting to select the most promising tasks.
3. **Time Estimation:** Estimate how long each task will take to complete.
4. **Task Development:** Begin work on selected tasks.
5. **Solution Gathering:** Collect solutions from various contributors.
6. **Final Voting:** Vote to select the best solution.
7. **Iteration:** Start the cycle again with a fresh collection of tasks.

---

## Current Challenges

One of the key challenges is efficiently scheduling tasks:

- **Sequential Processing:**  
  Processing tasks one after another may not utilize resources efficiently because many tasks can be executed in parallel.

- **Dependent Tasks System:**  
  A system based on dependent tasks might discourage task creators from declaring dependencies—since a task without dependencies can start sooner—complicating the prioritization process.

- **Proposed Approach:**  
  For now, the idea is to execute tasks sequentially within different streams. For example:
  
  - **Main Quest Stream:** Tasks related to the main narrative, like the primary quest scenario.
  - **Hero Model Stream:** Tasks involving the creation and development of the hero's model.
  - **Enemy Behavior Stream:** Tasks dedicated to programming enemy actions and behavior.

  This approach leaves room for optimization. Alongside these, additional tasks might run in parallel, such as writing side quest scenarios, creating enemy models, or developing day/night transition logic.

---
## Future Plans

- **Frontend Development:** Build a dedicated frontend for a better user experience.
- **Security Enhancements:** Implement protection measures against falsification.
- **Reward System:** Develop a system to reward users for their contributions.
- **Automated Build System:** Implement an automatic build process for submitted solutions, allowing users unfamiliar with the engine to evaluate results.  
  - Consider using CI/CD tools to trigger builds on push.  
  - Support both predefined engines and custom configurations.  
  - Ensure flexibility for adapting to different development environments.  
---

## Credits

If you reuse or adapt any parts of this project, please give credit to **OlafOats** for the original concept and logic.

---
