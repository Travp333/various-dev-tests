//KNOWN ISSUES:
//animations scale you up a little bit
// in air light goes on when in water
// what if i have an empty on the character that rotates around you based on your input then i draw a line between the charcater and the empty to determine the rotation?
// still having issues related to velocity deciding rotation, such as jumping and pressing against a sloped surface
// if you press forward then backward very quickly afterwards youll continue moving forwards just really really slowly. also applied to climbing 
// make the default be that stuff is not climbable, and make it so you have to manually say what is climbable. the current way is annoying 
// animations tied to being on a wall, like climbing and wallrunning, will still trigger if youre just going against a little ledge. maybe do like 5 raycasts from the body and if 3 or more return a climbable thing / a wall then it considers that valid? is that too intensive?
// wall slide animations should only play if you are pushing into that wall, rather than just jumping next to it  
// aim mode camera rotation doesnt work
// hyper specific bug but technically if you were to be standing on a big platform that is a rigid body and it was orbiting an orbital gravity source your orientation would go weirdo. really true if gravity is changing at all. 
// when gravity changes while you are stationary you dont rotate relative to the gravity source unless you move ( really the same issue as above)
// maybe overhaul everything to be based on the characters velocity rather than just the inputs, that may make for a more elegant system
// if you jump after falling off a platform the jump animation plays but you dont jump (this is greatly alleviated, but technically if you jump right before you switch to the falling animation this can still happen)
// small jitters when running along a sphere gravity (update steps can alleviate this)
// small jitters when turning or rotating, this one is subtle but it seems to be true for all rigidbodies, i think that is what is causing it on my character (update steps can alleviate this)

//easy fixes: 
//you can entter the walking movement state while still being the in the sprinting animatin
// shouldnt have animations tied to buttons, but instead input axis

// Wanted features
// ground punch animation for explosive force
// switch to a cinemachine camera
// make wall climbing jumps contextual. climbing idle jump-> jump is perpendicular to wall, climbing left jump -> jump along wall to the left, etc
// blendspace to allow 45 degree climbing animations through blending right and up or whatever
// wall running state needs to differenciate from falling, as of right now youce just falling along the wall
// fall damage
// mario esque triple jump 
// increase downward acelleration on the way down after a jump ?
// little skid when you rapidly change directions
// gun shooting mode
// Gravity Gun
// Phys gun?
// crouching state
// prop grabbing / spinning / hook prop pickup
// interactable NPC's. Speaking, roaming, fighting, etc.
// Ragdoll gun, let me ragdoll any NPC and myself
// a "pushing props" state, new animation set for when you are running up against something but not moving, or maybe moving below expected speeds, ie pushing a rigid body or running into a wall. Should probably enforce walk speed in that case. 

// NO-NO's for mall Kid overhaul:
// make sure you have a "isMoving" bool that checks if isWalking, isRunning, OR isSprinting is true
// get your IK/weight painting solid before trying to put character in game

using UnityEngine;

[RequireComponent(typeof(Light))]
public class GenericMovingSphere : MonoBehaviour { 

	[SerializeField, Range(0.01f, 1f)]
	public float swimThreshold = 0.5f;
	public bool Swimming => submergence >= swimThreshold;

	[SerializeField, Range(90, 180)]
	float maxClimbAngle = 140f;

	Vector3 lastContactNormal, lastSteepNormal;

	AnimationStateController controller;
	// this is used to edit the light on the fly
	Light lt;
	// this is so i can get a refrence to the empty that is a child of the main game object
	public GameObject parent;
	//these are for the booms
	public float radius = 20.0F;
    public float power = 100.0F;

	[SerializeField]
	[Tooltip("determines what rotation is relative to, ideally the camera")]
	Transform playerInputSpace = default;

	float minGroundDotProduct, minStairsDotProduct, minClimbDotProduct;

	[SerializeField, Min(0f)]
	float probeDistance = 1f;

	[SerializeField, Range(0f, 100f)]
	float maxSnapSpeed = 100f;

	[SerializeField, Range(0f, 90f)]
	float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Range(0f, 100f)]
	[Tooltip("speeds of the character, these three states represent the speed when your character is jogging, sprinting, and walking")]
	float baseSpeed = 10f, sprintSpeed = 25f, maxClimbSpeed = 2f, maxSwimSpeed = 5f;

	public float maxSpeed;

	[SerializeField, Range(0f, 100f)]
	[Tooltip("how quickly your character responds to input")]
	float maxAcceleration = 10f, maxAirAcceleration = 1f, maxClimbAcceleration = 20f, maxSwimAcceleration = 5f;

	[SerializeField, Range(0f, 100f)]
	[Tooltip("character's jump height")]
	float jumpHeight = 2f;

	[SerializeField, Range(0, 5)]
	[Tooltip("controls the amount of jumps you can do while in the air")]
	int maxAirJumps = 1;

	[SerializeField]
	LayerMask probeMask = -1, stairsMask = -1, climbMask = -1, waterMask = 0;
	
	[HideInInspector]
	public Rigidbody body, connectedBody; 
	Rigidbody previousConnectedBody;

	bool desiredJump, desiresClimbing;

	[HideInInspector]
	public int groundContactCount, steepContactCount, climbContactCount;

	bool gravSwap;

	[HideInInspector]
	public bool isAiming;

	public bool OnGround {
		get {
			return groundContactCount > 0;
		}
	}

	public bool OnSteep {
		get {
			return steepContactCount > 0;
		}
	}
	public bool Climbing => climbContactCount > 0 && stepsSinceLastJump > 2;
	int jumpPhase;
	bool InWater => submergence > 0f;
	public float submergence;
	int stepsSinceLastGrounded, stepsSinceLastJump;

	[SerializeField, Range(0f, 10f)]
	float waterDrag = 1f;

	Vector3 contactNormal, steepNormal, lastClimbNormal;

	[HideInInspector]
	public Vector3 climbNormal;
	Vector3 upAxis, rightAxis;
	[HideInInspector]
	public Vector3 forwardAxis;

	Vector3 connectionWorldPosition, connectionLocalPosition;
	[HideInInspector]
	public Vector3 playerInput;
	[HideInInspector]
	public Vector3 velocity; 
	Vector3 connectionVelocity;
	//public bool WalkPressed;

	[SerializeField]
	float submergenceOffset = 1.5f;

	[SerializeField, Min(0.1f)]
	float submergenceRange = 3f;

	[SerializeField, Min(0f)]
	float buoyancy = 1f;

	// this is so i can prevent the player from entering a climbing state while standing on the ground
	[HideInInspector]
	public bool ClimbingADJ;

	void Awake () {
		// this is so i can prevent the player from entering a climbing state while standing on the ground
		if(Climbing && !OnGround){
			ClimbingADJ = true;
		}
		else{
			ClimbingADJ = false;
		}
		// gets a refrence to the animation state controller, its pinned to the mesh thats a child of the main player thats why it looks weird
		//controller = parent.transform.GetChild(0).GetComponent<AnimationStateController>();
		//get the light
		lt = GetComponent<Light>();
		//get the rigidbody
		body = GetComponent<Rigidbody>();
		//turn gravity off for the rigid body
		body.useGravity = false;
		//call validate ?
		OnValidate();
	}
	void Update () {

		// this is so i can prevent the player from entering a climbing state while standing on the ground
		if(Climbing && !OnGround){
			ClimbingADJ = true;
		}
		else{
			ClimbingADJ = false;
		}
		if (Swimming) {
			desiresClimbing = false;
		}
		else {
			desiredJump |= Input.GetButtonDown("Jump");
			desiresClimbing = Input.GetButton("Climb");
		}
		isAiming = Input.GetKey(KeyCode.Mouse1);
		//Debug.Log(hp);
		//Debug.Log(coins);
		ExplosiveForce();
		MovementState();
		// light that shows if youre on the ground or not
		if (Swimming){
			lt.color = Color.blue;
		}
		else if (OnGround){
			lt.color = Color.red;
		}
		else if (ClimbingADJ){
			lt.color = Color.white;
		}
		else if (OnSteep){
			lt.color = Color.yellow;
		}
		else if (!OnSteep && !OnGround && !Swimming){
			lt.color = Color.green;
		}
	    playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
    	playerInput.z = Swimming ? Input.GetAxis("UpDown") : 0f;
		playerInput = Vector3.ClampMagnitude(playerInput, 1f);

		if (playerInputSpace) {
			rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
			forwardAxis =
				ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
		}
		else	{
			rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
			forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
		}
		//UpdateRotation();
	}

	bool CheckSwimming () {
		if (Swimming) {
			groundContactCount = 0;
			contactNormal = upAxis;
			return true;
		}
		return false;
	}

	void OnTriggerEnter (Collider other) {
		if ((waterMask & (1 << other.gameObject.layer)) != 0) {
			EvaluateSubmergence(other);
		}
	}

	void OnTriggerStay (Collider other) {
		if ((waterMask & (1 << other.gameObject.layer)) != 0) {
			EvaluateSubmergence(other);
		}
	}

	void EvaluateSubmergence (Collider collider) {
		if (Physics.Raycast(
			body.position + upAxis * submergenceOffset,
			-upAxis, out RaycastHit hit, submergenceRange + 1f,
			waterMask, QueryTriggerInteraction.Collide
		)) {
			submergence = 1f - hit.distance / submergenceRange;
		}
		else {
			submergence = 1f;
		}
		if (Swimming) {
			connectedBody = collider.attachedRigidbody;
		}
	}
// Climbing
	bool CheckClimbing () {
		if (ClimbingADJ) {
			if (climbContactCount > 1) {
				climbNormal.Normalize();
				float upDot = Vector3.Dot(upAxis, climbNormal);
				if (upDot >= minGroundDotProduct) {
					climbNormal = lastClimbNormal;
				}
			}
			groundContactCount = 1;
			contactNormal = climbNormal;
			return true;
		}
		return false;
	}

	void FixedUpdate() {
		Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);
		UpdateState();
		if (InWater) {
			velocity *= 1f - waterDrag * submergence * Time.deltaTime;
		}
		AdjustVelocity();
		if (desiredJump) {
			desiredJump = false;
			Jump(gravity);
		}
		if (ClimbingADJ && !OnGround) {
			// = false;
			velocity -= contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
		}
		else if (InWater) {
			velocity +=
				gravity * ((1f - buoyancy * submergence) * Time.deltaTime);
		}
		else if (OnGround && velocity.sqrMagnitude < 0.01f) {
			//WalkPressed = false;
			velocity +=
				contactNormal *
				(Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
		}
		else if (desiresClimbing && OnGround) {
			//WalkPressed = true;
			velocity +=
				(gravity - contactNormal * (maxClimbAcceleration * 0.9f)) *
				Time.deltaTime;
		}
		else {
			//WalkPressed = false;
			velocity += gravity * Time.deltaTime;
		}
		body.velocity = velocity;
		ClearState();
	}

	bool CheckSteepContacts () {
		if (steepContactCount > 1) {
			steepNormal.Normalize();
			float upDot = Vector3.Dot(upAxis, steepNormal);
			if (upDot >= minGroundDotProduct) {
				groundContactCount = 1;
				contactNormal = steepNormal;
				return true;
			}
		}
		return false;
	}

	float GetMinDot (int layer) {
		return (stairsMask & (1 << layer)) == 0 ?
			minGroundDotProduct : minStairsDotProduct;
	}

	bool SnapToGround () {
		if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2) {
			return false;
		}
		float speed = velocity.magnitude;
		if (speed > maxSnapSpeed) {
			return false;
		}
		if (!Physics.Raycast(
			// i changed the first argument to be  the "feet" empty tied to the player character. this may be causing jitters (parent.transform.GetChild(1))
			this.transform.position, -upAxis, out RaycastHit hit,
			probeDistance, probeMask, QueryTriggerInteraction.Ignore
			)) {
			return false;
		}
		float upDot = Vector3.Dot(upAxis, hit.normal);
		if (upDot < GetMinDot(hit.collider.gameObject.layer)) {
			return false;
		}
		groundContactCount = 1;
		contactNormal = hit.normal;
		float dot = Vector3.Dot(velocity, hit.normal);
		if (dot > 0f) {
		velocity = (velocity - hit.normal * dot).normalized * speed;
		}

		connectedBody = hit.rigidbody;
		return true;
	}

	void OnValidate () {
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
		minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
		minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
	}

	void ExplosiveForce(){
		bool BoomPressed = Input.GetKeyDown("v");
		if(BoomPressed){
			Vector3 explosionPos = transform.position;
        	Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        	foreach (Collider hit in colliders)
        	{
            	Rigidbody rb = hit.GetComponent<Rigidbody>();
				if (rb != body){
					if (rb != null)
						rb.AddExplosionForce(power, explosionPos, radius);
					}
				}
		}
	}
	void MovementState(){
		// put this in adjust velocity?
		//this allows me to control movement speed based on key presses. the same key checks are happening in the animation controller.
		//walk pressed is controlled in the climbing script now
		bool SprintPressed = Input.GetKey("left shift");
		bool forwardPressed = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
		// default situation
		if ((!SprintPressed) && forwardPressed){
			maxSpeed = baseSpeed;
		}
		//sprinting
        if (forwardPressed && SprintPressed){
			maxSpeed = sprintSpeed;
        }
		//walking
		//if (forwardPressed && WalkPressed && !SprintPressed){
			//maxSpeed = walkSpeed;
		//}		
	}

	void ClearState (){
		lastContactNormal = contactNormal;
		lastSteepNormal = steepNormal;
		groundContactCount = steepContactCount = climbContactCount = 0;
		contactNormal = steepNormal = climbNormal = connectionVelocity = Vector3.zero;
		previousConnectedBody = connectedBody;
		connectedBody = null;
		submergence = 0f;
		lastContactNormal = contactNormal;
	}

	public void PreventSnapToGround () {
		stepsSinceLastJump = -1;
		Debug.Log("HELLO");
	}

	void UpdateState(){
		if (connectedBody) {
			if (connectedBody.isKinematic || connectedBody.mass >= body.mass) {
				UpdateConnectionState();
			}
		}
		stepsSinceLastGrounded += 1;
		stepsSinceLastJump += 1;
		velocity = body.velocity;
		if (CheckClimbing() || CheckSwimming() || OnGround || SnapToGround() || CheckSteepContacts()){
			stepsSinceLastGrounded = 0;
			if (stepsSinceLastJump > 1) {
				jumpPhase = 0;
			}
			if (groundContactCount > 1){
				contactNormal.Normalize();
			}
			contactNormal.Normalize();
		}
		else {
			contactNormal = upAxis;
		}
	}

	void UpdateConnectionState () {
		if (connectedBody == previousConnectedBody) {
			Vector3 connectionMovement =
				connectedBody.transform.TransformPoint(connectionLocalPosition) - 
				connectionWorldPosition;
			connectionVelocity = connectionMovement / Time.deltaTime;
			connectionWorldPosition = body.position;
			connectionLocalPosition = connectedBody.transform.InverseTransformPoint(
				connectionWorldPosition
			);
		}
	}

	public void JumpTrigger(){

		desiredJump = true;
	}
	
	void Jump(Vector3 gravity) {
			
			Vector3 jumpDirection;
			//this was originally just "OnGround" but i switched it to be the adjusted one so you can jump a little after going off the ledge
			if (submergence < 1){
				jumpDirection = contactNormal;
			}
			else if (controller.isOnGroundADJ) {
				jumpDirection = contactNormal;
			}
			else if (OnSteep) {
				desiresClimbing = false;
				jumpDirection = steepNormal;
				jumpPhase = 0;
			}
			else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps) {
				if (jumpPhase == 0) {
				jumpPhase = 1;
				}
				jumpDirection = contactNormal;
				}
			else {
				return;
			}
			stepsSinceLastJump = 0;
			jumpPhase += 1;
			float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
			//This slows down your jump speed based on how far you are in water, Im gonna comment it out for now so that water jumps work better
			//if (InWater) {
			//jumpSpeed *= Mathf.Max(0f, 1f - submergence / swimThreshold);
			//}
			jumpDirection = (jumpDirection + upAxis).normalized;
			float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
			if (alignedSpeed > 0f) {
				jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
			}
			velocity += jumpDirection * jumpSpeed;
		}

	void OnCollisionEnter (Collision collision) {
		EvaluateCollision(collision);
	}

	void OnCollisionStay (Collision collision) {
		EvaluateCollision(collision);
	}
	void EvaluateCollision (Collision collision) {
		int layer = collision.gameObject.layer;
		float minDot = GetMinDot(layer);
			for (int i = 0; i < collision.contactCount; i++) {
			Vector3 normal = collision.GetContact(i).normal;
			float upDot = Vector3.Dot(upAxis, normal);
			//This was just > than for conor and i, but on the tutorial it was changed to >= without any explanation so keep that in mind
			if (upDot >= minDot) {
				connectedBody = collision.rigidbody;
				groundContactCount += 1;
				contactNormal += normal;
			}
			else {
				if (upDot > -0.01f) {
					steepContactCount += 1;
					steepNormal += normal;
					if (groundContactCount == 0) {
						connectedBody = collision.rigidbody;
					}
				}
				if (desiresClimbing && upDot >= minClimbDotProduct&&
					(climbMask & (1 << layer)) != 0)
					{
					climbContactCount += 1;
					climbNormal += normal;
					lastClimbNormal = normal;
					connectedBody = collision.rigidbody;
				}

			}
		}
	}
// these two statements are equal (question mark notation reminder)


//	movement *= speed * ( ( absJoyPos.x > absJoyPos.y ) ? absJoyPos.x : absJoyPos.y );

//	movement *= speed;
//	If( absJoyPos.x > absJoyPos.y )
//	{
//	movement *= absJoyPos.x;
//	}
//	else
//	{
//	movement *= absJoyPos.y;
//	}

// basically a = b ? c:d; means a is either c or d depending on b, or 
// if(b){
// a = c
// }
// else {
// a = d
// }

	void AdjustVelocity () {
		float acceleration, speed;
		Vector3 xAxis, zAxis;
		if (ClimbingADJ) {
			acceleration = maxClimbAcceleration;
			speed = maxClimbSpeed;
			xAxis = Vector3.Cross(contactNormal, upAxis);
			zAxis = upAxis;

			
		}

		else if (InWater) {
			float swimFactor = Mathf.Min(1f, submergence / swimThreshold);
			acceleration = Mathf.LerpUnclamped(
				OnGround ? maxAcceleration : maxAirAcceleration,
				maxSwimAcceleration, swimFactor
			);
			speed = Mathf.LerpUnclamped(maxSpeed, maxSwimSpeed, swimFactor);
			xAxis = rightAxis;
			zAxis = forwardAxis;
		}

		else {
			acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
			speed = OnGround && desiresClimbing ? maxClimbSpeed : maxSpeed;
			xAxis = rightAxis;
			zAxis = forwardAxis;
		}
		
		xAxis = ProjectDirectionOnPlane(xAxis, contactNormal);
		zAxis = ProjectDirectionOnPlane(zAxis, contactNormal);

		Vector3 relativeVelocity = velocity - connectionVelocity;

		float currentX = Vector3.Dot(relativeVelocity, xAxis);
		float currentZ = Vector3.Dot(relativeVelocity, zAxis);

		float maxSpeedChange = acceleration * Time.deltaTime;

		float newX =
			Mathf.MoveTowards(currentX, playerInput.x * speed, maxSpeedChange);
		float newZ =
			Mathf.MoveTowards(currentZ, playerInput.y * speed, maxSpeedChange);

		velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

		
		if (Swimming) {
			float currentY = Vector3.Dot(relativeVelocity, upAxis);
			float newY = Mathf.MoveTowards(
				currentY, playerInput.z * speed, maxSpeedChange
			);
			velocity += upAxis * (newY - currentY);
		}
	}
	public Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}
}
