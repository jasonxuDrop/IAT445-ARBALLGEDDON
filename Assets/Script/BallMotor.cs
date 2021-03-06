using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMotor : MonoBehaviour
{
	protected Rigidbody rb;
	public float gravityScale = 0.1f;
	public AnimationCurve speedDownCurve;
	public float speedDownDuration;

	protected float timeSinceMoved = -1f;
	[HideInInspector] public float maxSpeed;

	protected bool updateVelocity;
	protected Vector3 toVelocity = new Vector3();

	[HideInInspector] public float frameCountBeforeSimulationBreak = 0; // updated in prediction line manager only


	private void Awake() {
		if (!rb) {
			rb = GetComponent<Rigidbody>();
		}
	}
	private void Update() {
		if (!rb) {
			rb = GetComponent<Rigidbody>();
		}
	}

	public virtual void Move(Vector3 force) {
		rb.AddForce(force, ForceMode.VelocityChange);
		timeSinceMoved = 0;
	}
	public virtual void Move(Vector3 force, float _timeSinceMoved) {
		rb.AddForce(force, ForceMode.VelocityChange);
		timeSinceMoved = _timeSinceMoved;
	}
	public virtual void SetTimeSinceMoved(float toTime) {
		timeSinceMoved = toTime;
	}
	public float GetSpeedRatio() {
		//return rb.velocity.magnitude / maxSpeed;
		if (HasStoppedMoving())
			return 0;
		else return Mathf.Clamp01((speedDownDuration-timeSinceMoved) / speedDownDuration);
	}

	public virtual bool HasStoppedMoving() {
		return (timeSinceMoved < 0
			|| rb.velocity.sqrMagnitude < 0.003f);
	}

	public virtual void FixedUpdate() {
		// custome gravity
		ApplyGravity();

		// eventrual full stop
		if (timeSinceMoved > speedDownDuration) {
			timeSinceMoved = -1f;
			rb.velocity *= 0;
		}
		else if (timeSinceMoved >= 0) {
			float dampFactor = speedDownCurve.Evaluate(timeSinceMoved / speedDownDuration);
			rb.velocity *= dampFactor;
			timeSinceMoved += Time.fixedDeltaTime;
		}
	}

	protected virtual void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Wall" && frameCountBeforeSimulationBreak == 0) {
			var contact = collision.GetContact(0);

			// try to change the velocity directly
			toVelocity = Vector3.Reflect(rb.velocity, contact.normal);
			rb.velocity = toVelocity;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
	}


	public void ApplyGravity() {
		Vector3 gravity = -9.81f * gravityScale * Vector3.up;
		rb.AddForce(gravity, ForceMode.Acceleration);
	}
}
