private var motor : CharacterMotor;
public var canMove;

// Use this for initialization
function Awake () {
    canMove = true;
	motor = GetComponent(CharacterMotor);
}

// Update is called once per frame
function Update () {
    if (canMove) {
	var directionVector;

	// Get the input vector from keyboard or analog stick
	if (gameObject.tag == "Player") {
		directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	} else {
		//directionVector = new Vector3(Input.GetAxis("Joystick Move X"), 0, Input.GetAxis("Joystick Move Y"));
	}
	
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;

	if (gameObject.tag == "Player") {
		motor.inputJump = Input.GetButton("Jump");
	} else {
		//motor.inputJump = Input.GetButton("Joystick Jump");
	}
	}
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")