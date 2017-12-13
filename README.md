# support-wheel-of-fate

### ShiftController 
used to manage engineers turns find engineers save turns

### SHIFT_DURATION and MAX_SHIFTS_DURATION 
used to configure how much time takes each turn and the maximum amount of time an engineer can have in his N weeks period. To avoid creating a new table with only two values that will barely change, I decided to use environment variables to store them.

### CI/CD 
AppVeyor and Windows Azure

- DAL is just a folder not a class library

- create  Startup class diferent files of to hold different settings

### AuthController
- use JWT to authenticate user, even though theres is not solid user management implementation, token based authentication works just fine, and will make easier integrate with other clients.


### Architecture

there is only two layer on the project, DAL (Data Access Layer) and Controllers. As the project still in development and does not have many features create more layers like a Service or Facade could lead to needless complexity
