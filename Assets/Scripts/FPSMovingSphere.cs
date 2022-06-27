using UnityEngine;

[RequireComponent(typeof(Light))]
public class FPSMovingSphere : MonoBehaviour { 
	Grab grab;
	[SerializeField]
	[Tooltip("How strong the force pushing you into the wall is while climbing")]
	float climbStickyness = 5f;
	MovementSpeedController speedController;
	public bool Diving;
	Vector3 jumpDirection;
	
	[HideInInspector]
	public bool isBarraging;

	[SerializeField, Range(0.01f, 1f)]
	public float swimThreshold = 0.5f;
	public bool Swimming => submergence >= swimThreshold;

	[SerializeField, Range(90, 180)]
	float maxClimbAngle = 140f;

	Vector3 lastContactNormal, lastSteepNormal;

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

	[SerializeField]
	float submergenceOffset = 1.5f;

	[SerializeField, Min(0.1f)]
	float submergenceRange = 3f;

	[SerializeField, Min(0f)]
	float buoyancy = 1f;
	Animator camanim;
	
	// this is so i can prevent the player from entering a climbing state while standing on the ground
	[HideInInspector]
	public bool ClimbingADJ;
	[HideInInspector]
	public bool divingPrep;

	bool skip = true;

	bool diveGate;

	void Awake () {
		grab = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Grab>();
		speedController = GetComponent<MovementSpeedController>();
		camanim = transform.GetChild(0).GetComponent<Animator>();
		// this is so i can prevent the player from entering a climbing state while standing on the ground
		if(Climbing && !OnGround){
			ClimbingADJ = true;
		}
		else{
			ClimbingADJ = false;
		}
		// gets a refrence to the animation state controller, its pinned to the mesh thats a child of the main player thats why it looks weird
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
		if(OnGround || ClimbingADJ){
			if (!diveGate){
				Diving = false;
			}
		}
        if(Input.GetButtonDown("Duck")){
            camanim.SetBool("divePrep", true);
			divingPrep = true;
        }
        if(Input.GetButtonUp("Duck")){
            camanim.SetBool("divePrep", false);
			divingPrep = false;
        }
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
		if(grab.isHolding){
			desiresClimbing = false;
			desiredJump |= Input.GetButtonDown("Jump");
		}
		else {
			desiredJump |= Input.GetButtonDown("Jump");
			desiresClimbing = Input.GetButton("Duck");
		}
		ExplosiveForce();
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
		// what is this? does it ever get called?
		//if (ClimbingADJ && !OnGround) {
		//	Debug.Log("Climbing!");
		//	velocity -= contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
		//}
		else if (InWater) {
			velocity +=
				gravity * ((1f - buoyancy * submergence) * Time.deltaTime);
		}
		else if (OnGround && velocity.sqrMagnitude < 0.01f) {
			velocity +=
				contactNormal *
				(Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
		}

		else if (desiresClimbing && ClimbingADJ && !grab.isHolding) {
			velocity +=
				(gravity - contactNormal * (maxClimbAcceleration * climbStickyness)) *
				Time.deltaTime;
		}
		else {
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
			parent.transform.GetChild(3).position, -upAxis, out RaycastHit hit,
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
	}

	void UpdateState(){
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
		if (connectedBody) {
			if (connectedBody.isKinematic || connectedBody.mass >= body.mass) {
				UpdateConnectionState();
			}
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

	void resetDiveGate(){
		diveGate = false;
	}

	public void hungerDive(){
		if(!OnGround && Diving &&!ClimbingADJ){
			PreventSnapToGround();
			jumpDirection = contactNormal + transform.forward * 3f;
			body.velocity = new Vector3( 0f, 0f, 0f);
			//body.velocity += new Vector3( 0f, -body.velocity.y, 0f);
			body.velocity += (jumpDirection.normalized * 6f) + (-CustomGravity.GetGravity(body.position, out upAxis).normalized * 7f);
			skip = false;
		}
	}
	
	void Jump(Vector3 gravity) {
			if (submergence < 1){
				jumpDirection = contactNormal;
			}
			if(divingPrep && OnGround && !ClimbingADJ){
				Diving = true;
				PreventSnapToGround();
				jumpDirection = contactNormal + transform.forward * 3f;
				velocity += (jumpDirection.normalized * 25f) + (contactNormal * 2);
				skip = false;
				diveGate = true;
				Invoke("resetDiveGate", .5f);
			}
			else if (OnGround) {
				jumpDirection = contactNormal;
			}
			else if (OnSteep) {
				desiresClimbing = false;
				jumpDirection = steepNormal;
				// this was originally 0 but i changed it so that wall jumping doesnt count as one of your air jumps
				jumpPhase -= 1;
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

			if (skip){
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
			else{
				skip = true;
			}
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
			speed = speedController.maxClimbSpeed;
			xAxis = Vector3.Cross(contactNormal, upAxis);
			zAxis = upAxis;
		}
		else if (InWater) {
			float swimFactor = Mathf.Min(1f, submergence / swimThreshold);
			acceleration = Mathf.LerpUnclamped(
				OnGround ? maxAcceleration : maxAirAcceleration,
				maxSwimAcceleration, swimFactor
			);
			speed = Mathf.LerpUnclamped(speedController.currentSpeed, speedController.maxSwimSpeed, swimFactor);
			xAxis = rightAxis;
			zAxis = forwardAxis;
		}

		else {
			acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
			speed = OnGround && desiresClimbing ? speedController.walkSpeed : speedController.currentSpeed;
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
