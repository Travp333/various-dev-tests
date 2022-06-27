using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour {

	[SerializeField]
	Vector3 buoyancyOffset = Vector3.zero;
    float submergence;
	Vector3 gravity;
    [SerializeField]
	float submergenceOffset = 0.5f;

	[SerializeField, Min(0.1f)]
	float submergenceRange = 1f;

	[SerializeField, Min(0f)]
	float buoyancy = 1f;

	[SerializeField, Range(0f, 10f)]
	float waterDrag = 1f;

	[SerializeField]
	LayerMask waterMask = 0;
    [SerializeField]
    bool floatToSleep = false;
    float floatDelay;
    Renderer color;

	Rigidbody body;
	Vector3 DummyGrav;
	bool showSleep = false;

	void Awake () {
        color = GetComponent<Renderer>();
		body = GetComponent<Rigidbody>();
		body.useGravity = false;
	}
	void Start() {
		DummyGrav = CustomGravity.GetUpAxis(body.position);
	}
    void FixedUpdate () {

		//basically i am tryign to ensure that rigidbodies will respond to gravity changes by creating a bool that represents whether gravity is changing or not and waking the body accordingly
		bool gravSwap;
		if(DummyGrav == CustomGravity.GetGravity(body.position)){
			gravSwap = false;
		}
		else{
			gravSwap = true;
			DummyGrav = CustomGravity.GetGravity(body.position);
		}

        if (floatToSleep){
            if (body.IsSleeping()) {
				//I added this gravswap check to ensure ridigbodies are always aware of gravity changes
				if(gravSwap){
					body.WakeUp();
				}
				else{
					floatDelay = 0f;
					return;
				}
            }
            if (body.velocity.sqrMagnitude < 0.0001f) {
                floatDelay += Time.deltaTime;
                if (floatDelay >= 1f) {
                    return;
                }
				if(showSleep){
					color.material.SetColor("_EmissionColor", Color.yellow);
				}
            }
            else {              
                floatDelay = 0f;
				if(showSleep){
					color.material.SetColor("_EmissionColor", Color.red);
				}
            }
		gravity = CustomGravity.GetGravity(body.position);
		if (submergence > 0f) {
            float drag =
				Mathf.Max(0f, 1f - waterDrag * submergence * Time.deltaTime);
			body.velocity *= drag;
            body.angularVelocity *= drag;
			body.AddForceAtPosition(
				gravity * -(buoyancy * submergence),
				transform.TransformPoint(buoyancyOffset),
				ForceMode.Acceleration
			);
			submergence = 0f;
		}
		body.AddForce(gravity, ForceMode.Acceleration);
	    }
    }
    void OnTriggerEnter (Collider other) {
		if ((waterMask & (1 << other.gameObject.layer)) != 0) {
			EvaluateSubmergence();
		}
	}

	void OnTriggerStay (Collider other) {
		if (
            !body.IsSleeping() && (waterMask & (1 << other.gameObject.layer)) != 0)
        {
			EvaluateSubmergence();
		}
	}
	
	void EvaluateSubmergence () {
		Vector3 upAxis = -gravity.normalized;
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
	}
}
