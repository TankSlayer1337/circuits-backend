# circuits-backend

## Exercise examples

### No gear
* Jumping Jacks
* Run
* Pistol squat

### Machine
* Leg curl
* Seated calf raises
* Cable rows

### Free weights
* Bench press
* Deadlift
* Weighted lounges

### Gear
* Medicine ball russian twist
* Kettle bell swings
* Box jumps

### Assisted
* Assisted pull ups
* 


## How to model an exercise repetition
### Gear + amount model
You could think about it that either you do not use any gear (only bodyweight) or you use some kind of gear, e.g. a dumbbell. The gear can optionally have a load that is either positive (makes the exercise more difficult) or negative (assists in the exercise).

You would then need a way of keeping track of the repetition/set amount. An amount could be repetitions in the case of a deadlift, or seconds in the case of a plank, or minutes in the case of a run.

The relation between equipment and repetitions should be such that duplicate data is avoided (i.e. since multiple repetitions probably use the same equipment, each repetition should not contain the information about the equipment).

The repititions and which equipment that was used needs to be ordered. e.g. one could do a set of DB shrugs consisting of 5 reps of heavy weight and 5 of lighter.

### Resistance model
